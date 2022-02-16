using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GAMEFILE
{
    public string chapterName;
    public int chapterProgress = 0;
    public string cachedLastSpeaker = "";
    public string currentTextSystemSpeakerDisplayText = "";
    public string currentTextSystemDisplayText = "";

    public List<CHARACTERDATA> characterInScene = new List<CHARACTERDATA>();

    public Texture background = null;
    //public Texture foreground = null;
    //public Texture cinematic = null;

    public AudioClip music = null;


    public GAMEFILE() 
    {
        this.chapterName = "chapter0_01";
        this.chapterProgress = 0;
        this.cachedLastSpeaker = "";

        characterInScene = new List<CHARACTERDATA>();
    }

    [System.Serializable]
    public class CHARACTERDATA
    {
        public string characterName = "";
        public bool enabled = true;
        //public string facialExpression = "";
        public string bodyExpression = "";
        //public bool facingLeft = true;
        public Vector2 position = Vector2.zero;

        public CHARACTERDATA(Character character)
        {
            this.characterName = character.Charactername;
            this.enabled = character.isVisibleInScene;
            //this.facialExpression = character.renderers.expressionRenderer.sprite.name;
            this.bodyExpression = character.renderers.bodyRenderer.sprite.name;
            this.position = character._targetPosition;
        }
    }
}
