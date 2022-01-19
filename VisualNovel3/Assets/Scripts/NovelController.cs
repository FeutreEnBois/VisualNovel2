using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

public class NovelController : MonoBehaviour
{

    //The lines of data loaded directly from a chapter file.
    List<string> data = new List<string>();
    // The progress in the current data list.
    int progress = 0;
    public bool canProgress = true;
    public static NovelController instance;
    public bool isHandlingChapterFile { get { return canProgress && progress < data.Count; } }
    //public GameObject autoPlay;


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
        LoadChapterFile("chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {
        //if(autoPlay.activeSelf)
        //testing
        /*if (canProgress &&  progress < data.Count && autoPlay.activeSelf == false)
        {
            string line = data[progress];
            if (line.StartsWith("choice"))
            {

                autoPlay.SetActive(true);
                //StartCoroutine(HandlingChoiceLine(line));
                StartCoroutine(HandlingChoiceLine(line));

            }
            else
            {
                HandleLine(line);
                progress++;
            }
                //System.Threading.Thread.Sleep(1000);
        }*/

        if (canProgress && Input.GetKeyDown(KeyCode.RightArrow) && progress < data.Count /*&& autoPlay.activeSelf == true*/)
        {
            string line = data[progress];
            if (line.StartsWith("choice"))
            {
                StartCoroutine(HandlingChoiceLine(line));
            }
            else
            {
                HandleLine(line);
                progress++;
            }
        }
    }

    IEnumerator HandlingChoiceLine(string line)
    {
        canProgress = false;
        Debug.Log("progress false");
        string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();

        while (true)
        {
            progress++;
            line = data[progress];
            if (line == "{")
                continue;
            line = line.Replace("    ", ""); // remove the tabs that have becomes quad spaces

            if (line != "}")
            {
                choices.Add(line.Split('"')[1]);
                actions.Add(data[progress + 1].Replace("    ", ""));
                progress++;
            }
            else
            {
                break;
            }
        }

        //displaying choices
        if (choices.Count > 0)
        {
            ChoiceScreen.instance.Show(title, choices.ToArray()); yield return new WaitForEndOfFrame();
            while (ChoiceScreen.instance.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();
            //choice is made. execute the paired action.
            string action = actions[ChoiceScreen.LastChoiceMade.index];
            HandleLine(action); // need to be an IEnumerator

            while (isHandlingChapterFile)
                yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("!!! Invalid choice operation. No choices were found. !!!");
        }
        canProgress = true;
        progress++;
    }
    public void LoadChapterFile(string filename, int progress = 0)
    {
        if (!canProgress)
        {
            canProgress = true;
        }
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + filename);
        this.progress = progress;
        if (this.progress < data.Count)
        {
            HandleLine(data[this.progress]);
            this.progress++;
        }
    }

    void HandleLine(string line)
    {
        string[] dialogueAndActions = line.Split('"');

        if(dialogueAndActions.Length == 3)
        {
            HandleDialogue(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);
        }
        else
        {
            HandleEventsFromLine(dialogueAndActions[0]);
        }
    }

    // used as a fallback when no speaker is given
    string cachedLastSpeaker = "";
    void HandleDialogue(string dialogueDetails, string dialogue)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetails.Contains("+");

