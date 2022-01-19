using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    
    public int ID;
    public string type;
    public string description;
    public bool empty = true;
    public Sprite icon;

    public void UpdateSlot()
    {
        this.GetComponent<Image>().sprite = icon;
        
    }
}
