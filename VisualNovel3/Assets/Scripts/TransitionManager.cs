using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransitionManager : MonoBehaviour
{

    public static TransitionManager instance;
    public RawImage overlayImage;
    public Material transitionMaterialPrefab;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("!!! Trying to create multiple instance of Transition Manager !!!");
            return;
        }
        instance = this;
        overlayImage.material = new Material(transitionMaterialPrefab);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowScene(!sceneVisible);
        }
        
    }

    static bool sceneVisible = true;
    public static void ShowScene(bool show, float speed = 1, bool smooth = false, Texture2D transitionEffect = null)
    {
        if (transitioningOverlay != null)
        {
            instance.StopCoroutine(transitioningOverlay);
        }
        sceneVisible = show;

        if(transitionEffect != null)
        {
            instance.overlayImage.material.SetTexture("_Alphatex", transitionEffect);
        }

        transitioningOverlay = instance.StartCoroutine(TransitioningOverlay(show, speed, smooth));
    }

    static Coroutine transitioningOverlay = null;
    static IEnumerator TransitioningOverlay(bool show, float speed, bool smooth)
    {
        float targVal = show ? 1 : 0;
        float curVal = instance.overlayImage.material.GetFloat("_Cutoff");

        while(curVal != targVal)
        {
            curVal = smooth ? Mathf.Lerp(curVal, targVal, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, targVal, speed * Time.deltaTime);
            instance.overlayImage.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }

        transitioningOverlay = null;
    }

    //handle transitioning directly yo images for layers
    public static void TransitionLayer(BCFC.LAYER layer, Texture2D targetImage, Texture2D transitionEffect, float speed = 1, bool smooth = false)
    {

    }
}
