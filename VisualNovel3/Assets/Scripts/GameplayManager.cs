using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public ClickableObject[] clickable;
    public ClickableObject[] clickableFind;
    // Start is called before the first frame update
    void Start()
    {
        clickableFind = new ClickableObject[clickable.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitPointAndClickScene()
    {
        //Change Background To point and click background

        // Clickable object setActive()

    }

    public void ClickOnClickableObject(ClickableObject preuve)
    {
        // Add something to say
        NovelController.instance.LoadChapterFile(preuve.dialogueText, preuve.progress);
        //Add preuve to preuve
        Debug.Log(clickableFind.Length);
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

            clickableFind[clickableFind.Length] = preuve;
        }
        // change color on hightlighted

    }

}
