using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    [Header("Target Detection")]
    [SerializeField] private int sampleCount = 150;//how many samples to use when detecting, a number too high could cause lag spikes
    private Texture2D screenCapture;
    private bool viewingPhoto;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!viewingPhoto) 
            {
                StartCoroutine(CapturePhoto());
            }
            else 
            {
                RemovePhoto();
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        cameraUI.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        //make a new tex2D to stop duplication
        var newTex = new Texture2D(screenCapture.width, screenCapture.height);
        newTex.SetPixels(screenCapture.GetPixels());
        newTex.Apply();//remember to apply the changes when you update a texture
        var caps = GetTargets(Camera.main);
        PhotoBook.main.allPictures.Add(new Picture(newTex,caps));
        MonsterDex.main.Captured(caps);
        ShowPhoto();
    }

    public List<CaptureTarget> GetTargets(Camera cam)
    {
        List<CaptureTarget> targs = new List<CaptureTarget>();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");//whatever the target's tag is
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        foreach (GameObject obj in taggedObjects)
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend)
            {
                var bounds = rend.bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))//is it in the camera's frustum
                {
                    int failures = 0;//how many tests dont hit the target (something is blocking)
                    int tests = sampleCount;//the amounts of tests to do
                    int trueTests = tests;//the amount of tests that were actually on target
                    float dist = Vector3.Distance(cam.transform.position, obj.transform.position);
                    for (int i = 0; i < tests; i++)
                    {
                        var p = bounds.center + MultiplyVector(Random.insideUnitSphere, bounds.size);//distribute the tests across the bounds of the collider
                        var dir = p - cam.transform.position;
                        // you can put a layermask into the following line's RayCastAll to block some layers from triggering the blockage
                        var hits = Physics.RaycastAll(cam.transform.position, dir.normalized, dir.magnitude);//get all colliders along a path/ray
                        bool onTarget = false;//has the ray actually hit the target
                        Vector3 targetHit = Vector3.zero;
                        bool blocked = false;
                        List<Vector3> blockedHits = new List<Vector3>();//all blocking hits
                        foreach (var hit in hits)//for each of the colliders hit
                        {
                            if (hit.collider.gameObject == obj)//did we hit the target
                            {
                                onTarget = true;
                                targetHit = hit.point;
                                Debug.DrawRay(cam.transform.position, dir, Color.green * new Color(1, 1, 1, 0.5f));
                                //Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.2f);
                            }
                            else
                            {
                                blocked = true;
                                blockedHits.Add(hit.point);
                            }
                        }
                        if (onTarget)
                        {
                            if (blocked && blockedHits.Count > 0)
                            {
                                foreach (var block in blockedHits)
                                {
                                    if (Vector3.Dot(dir.normalized, (block - targetHit).normalized) < 0.95f)//the blocked hit isnt behind the target
                                    {
                                        Debug.DrawRay(block, Vector3.up * 0.1f, Color.red, 0.2f);
                                        failures++;
                                        break;
                                    }
                                    else
                                    {
                                        Debug.DrawRay(block, Vector3.up * 0.1f, Color.yellow, 0.2f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            trueTests--;
                        }
                    }
                    float visibility = trueTests == 0 ? 1 : ((float)failures / (float)trueTests);//make sure to account for all misses
                    if (obj.TryGetComponent(out nameCharMonster name))
                    {
                        string n = name.objName;
                        if (string.IsNullOrEmpty(n))
                            n = "NO_NAME_FOUND";
                        targs.Add(new CaptureTarget(n, visibility));
                    }
                }
            }
        }
        return targs;
    }
    public static Vector3 MultiplyVector(Vector3 a, Vector3 b)//just multiplies the values of two vectors
    {
        Vector3 v = new Vector3(a.x * (b.x), a.y * (b.y), a.z * (b.z));
        return v;
    }



    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        StartCoroutine(CameraFlashEffect());
        fadingAnimation.Play("PhotoFade");
    }

    IEnumerator CameraFlashEffect()
    {
        cameraAudio.Play();
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
        cameraUI.SetActive(true);
    }
}
