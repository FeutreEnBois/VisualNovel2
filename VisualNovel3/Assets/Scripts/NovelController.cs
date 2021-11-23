using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class NovelController : MonoBehaviour
{

    //The lines of data loaded directly from a chapter file.
    List<string> data = new List<string>();
    // The progress in the current data list.
    int progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadChapterFile("chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {
        //testing
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleLine(data[progress]);
            progress++;
        }
    }

    void LoadChapterFile(string filename)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + filename);
        Debug.Log(data[0]);
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
        if (speaker != "narrator")
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
        print("Handle action [" + action + "]");
        string[] data = action.Split('(', ')');

        /*if(data[0] == "setBackground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if (data[0] == "setCinematic")
        {
            Command_SetLayerImage(data[1], BCFC.instance.cinematic);
            return;
        }
        if (data[0] == "setForeground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.foreground);
            return;
        }*/
        if(data[0] == "playSound")
        {
            Command_PlaySound(data[1]);
        }

        if (data[0] == "moveTo")
        {
            Command_MoveCharacter(data[1]);
        }
        if (data[0] == "setExpression")
        {
            Command_ChangeExpression(data[1]);
        }
        if (data[0] == "flip")
        {
            Command_ChangeExpression(data[1]);
        }
        if (data[0] == "flipLeft")
        {
            Command_ChangeExpression(data[1]);
        }
        if (data[0] == "flipRight")
        {
            Command_ChangeExpression(data[1]);
        }
        if (data[0] == "exit")
        {
            Command_Exit(data[1]);
        }
        if (data[0] == "enter")
        {
            Command_Enter(data[1]);
        }
        /*if (data[0] == "playMusic")
        {
            Command_PlayMusic(data[1]);
        }
        */
    }

    /*void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
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
    }*/

    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if (clip != null)
            AudioManager.instance.PlaySFX(clip);
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
