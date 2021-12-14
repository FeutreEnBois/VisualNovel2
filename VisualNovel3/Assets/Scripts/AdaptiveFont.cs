using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptiveFont : MonoBehaviour
{
    Text text;
    public bool continualUpdate = true;
    public int fontSizeAtDefaultResolution = 24;
    public static float defaultResolution = 1589f; 

    private void Start()
    {
        text = GetComponent<Text>();
        if (continualUpdate)
        {
            InvokeRepeating("Adjust", 0f, UnityEngine.Random.Range(0.5f, 2f));
        }
        else
        {
            Adjust();
            enabled = false;
        }
    }

    private void Adjust()
    {
        if(!enabled || !gameObject.activeInHierarchy)
        {
            return;
        }
        float totalCurrentRes = Screen.height + Screen.width;
        float perc = totalCurrentRes / defaultResolution;
        int fontSize = Mathf.RoundToInt((float)fontSizeAtDefaultResolution * perc);

        text.fontSize = fontSize;
    }
}