        // remove the additive sign from the speaker name area
        if (additive)
        {
            dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);
        }
        if (dialogueDetails.Length > 0)
        {
            // remove the space after the speaker's name if present.
            if(dialogueDetails[dialogueDetails.Length-1] == ' ')
            {
                dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);
            }

            speaker = dialogueDetails;
            cachedLastSpeaker = speaker;
        }

        //now speak
        // a narrator should be retrived as a character.
        if (speaker != "narrator" && speaker != "[. . .]" && CharacterManager.instance.GetCharacter(speaker, false) != null)
        {
            Character character = CharacterManager.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
        }
        else
        {
            DialogueSystem.instance.Say(dialogue, additive, speaker);
        }
    }

    void HandleEventsFromLine(string events)
    {
        string[] actions = events.Split(' ');

        foreach(string action in actions)
        {
            HandleAction(action);
        }
    }
    void HandleAction(string action)
    {
        string[] data = action.Split('(', ')');

        switch (data[0])
        {
            case "setBackground":
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;
            case "transBackground":
                Command_TransitionLayerImage(data[1], BCFC.instance.background);
                break;
            case "playSound":
                Command_PlaySound(data[1]);
                break;
            case "moveTo":
                Command_MoveCharacter(data[1]);
                break;
            case "setExpression":
                Command_ChangeExpression(data[1]);
                break;
            case "flip":
                Command_Flip(data[1]);
                break;
            case "flipLeft":
                Command_FlipLeft(data[1]);
                break;
            case "flipRight":
                Command_FlipRight(data[1]);
                break;
            case "exit":
                Command_Exit(data[1]);
                break;
            case "enter":
                Command_Enter(data[1]);
                break;
            case "playMusic":
                Command_PlayMusic(data[1]);
                break;
            case "goToPreuve":
                Command_GoToPreuveScene(data[1]);
                break;
            case "stop":
                Command_Stop(data[1]);
                break;
            case "load":
                Command_Load(data[1]);
                break;
            case "continue":
                Command_Continue();
                break;
        }
        if(data[0] == "setBackground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if (data[0] == "transBackground")
        {
            Command_TransitionLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if (data[0] == "playSound")
        {
            Command_PlaySound(data[1]);
        }

    }

    private void Command_Load(string data)
    {
        if (data.Contains(","))
        {
            int chapProgress = 0;
            string[] parameters = data.Split(',');
            foreach (string p in parameters)
            {
                int ival = 0;
                if (int.TryParse(p, out ival))
                {
                    chapProgress = ival; continue;
                }
            }
            LoadChapterFile(parameters[0], chapProgress);
        }
        else
        {
            LoadChapterFile(data);

        }
    }
    private void Command_Continue()
    {

        HandleLine(data[this.progress+1]);
        this.progress++;
    }
    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        /*
         Definition of backdrop
            : a painted cloth that is hung across the back of a stage
            : the scene or scenery that is in the background
        */
        string textName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D text = textName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + textName) as Texture2D;
        float spd = 2f;
        bool smooth = false;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            foreach(string p in parameters)
            {
                float fval = 0;
                bool bval = false;
                if(float.TryParse(p, out fval)){
                    spd = fval; continue;
                }
                if(bool.TryParse(p, out bval))
                {
                    smooth = bval; continue;
                }
            }
        }
        layer.TransitionToTexture(text, spd, smooth);
    }

    void Command_TransitionLayerImage(string data, BCFC.LAYER layer)
    {
        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            Texture2D texture = parameters[0] == "null" ? null : Resources.Load("Images/UI/Backdrops/" + parameters[0]) as Texture2D;
            Texture2D transition = parameters[1] == "null" ? null : Resources.Load("Images/TransitionEffects/" + parameters[1]) as Texture2D;
            TransitionManager.TransitionLayer(BCFC.instance.background, texture, transition);
        }
        else
        {
            Texture2D text = data == "null" ? null : Resources.Load("Images/UI/Backdrops/" + data) as Texture2D;
            TransitionManager.TransitionLayer(BCFC.instance.background, null, text);
        }
    }

    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if (clip != null)
            AudioManager.instance.PlaySFX(clip);
        else
            Debug.LogError("Clip does not exist : " + data);
    }

    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        if (clip != null)
            AudioManager.instance.PlaySong(clip);
        else
            Debug.LogError("Clip does not exist : " + data);
    }

    /*void Command_PlayMusic()
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        if (clip != null)
            AudioManager.instance.PlaySong(clip);
        else
            Debug.LogError("Clip does not exist : " + data);
    }
    */
    void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1], CultureInfo.InvariantCulture);
        float locationY = float.Parse(parameters[2], CultureInfo.InvariantCulture);
        float speed = parameters.Length <= 4 ? float.Parse(parameters[3]) : 1f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : false;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);

    }

    void Command_ChangeExpression(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string region = parameters[1];
        string expression = parameters[2];
        float speed = parameters.Length <= 4 ? float.Parse(parameters[3]) : 1f;
        bool smooth = parameters.Length == 4 ? bool.Parse(parameters[4]) : false;
        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);
        if(region.ToLower() == "body")
        {
            c.TransitionBody(sprite, speed, smooth);
        }
        if (region.ToLower() == "face")
        {
            c.TransitionExpression(sprite, speed, smooth);
        }
    }

    void Command_Flip(string data)
    {
        Character c = CharacterManager.instance.GetCharacter(data);
        c.Flip();
    }
    void Command_FlipLeft(string data)
    {
        Character c = CharacterManager.instance.GetCharacter(data);
        c.FlipLeft();
    }
    void Command_FlipRight(string data)
    {
        Character c = CharacterManager.instance.GetCharacter(data);
        c.FlipRight();
    }
    void Command_GoToPreuveScene(string data)
    {
        GameplayManager.instance.InitPointAndClickScene();
    }

    void Command_Stop(string data)
    {
        DialogueSystem.instance.Close();
        canProgress = false;
    }

    void Command_Exit(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for(int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if(float.TryParse(parameters[i], out fVal))
            {
                speed = fVal; continue;
            }
            if(bool.TryParse(parameters[i], out bVal))
            {
                smooth = bVal;continue;
            }
        }
        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }

    void Command_Enter(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            {
                speed = fVal; continue;
            }
            if (bool.TryParse(parameters[i], out bVal))
            {
                smooth = bVal; continue;
            }
        }
        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s,true,false);
            if (!c.enabled)
            {
                c.renderers.bodyRenderer.color = new Color(1, 1, 1, 0);
                c.renderers.expressionRenderer.color = new Color(1, 1, 1, 0);
                c.enabled = true;

                c.TransitionBody(c.renderers.bodyRenderer.sprite, speed, smooth);
                c.TransitionExpression(c.renderers.expressionRenderer.sprite, speed, smooth);
            }
            else
            {
                c.FadeIn(speed, smooth);
            }
        }
    }
}
