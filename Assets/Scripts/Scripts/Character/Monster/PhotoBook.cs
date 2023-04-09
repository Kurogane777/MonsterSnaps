using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoBook : MonoBehaviour
{
    public List<Picture> allPictures = new List<Picture>();//contains all your pictures, should be stored on the gamecontroller and not here

    public GameObject displayPrefab;//the UI prefab for displaying an image
    public Transform page1, page2;
    public Transform pagePlace;//where to create the prefabs
    public Transform pagePlace2;
    public int page = 0;//the page we are on
    public int pageSize;//how many items fit on a single page
    bool open;
    public static PhotoBook main;
    void Start()
    {
        main = this;
        page1.gameObject.SetActive(open);
        page2.gameObject.SetActive(open);
        //Populate();//call this every time you change page
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            open = !open;
            page1.gameObject.SetActive(open);
            page2.gameObject.SetActive(open);
            Populate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            page--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            page++;
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
        foreach (var item in GetPage(page))//creates all the images you need
        {
            var obj = Instantiate(displayPrefab, pagePlace);//create the prefab
            //set the image and whatever you want here (im using text for an example)
            obj.transform.GetChild(1).GetComponent<Text>().text = item.name;
            obj.transform.GetChild(0).GetComponent<RawImage>().texture = item.texture;
        }
        //incase you wanted 2 pages
        foreach (Transform child in pagePlace2)//clears any previously existing
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GetPage(page+1))//creates all the images you need
        {
            var obj = Instantiate(displayPrefab, pagePlace2);//create the prefab
            //set the image and whatever you want here (im using text for an example)
            obj.transform.GetChild(1).GetComponent<Text>().text = item.name;
            obj.transform.GetChild(0).GetComponent<RawImage>().texture = item.texture;
            
        }
    }
    List<Picture> GetPage(int pageIndex, List<Picture> pool = allPictures)//returns a list of items for that page index
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
    public Texture2D texture;
    //any other data you want in here
    public Picture(string name,Texture2D texture, List<CaptureTarget> targets)
    {
        this.texture = texture;
        this.name = name;
        this.targets = targets;
    }
}
public class CaptureTarget
{
    string name;
    float visibiity;
    public CaptureTarget(string name, float visibility)
    {
        this.visibiity = visibiity;
        this.name = name;
    }
}
