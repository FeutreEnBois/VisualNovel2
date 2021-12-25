using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Testing_TextArchitect : MonoBehaviour
{
    public Text text;
    public Color color;
    TextArchitect architect;

    [TextArea(5, 10)] public string say;

    private void Start()
    {
        architect = new TextArchitect(say);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            architect = new TextArchitect(say);
        }

        text.text = architect.currentText;
    }
}
