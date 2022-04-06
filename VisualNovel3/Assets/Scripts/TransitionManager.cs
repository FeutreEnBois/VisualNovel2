using System;
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
        Debug.Log("ta mamoune");
        if (layer.specialTransitionCoroutine != null)
        {
            instance.StopCoroutine(layer.specialTransitionCoroutine);
        }
        if(targetImage != null)
        {
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayer(layer, targetImage, transitionEffect, speed, smooth));
        }
        else
        {
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayerToNull(layer,transitionEffect, speed, smooth));
        }
    }


    private static IEnumerator TransitioningLayer(BCFC.LAYER layer, Texture2D targetTex, Texture2D transitionEffect, float speed, bool smooth)
    {
        GameObject ob = Instantiate(layer.newImageObjectReference, layer.newImageObjectReference.transform.parent);
        ob.SetActive(true);

        RawImage im = ob.GetComponent<RawImage>();
        im.texture = targetTex;

        layer.activeImage = im;
        layer.alImages.Add(im);

        im.material = new Material(instance.transitionMaterialPrefab);
        im.material.SetTexture("_AlphaTex", transitionEffect);
        im.material.SetFloat("_Cutoff", 1);
        float curVal = 1;

        while(curVal > 0)
        {
            Debug.Log("Crash ?");
            curVal = smooth ? Mathf.Lerp(curVal, 0, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 0, speed * Time.deltaTime);
            im.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }

        // remove the material so we can use regular alpha for transition.
        // check for null if we rapidly progress through fading and transition overlaps
        if(im != null)
        {
            im.material = null;
            //transition doaes use alpha so make sure alpha is up
            im.color = GlobalF.SetAlpha(im.color, 1);
        }

        // now remove all other images on layer
        for(int i = layer.alImages.Count -1; i >= 0; i--)
        {
            if(layer.alImages[i] == layer.activeImage && layer.activeImage != null)
            {
                continue;
            }
            if(layer.alImages[i] != null)
            {
                Destroy(layer.alImages[i].gameObject, 0.01f);
            }
            layer.alImages.RemoveAt(i);
        }

        //clear special transition field
        layer.specialTransitionCoroutine = null;
    }
    private static IEnumerator TransitioningLayerToNull(BCFC.LAYER layer, Texture2D transitionEffect, float speed, bool smooth)
    {

        List<RawImage> currentImagesOnLayer = new List<RawImage>();

        foreach(RawImage r in layer.alImages)
        {
            r.material = new Material(instance.transitionMaterialPrefab);
            r.material.SetTexture("_AlphaTex", transitionEffect);
            r.material.SetFloat("_Cutoff", 0);
            currentImagesOnLayer.Add(r);
        }

        float curVal = 0;
        while (curVal < 1)
        {
            curVal = smooth ? Mathf.Lerp(curVal, 1, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 1, speed * Time.deltaTime);
            for(int i = 0; i < layer.alImages.Count; i++)
            {
                layer.alImages[i].material.SetFloat("_Cutoff", curVal);
            }
            yield return new WaitForEndOfFrame();
        }

        foreach(RawImage r in currentImagesOnLayer)
        {
            layer.alImages.Remove(r);
            if(r.material != null)
            {
                Destroy(r.gameObject, 0.01f);
            }
        }

        layer.specialTransitionCoroutine = null;
    }
}
