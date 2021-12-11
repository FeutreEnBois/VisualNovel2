using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Choices : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(DynamicStoryExample());
    }

    private IEnumerator DynamicStoryExample()
    {
        NovelController.instance.LoadChapterFile("Chapter_0"); yield return new WaitForEndOfFrame();

        while (NovelController.instance.isHandlingChapterFile)
        {
            yield return new WaitForEndOfFrame();
        }
        ChoiceScreen.instance.Show("I like ...", "Jean", "Marcel", "Erik");
        while (ChoiceScreen.instance.isWaitingForChoiceToBeMade)
        {
            yield return new WaitForEndOfFrame();
        }
        if(ChoiceScreen.LastChoiceMade.index == 0)
        {
            NovelController.instance.LoadChapterFile("Interrogatoire_Jean");
        }
        else
        {
            NovelController.instance.LoadChapterFile("Interrogatoire_Marcel");
        }
        yield return new WaitForEndOfFrame();

        while(NovelController.instance.isHandlingChapterFile)
            yield return new WaitForEndOfFrame();
    }
}
