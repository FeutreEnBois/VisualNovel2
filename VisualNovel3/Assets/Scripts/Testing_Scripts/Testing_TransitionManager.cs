using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_TransitionManager : MonoBehaviour
{
    public Texture2D tex1;
    public Texture2D trans1;
    public Texture2D tex2;
    public Texture2D trans2;
    public int progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            progress = Mathf.Clamp(progress + 1, 0, 10);

            switch (progress)
            {
                case 0:
                    TransitionManager.ShowScene(false);
                    break;
                case 1:
                    TransitionManager.ShowScene(true);
                    break;
                case 2:
                    TransitionManager.TransitionLayer(BCFC.instance.background, tex1, trans1);
                    break;
                case 3:
                    TransitionManager.TransitionLayer(BCFC.instance.background, tex2, trans2);
                    break;
                case 4:
                    BCFC.instance.background.TransitionToTexture(tex1);
                    break;
                case 5:
                    TransitionManager.TransitionLayer(BCFC.instance.background, tex2, trans2);
                    break;
                case 6:
                    BCFC.instance.background.TransitionToTexture(tex1);
                    break;
                case 7:
                    TransitionManager.TransitionLayer(BCFC.instance.background, null, trans1);
                    break;
                case 8:
                    BCFC.instance.background.TransitionToTexture(tex1);
                    TransitionManager.ShowScene(true);
                    break;
                case 9:
                    TransitionManager.ShowScene(false);
                    break;
            }
        }
    }
}
