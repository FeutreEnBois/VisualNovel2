using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Layer : MonoBehaviour
{
    BCFC controller;
    public Texture texture;
    public BCFC.LAYER layer = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = BCFC.instance;
        layer = controller.background;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            layer.TransitionToTexture(texture);
        }
    }
}
