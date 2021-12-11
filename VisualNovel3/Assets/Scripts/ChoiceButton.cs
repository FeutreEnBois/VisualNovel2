using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The ChoiceButton script is assigned to the buttons on the screen
/// and hold valuable data about each choice.
/// </summary>
public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI tmpro;
    public string text { get { return tmpro.text; } set { tmpro.text = value; } }

    [HideInInspector]
    public int choiceIndex = -1;
}
