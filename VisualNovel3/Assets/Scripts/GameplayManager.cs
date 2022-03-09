using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public ClickableObject[] clickable;
    public ClickableObject[] clickableFind;


    public static GameplayManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("!!! trying to create multiple instance of NovelController !!!");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        clickableFind = new ClickableObject[clickable.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePointAndClickScene()
    {
        //Change Background To point and click background

        // Clickable object setActive()
        foreach(ClickableObject obj in clickable)
        {
            obj.gameObject.SetActive(!obj.gameObject.activeInHierarchy);
        }
    }

    public void ClickOnClickableObject(ClickableObject preuve)
    {
        // Add something to say
        NovelController.instance.LoadChapterFile(preuve.dialogueText, preuve.progress);
        //Add preuve to preuve
        bool tempAdd = true;
        foreach(ClickableObject obj in clickableFind)
        {
            if (obj == preuve)
            {
                tempAdd = false;
            }
        }
        if (tempAdd)
        {

            clickableFind[clickableFind.Length-1] = preuve;
        }
        // change color on hightlighted

    }

}
