using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextArchitect : MonoBehaviour
{
    /// <summary>
    /// The current text build by the text architect up to this point in time
    /// </summary>
    public string currentText { get { return _currentText; } }
    private string _currentText = "";

    private string preText;
    private string targetText;

    private int characterPerFrame = 1;
    [Range(1, 60f)] private float speed = 1f; // This variable will be shown as a slider btw 1 && 60f in the inspector
    private bool useEncapsulation = true;

    public bool skip = false;

    public bool isConstructing { get { return buildProcess != null; } }
    Coroutine buildProcess = null;

    public TextArchitect(string targetText, string preText = "", int characterPerFrame = 1, float speed = 1f, bool useEncapsulation = true)
    {
        this.targetText = targetText;
        this.preText = preText;
        this.characterPerFrame = characterPerFrame;
        this.speed = speed;
        this.useEncapsulation = useEncapsulation;

        buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
    }

    IEnumerator Construction()
    {
        int runsThisFrame = 0;
        string[] speechAndTags = useEncapsulation ? TagManager.SplitByTag(targetText) : new string[1] { targetText };

        // if this is additive, make sure we include the additive text.
        _currentText = preText;

        //make a storage for the building text so we don't change what is already made. so we can sandwitch it between tags.
        string curText = "";

        // Build the text by moving through each part
        for (int a = 0; a < speechAndTags.Length; a++)
        {
            string section = speechAndTags[a];
            // tags will always be odd indexed
            bool isATag = (a & 1) != 0;

            if (isATag && useEncapsulation)
            {
                // store the current text into something that can be referenced as a restart point as tagged sections of text are added and removed.
                curText = _currentText;
                ENCAPSULATED_TEXT encapsulation = new ENCAPSULATED_TEXT(string.Format("<{0}>", section), speechAndTags, a);
                while (!encapsulation.isDone)
                {
                    bool stepped = encapsulation.Step();

                    _currentText = curText + encapsulation.displayText;

                    // only yield if a step was taken in building the string

                    if (stepped)
                    {
                        runsThisFrame++;
                        int maxRunsPerFrame = skip ? 5 : characterPerFrame;
                        if (runsThisFrame == maxRunsPerFrame)
                        {
                            runsThisFrame = 0;
                            yield return new WaitForSeconds(skip ? 0.01f : 0.01f * speed);
                        }
                    }
                }
                a = encapsulation.speechAndTagsArrayProgress + 1;
            }
            //not a tag or not using encap. build like regular text.
            else
            {
                for (int i = 0; i < section.Length; i++)
                {
                    _currentText += section[i];

                    runsThisFrame++;
                    int maxRunsPerFrame = skip ? 5 : characterPerFrame;
                    if (runsThisFrame == maxRunsPerFrame)
                    {
                        runsThisFrame = 0;
                        yield return new WaitForSeconds(skip ? 0.01f : 0.01f * speed);
                    }
                }
            }
        }
        // end the build process, construction is done.
        buildProcess = null;

    }

    private class ENCAPSULATED_TEXT
    {
        // tag precedes text. ending tag trails it.
        private string tag = "";
        private string endingTag = "";
        // current text is the currently built target text without tags. target text is the build target.
        private string currentText = "";
        private string targetText = "";

        public string displayText { get { return _displayText; } }
        private string _displayText = "";

        // contains elements that the encapsulator will attempt to advance to when searching for sub encapsulator.
        private string[] allSpeechAndTagsArray;
        public int speechAndTagsArrayProgress { get { return arrayProgress; } }
        private int arrayProgress = 0;

        public bool isDone { get { return _isDone; } }
        private bool _isDone = false;

        public ENCAPSULATED_TEXT encapsulator = null;
        public ENCAPSULATED_TEXT subEncapsulator = null;
        public ENCAPSULATED_TEXT(string tag, string[] allSpeechAndTagsArray, int arrayProgress)
        {
            this.tag = tag;
            GenerateEndingTag();

            this.allSpeechAndTagsArray = allSpeechAndTagsArray;
            this.arrayProgress = arrayProgress;

            if (allSpeechAndTagsArray.Length - 1 > arrayProgress)
            {
                string nextPart = allSpeechAndTagsArray[arrayProgress + 1];
                bool isATag = ((arrayProgress + 1) & 1) != 0;

                // if the next writable part is not a tag, make it the target text of this encapsulator.
                if (!isATag)
                {
                    targetText = nextPart;
                    // if the next writable part is a tag, add a sub encapsulator to take care of it.
                }

                // increment progress so the next attempted part is updated.
                this.arrayProgress++;
            }
        }

        void GenerateEndingTag()
        {
            endingTag = tag.Replace("<", "").Replace(">", "");
            if (endingTag.Contains("="))
            {
                endingTag = string.Format("</{0}>", endingTag.Split('=')[0]);
            }
            else
            {
                endingTag = string.Format("</{0}>", endingTag);
            }
        }


        public bool Step()
        {
            // a completed encapsulation should not step any further. return true so if there is an error, yielding may occur.
            if (isDone)
                return true;
            // if there is a sub encapsulator, then it must finish before this encapsulator can procede.
            if (subEncapsulator != null && (!subEncapsulator.isDone))
            {
                return subEncapsulator.Step();
            }

            // this encapsulator needs to finish its text.
            else
            {
                // this encapsulator has reached the end of it's text
                if (currentText == targetText)
                {
                    // if there is still more dialogue to build .
                    if(allSpeechAndTagsArray.Length > arrayProgress + 1)
                    {
                        string nextPart = allSpeechAndTagsArray[arrayProgress + 1];
                        bool isATag = ((arrayProgress + 1) & 1) != 0;

                        if (isATag)
                        {
                            if(string.Format("<{0]>", nextPart) == endingTag)
                            {
                                _isDone = true;

                                // Update this encapsulator's encapsulator is any.
                                if(encapsulator != null)
                                {
                                    string taggedText = tag + currentText + endingTag;
                                    encapsulator.currentText += taggedText;
                                    encapsulator.targetText += taggedText;

                                    // Update array progress to get past the current text AND the ending tag. +2
                                    UpdateArrayProgress(2);
                                }
                            }
                            // if the tag we reached os not the terminator for this encapsulator, then a sub encapsulator must be created.
                            else
                            {
                                subEncapsulator = new ENCAPSULATED_TEXT(string.Format("<{0}>", nextPart), allSpeechAndTagsArray, arrayProgress + 1);
                                subEncapsulator.encapsulator = this;

                                //have the encapsulators keep up with the current progress.
                                UpdateArrayProgress();
                            }
                        }
                        // if the next part is not a tag, then this is an extention to be added to the encapsulator's target.
                        else
                        {
                            targetText += nextPart;
                            UpdateArrayProgress();
                        }
                    }
                    // finished dialogue. Close.
                    else
                    {
                        _isDone = true;
                    }
                }

                // if there is still more text to build
                else
                {
                    currentText += targetText[currentText.Length];
                    // Update the display text, which means we have to update any encapsulators if this is a sub encapsulator
                    UpdateDisplay("");

                    return true; // a step was taken.
                }
            }
            return false;
        }


        void UpdateArrayProgress(int val = 2)
        {
            arrayProgress += val;

            if( encapsulator != null)
            {
                encapsulator.UpdateArrayProgress(val);
            }
        }
        void UpdateDisplay(string subValue)
        {
            _displayText = string.Format("{0}{1}{2}{3}", tag, currentText, subValue, endingTag);

            // Update an encapsulators text to show its own text and its sub encapsulator's encapsulated within its tags.
            if(encapsulator != null)
            {
                encapsulator.UpdateDisplay(displayText);
            }
        }
    }
}
