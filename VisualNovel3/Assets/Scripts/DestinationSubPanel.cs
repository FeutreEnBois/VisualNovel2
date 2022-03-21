using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationSubPanel : MonoBehaviour
{
    public string Name;
    public string Destination;
    public Texture2D DestinationTexture;
    public string Description;

    public Image destinationImage;
    private void Start()
    {
        destinationImage.GetComponent<Image>().sprite = Sprite.Create(DestinationTexture, new Rect(0,0,600, 400), new Vector2(0.5f,0.5f));
    }
}
