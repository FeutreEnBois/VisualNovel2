using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    
    public int ID;
    public string Name;
    public string type;
    public string description;
    public bool empty = true;
    public Sprite icon;

    public void UpdateSlot()
    {
        Image image = this.GetComponent<Image>();
        image.sprite = icon;
        image.color = new Color(255, 255, 255, 255);
        //this.GetComponent<Button>().onClick.
        
    }
}
