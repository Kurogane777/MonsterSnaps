using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoBook : MonoBehaviour
{
    [SerializeField] InputController inputC;

    public List<Picture> allPictures = new List<Picture>();//contains all your pictures, should be stored on the gamecontroller and not here

    public GameObject displayPrefab;//the UI prefab for displaying an image
    public Transform page1, page2;
    public Transform pagePlace;//where to create the prefabs
    public Transform pagePlace2;
    public int page = 0;//the page we are on
    public int pageSize;//how many items fit on a single page
    bool open;
    public Text pageNumber;
    public static PhotoBook main;

    public bool OffOnPage;
    private void Awake()
    {
        main = this;
    }
    void Start()
    {
        page1.gameObject.SetActive(open);
        page2.gameObject.SetActive(open);
        //Populate();//call this every time you change page
    }
    public void Update()
    {
        int maxPages = (allPictures.Count / (pageSize * 2)) * 2;
        ChangePage();
        pageNumber.text = $"{page / 2}/{maxPages/2}";
    }

    public void OffChangePage()
    {
        OffOnPage = false;
    }

    public void OnChangePage()
    {
        OffOnPage = true;
    }

    void ChangePage()
    {
        if (OffOnPage)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                page = Mathf.Max(0, page - 2);
                Populate();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                page = Mathf.Min((allPictures.Count / (pageSize * 2)) * 2, page + 2);
                Populate();
            }
        }
    }

    public void Open()
    {
        open = true;
        page1.gameObject.SetActive(open);
        page2.gameObject.SetActive(open);
        Populate();

    }
    public void Close()
    {
        if (!open)
            return;
        open = false;
        page1.gameObject.SetActive(open);
        page2.gameObject.SetActive(open);
    }
    public void Toggle()
    {
        open = !open;
        page1.gameObject.SetActive(open);
        page2.gameObject.SetActive(open);
        if (open)
            Populate();
    }

    public void PreviousPage()
    {
        if (allPictures != null && allPictures.Count > 0)
        {
            page = (page - 1) % pageSize * 2;//*2 because we have 2 pages right now
            Populate();
        }
    }

    public void NextPage()
    {
        if (allPictures!=null && allPictures.Count > 0)
        {
            page = (page + 1) % pageSize * 2;//*2 because we have 2 pages right now
            Populate();
        }
    }
    public void Populate()
    {
        foreach (Transform child in pagePlace)//clears any previously existing
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GetPage(page,allPictures))//creates all the images you need
        {
            var obj = Instantiate(displayPrefab, pagePlace);//create the prefab
            //set the image and whatever you want here (im using text for an example)
            obj.transform.GetChild(1).GetComponent<Text>().text = item.name;
            Debug.Log(item.name);
            obj.transform.GetChild(3).GetComponent<Text>().text = item.GetStars();
            obj.transform.GetChild(0).GetComponent<RawImage>().texture = item.texture;
        }
        //incase you wanted 2 pages
        foreach (Transform child in pagePlace2)//clears any previously existing
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GetPage(page+1,allPictures))//creates all the images you need
        {
            var obj = Instantiate(displayPrefab, pagePlace2);//create the prefab
            //set the image and whatever you want here (im using text for an example)
            obj.transform.GetChild(1).GetComponent<Text>().text = item.name;
            obj.transform.GetChild(3).GetComponent<Text>().text = item.GetStars();
            obj.transform.GetChild(0).GetComponent<RawImage>().texture = item.texture;
        }
    }
    List<Picture> GetPage(int pageIndex, List<Picture> pool)//returns a list of items for that page index
    {
        if (pageSize < 1)//page size needs to be higher than none and non-negative
            pageSize = 1;
        int pageCount = Mathf.CeilToInt((float)pool.Count / pageSize);//how many pages you have
        pageIndex = Mathf.Clamp(pageIndex,0,pageCount);//keep it in range
        var page = new List<Picture>();// create a new list to return
        for (int i = 0; i < pageSize; i++)
        {
            int index = (pageIndex * pageSize) + i;// gets the indexof the group
            if (index > pool.Count - 1)// cancel if we have reached the end
                break;
            page.Add(pool[index]);//add it to the return list
        }
        return page;
    }
}
[System.Serializable]//makes it visible in the inspector
public class Picture
{
    public string name;//i just used this for my example its not needed really
    public List<CaptureTarget> targets;//a list of all objects in the image, set when you take the picture (this could be something other than a string if you want)
    public CaptureTarget mainTarget;
    public Texture2D texture;
    public float score = 1;
    //any other data you want in here
    public Picture(Texture2D texture, List<CaptureTarget> targets)
    {
        this.texture = texture;
        this.targets = targets;
        GetScore();
    }
    void GetScore()
    {
        float m = 1;
        if (targets.Count > 0)
        {
            foreach (var targ in targets)
            {
                if (m > targ.unvisibility)
                {
                    m = targ.unvisibility;
                    mainTarget = targ;
                    name = targ.name;
                }
            }
        }
        score = m;
    }
    public string GetStars()
    {
        float v = 1 - score;
        string s = "";
        for (int i = Mathf.RoundToInt(v * 5); i >= 0; i--)
        {
            s += "*";
        }

        Debug.Log((Mathf.RoundToInt(score * 5),s));
        return s;
    }
}
[System.Serializable]
public class CaptureTarget
{
    public string name;
    public float unvisibility;
    public CaptureTarget(string name, float visibility)
    {
        this.unvisibility = visibility;
        this.name = name;
    }
}
