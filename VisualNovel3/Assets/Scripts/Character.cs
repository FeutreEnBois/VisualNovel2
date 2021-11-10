using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Character
{
    public string Charactername;

    //The root is the container for all images related to the character in the scene. the root object.
    [HideInInspector] public RectTransform root;

    public bool enabled { get { return root.gameObject.activeInHierarchy; } set { root.gameObject.SetActive(value); } }

    DialogueSystem dialogue = DialogueSystem.instance;
    public bool isMultiLayerCharacter { get { return renderers.renderer == null; } }
    public Character (string _name)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject prefab = Resources.Load("Characters/Character["+_name+"]") as GameObject;
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        Charactername = _name;

        //get the renderer(s)
        renderers.renderer = ob.GetComponentInChildren<RawImage>();
        if (isMultiLayerCharacter)
        {
            renderers.bodyRenderer = ob.transform.Find("bodyLayer").GetComponent<Image>();
            renderers.expressionRenderer = ob.transform.Find("expressionLayer").GetComponent<Image>();
        }
    }

    public void Say(string speech, bool add = false)
    {
        if (!enabled)
        {
            enabled = true;
        }
        dialogue.Say(speech, add, Charactername);
    }
    [System.Serializable]
    public class Renderers
    {
        // used as the only image for a single layer Character
        public RawImage renderer;

        // used as the body and expression images in a multiple layer Character
        public Image bodyRenderer;
        public Image expressionRenderer;
    }
    public Renderers renderers = new Renderers();
}
