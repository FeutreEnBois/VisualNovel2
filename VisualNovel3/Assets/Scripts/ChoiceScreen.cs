using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// The choiceScreen script will Show, Hide, and Control what choices
/// are displayed on Screen.
/// </summary>
public class ChoiceScreen : MonoBehaviour
{
    public static ChoiceScreen instance;


    public ChoiceButton choicePrefab;
    public GameObject root;
    public List<ChoiceButton> choices = new List<ChoiceButton>();

    public VerticalLayoutGroup layoutGroup;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("!!! Trying to make multiple instance of ChoiceScreen !!!");
            return;
        }
        instance = this;
        Hide();
    }

    public void Show(string title = "",params string[] choices)
    {
        root.SetActive(true);
        if (isShowingChoices)
            StopCoroutine(showingChoices);
        ClearAllCurrentChoices();

        showingChoices = StartCoroutine(ShowingChoices(choices));

    }

    public void Hide()
    {
        if (isShowingChoices)
        {
            instance.StopCoroutine(showingChoices);
        }
        showingChoices = null;

        ClearAllCurrentChoices();
        root.SetActive(false);
    }

    private void ClearAllCurrentChoices()
    {
        foreach(ChoiceButton b in choices)
        {
            DestroyImmediate(b.gameObject);
        }
        choices.Clear();
    }

    public bool isWaitingForChoiceToBeMade { get { return isShowingChoices && !LastChoiceMade.hasBeenMade; } }
    public bool isShowingChoices { get { return showingChoices != null; } }
    public Coroutine showingChoices = null;
    public IEnumerator ShowingChoices(string[] choices)
    {
        yield return new WaitForEndOfFrame(); // allow the header to begin appearing if it will be present.
        LastChoiceMade.Reset();

        /*while (root.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame();
        }*/

        for(int i = 0; i < choices.Length; i++)
        {
            CreateChoice(choices[i]);
        }

        SetLayoutSpacing();
        while (isWaitingForChoiceToBeMade)
        {
            yield return new WaitForEndOfFrame();
        }
        Hide();
    }

    private void SetLayoutSpacing()
    {
        int i = choices.Count;
        if(i <= 3)
        {
            instance.layoutGroup.spacing = 20;
        }else if (i >= 7){
            instance.layoutGroup.spacing = 1;
        }
        else
        {
            switch (i)
            {
                case 4:
                    instance.layoutGroup.spacing = 15;
                    break;
                case 5:
                    instance.layoutGroup.spacing = 10;
                    break;
                case 6:
                    instance.layoutGroup.spacing = 5;
                    break;
            }
        }
    }
    private void CreateChoice(string choice)
    {
        GameObject ob = Instantiate(instance.choicePrefab.gameObject, instance.choicePrefab.transform.parent);
        ob.SetActive(true);
        ChoiceButton b = ob.GetComponent<ChoiceButton>();

        b.text = choice;
        b.choiceIndex = choices.Count;

        choices.Add(b);
    }

    [System.Serializable]
    public class CHOICE
    {
        public bool hasBeenMade { get { return title != "" && index != -1;} }
        public string title = "";
        public int index = -1;

        public void Reset()
        {
            title = "";
            index = -1;
        }
    }
    public CHOICE choice = new CHOICE();
    public static CHOICE LastChoiceMade { get { return instance.choice; } }

    public void MakeChoice(ChoiceButton button)
    {
        choice.index = button.choiceIndex;
        choice.title = button.text;
    }
}
