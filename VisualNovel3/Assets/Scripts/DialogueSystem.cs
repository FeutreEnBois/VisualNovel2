using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public ELEMENTS elements;

    void Awake()
    {

        if (instance != null)
        {
            Debug.LogError("!!! trying to create multiple instance of DialogueSystem !!!");
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Say something to be added to what is already on the speech box.
    /// </summary>
    /// <param name="speech"></param>
    /// <param name="additive"></param>
    /// <param name="speaker"></param>
    public void Say(string speech, bool additive = false, string speaker = "") // 
    {
        StopSpeaking();
        speaking = StartCoroutine(Speaking(speech, additive, speaker)); // 
    }

    public void SayAdd(string speech, string speaker = "")
    {
        StopSpeaking();
        speechText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(speech, true, speaker));
    }

    public void Open(string speakerName = "", string speech = "")
    {
        if (speakerName == "" && speech == "" || speakerName == null && speech == null)
        {
            Close();
            return;
        }

        speechPanel.SetActive(true);

        speakerNameText.text = speakerName;

        speakerNamePanel.SetActive(speakerName != "");

        speechText.text = speech;
    }


    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        if (textArchitect != null && textArchitect.isConstructing)
        {
            textArchitect.Stop();
        }
        speaking = null;
    }

    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput = false;

    public string targetSpeech = "";
    Coroutine speaking = null;
    TextArchitect textArchitect = null;
    IEnumerator Speaking(string speech, bool additive, string speaker = "")
    {
        speechPanel.SetActive(true);

       // Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
//        AudioManager.instance.playText();

        string additiveSpeech = additive ? speechText.text : "";
        targetSpeech = additiveSpeech + speech;

        textArchitect = new TextArchitect(speech, additiveSpeech);

        speakerNameText.text = DetermineSpeaker(speaker);//temporary

        speakerNamePanel.SetActive(speakerNameText.text != "");

        isWaitingForUserInput = false;

        while (textArchitect.isConstructing)
        {
            if (Input.GetKey(KeyCode.Space))
                textArchitect.skip = true;
            speechText.text = textArchitect.currentText;
            yield return new WaitForEndOfFrame();

        }

        speechText.text = textArchitect.currentText;
        //text finished
        isWaitingForUserInput = true;
        //if(autoPlay == true) {
        //	yield return new WaitForSeconds(2);
        //	Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        //	StopSpeaking();

        //} else {
        while (isWaitingForUserInput)
        {
            //if (autoPlay == true)

            //{
            //     yield return new WaitForSecondsRealtime(2);
            //    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //     StopSpeaking();
            // }
            // else
            // {
            //AudioManager.instance.playText();
            yield return new WaitForEndOfFrame();
                StopSpeaking();
               AudioManager.instance.stopText();
            //}
        }
        //}

    }

    string DetermineSpeaker(string s)
    {
        string retVal = speakerNameText.text;//default return is the current name
        if (s != speakerNameText.text && s != "")
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;

        return retVal;
    }

    /// <summary>
    /// Close the entire speech panel. stop all dialogue.
    /// </summary>
    public void Close()
    {
        StopSpeaking();
        speechPanel.SetActive(false);
        Debug.Log("set active = false");
        AudioManager.instance.stopText();

    }



    [System.Serializable]
    public class ELEMENTS
    {
        /// <summary>
        /// The main panel containing all dialogue related elements on the UI
        /// </summary>
        public GameObject speechPanel;
        public GameObject speakerNamePanel;
        public Text speakerNameText;
        public Text speechText;
    }
    public GameObject speechPanel { get { return elements.speechPanel; } }
    public Text speakerNameText { get { return elements.speakerNameText; } }
    public Text speechText { get { return elements.speechText; } }
    public GameObject speakerNamePanel { get { return elements.speakerNamePanel; } }
}
