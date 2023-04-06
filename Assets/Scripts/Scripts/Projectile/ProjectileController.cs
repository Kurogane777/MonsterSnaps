using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //public float rotationSpeed = 1;
    public float projectileForce = 5;

    //public float mouseSensitivity = 100f;

    //[Space]
    //public Vector2 _horizontalMinMax = new Vector2(40, 133);
    //public Vector2 _verticalMinMax = new Vector2(0, 25);

    [Space]
    public GameObject bullet;
    public Transform shotPoint;

    private void Update()
    {
        ////float HorizontalRotation = Input.GetAxis("Horizontal");
        ////float VericalRotation = Input.GetAxis("Vertical");

        //float HorizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float VericalRotation = (Input.GetAxis("Mouse Y") * -1) * mouseSensitivity * Time.deltaTime;

        ////transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalRotation * rotationSpeed, VericalRotation * rotationSpeed));

        //var tRotation = transform.rotation.eulerAngles + new Vector3(0, HorizontalRotation * rotationSpeed, VericalRotation * rotationSpeed);

        //// Limit rotation of Horizontal and Vertical
        //transform.rotation = Quaternion.Euler(new Vector3(tRotation.x, 
        //Mathf.Clamp(tRotation.y, _horizontalMinMax.x, _horizontalMinMax.y),
        //Mathf.Clamp(tRotation.z, _verticalMinMax.x, _verticalMinMax.y)));

        if (Input.GetKeyDown(KeyCode.Mouse0)) // Shoot bullet
        {
            GameObject CreatedCannonball = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
            CreatedCannonball.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * projectileForce;
        }
    }


}
