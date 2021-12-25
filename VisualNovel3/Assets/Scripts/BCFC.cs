using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCFC : MonoBehaviour
{
    public static BCFC instance;

    public LAYER background = new LAYER();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("!!! trying to create multiple instance of BCFC !!!");
            return;
        }
        instance = this;
    }

    [System.Serializable]
    public class LAYER
    {
        public GameObject root;
        public GameObject newImageObjectReference;
        public RawImage activeImage;
        public List<RawImage> alImages = new List<RawImage>();

        public Coroutine specialTransitionCoroutine = null;

		public void SetTexture(Texture texture)
		{
			if (texture != null)
			{
				if (activeImage == null)
                {
					CreateNewActiveImage();
                }
				activeImage.texture = texture;
				activeImage.color = GlobalF.SetAlpha(activeImage.color, 1f);
			}
			else
			{
				if (activeImage != null)
				{
					alImages.Remove(activeImage);
					GameObject.DestroyImmediate(activeImage.gameObject);
					activeImage = null;
				}
			}
		}

		public void TransitionToTexture(Texture texture, float speed = 2f, bool smooth = false)
        {
			if(activeImage != null && activeImage.texture == texture)
            {
				return;
            }
			StopTransitioning();
			transitioning = BCFC.instance.StartCoroutine(Transitioning(texture, speed, smooth));
        }

        private void StopTransitioning()
        {
            if (isTransitioning)
            {
                BCFC.instance.StopCoroutine(transitioning);
            }
            transitioning = null;
        }

        public bool isTransitioning { get { return transitioning != null; } }
        Coroutine transitioning = null;
        IEnumerator Transitioning(Texture texture, float speed, bool smooth)
        {
            if(texture != null)
            {
                for(int i = 0; i < alImages.Count; i++)
                {
                    RawImage image = alImages[i];
                    if(image.texture == texture)
                    {
                        activeImage = image;
                        break;
                    }
                }

                if(activeImage == null || activeImage.texture != texture)
                {
                    CreateNewActiveImage();
                    activeImage.texture = texture;
                    activeImage.color = GlobalF.SetAlpha(activeImage.color, 0f);

                }
            }
            else
            {
                activeImage = null;
            }

            while(GlobalF.TransitionRawImages(ref activeImage, ref alImages, speed, smooth))
            {
                yield return new WaitForEndOfFrame();
            }

            StopTransitioning();

        }


        void CreateNewActiveImage()
		{
			GameObject ob = Instantiate(newImageObjectReference, root.transform);
			ob.SetActive(true);
			RawImage raw = ob.GetComponent<RawImage>();
			activeImage = raw;
			alImages.Add(raw);
		}
	}
}
