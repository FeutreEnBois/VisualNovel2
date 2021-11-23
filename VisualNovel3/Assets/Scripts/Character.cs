using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Character
{
    public string Charactername;

    //The root is the container for all images related to the character in the scene. the root object.
    [HideInInspector] public RectTransform root;

    // The space between the anchors of this character. Defines how much space a character takes up on the canvas.
    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }
    public bool isMultiLayerCharacter { get { return renderers.renderer == null; } }
    public bool enabled { get { return root.gameObject.activeInHierarchy; } set { root.gameObject.SetActive(value); } }

    DialogueSystem dialogue = DialogueSystem.instance;

    /// <summary>
    /// Create a new character
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="enableOnStart"></param>
    public Character (string _name, bool enableOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        //locate the character prefab.
        GameObject prefab = Resources.Load("Characters/Character["+_name+"]") as GameObject;
        //spawn an instance of the prefab directly on the character panel.
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        Charactername = _name;

        //get the renderer(s)
        renderers.renderer = ob.GetComponentInChildren<RawImage>();
        if (isMultiLayerCharacter)
        {
            renderers.bodyRenderer = ob.transform.Find("BodyLayer").GetComponentInChildren<Image>();
            renderers.expressionRenderer = ob.transform.Find("ExpressionLayer").GetComponentInChildren<Image>();
            renderers.allBodyRenderers.Add(renderers.bodyRenderer);
            renderers.allExpressionRenderers.Add(renderers.expressionRenderer);
        }

        dialogue = DialogueSystem.instance;
        enabled = enableOnStart;
    }

    /// <summary>
    /// Make this character say something.
    /// </summary>
    /// <param name="speech"></param>
    /// <param name="add"></param>
    public void Say(string speech, bool add = false)
    {
        if (!enabled)
        {
            enabled = true;
        }
        dialogue.Say(speech, add, Charactername);
    }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }
    /// <summary>
	/// Move to a specific point relative to the canvas space. (1,1) = far top right, (0,0) = far bottom left, (0.5,0.5) = Middle.
	/// </summary>
	/// <param name="Target">Target.</param>
	/// <param name="speed">Speed.</param>
	/// <param name="smooth">If set to <c>true</c> smooth.</param>
    public void MoveTo(Vector2 Target, float speed, bool smooth)
    {
        //if we are moving, stop moving
        StopMoving();
        //start moving coroutine.
        moving = CharacterManager.instance.StartCoroutine(Moving(Target, speed, smooth));
    }

    /// <summary>
	/// Stops the character in its tracks, either setting it immediately at the target position or not.
	/// </summary>
	/// <param name="arriveAtTargetPositionImmediately">If set to <c>true</c> arrive at target position immediately.</param>
    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if (isMoving)
        {
            CharacterManager.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        }
        moving = null;
    }

    /// <summary>
	/// Immediately set the position of this character to the intended target.
	/// </summary>
	/// <param name="target">Target.</param>
    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);

        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    /// <summary>
	/// The coroutine that runs to gradually move the character towards a position.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="speed">Speed.</param>
	/// <param name="smooth">If set to <c>true</c> smooth.</param>
    IEnumerator Moving(Vector2 target, float speed, bool smooth = true)
    {
        targetPosition = target;

        //now we want to get the padding between the anchors of this character so we know what their min and max positions are.
        Vector2 padding = anchorPadding;

        //now get the limitations for 0 to 100% movement. The farthest a character can move to the right before reaching 100% should be the 1 value - the padding (thickness of the character) so that 100% to the right/up does not place them outside of the canvas.
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        //now get the actual position target for the minimum anchors (left/bottom bounds) of the character. because maxX and maxY is just a percent reference.
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        //move until we reach the target position.
        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed)
                : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }
        StopMoving();
    }

    // Transitioning Images.
    public Sprite GetSprite(int index = 0)
    {
        // In case we want to load an individual sprite :
        // Do : Sprite sprites = Resources.Load<Sprite>("Images/Characters/" + characterName + "_" + emotion);
        // return sprites;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + Charactername);
        return sprites[index];
    }

    public Sprite GetSprite(string spriteName = "")
    {
        // In case we want to load an individual sprite :
        // Do : Sprite sprites = Resources.Load<Sprite>("Images/Characters/" + characterName + "_" + emotion);
        // return sprites;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + Charactername);
        
        for(int i = 0; i < sprites.Length; i++)
        {
            if(sprites[i].name == spriteName)
            {
                return sprites[i];
            }
        }
        return sprites.Length > 0 ? sprites[0] : null;
    }

    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);
    }
    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }
    public void SetBody(string spriteName)
    {
        renderers.bodyRenderer.sprite = GetSprite(spriteName);
    }

    public void SetExpression(int index)
    {
        renderers.expressionRenderer.sprite = GetSprite(index);
    }
    public void SetExpression(Sprite sprite)
    {
        renderers.expressionRenderer.sprite = sprite;
    }
    public void SetExpression(string spriteName)
    {
        renderers.expressionRenderer.sprite = GetSprite(spriteName);
    }
    //Transition Body
    bool isTransitioningBody { get { return transitioningBody != null; } }
    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite sprite, float speed, bool smooth)
    {
        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitioningBody(sprite, speed, smooth));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
            CharacterManager.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }

    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.bodyRenderer = image;
                break;
            }
        }

        if (renderers.bodyRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.bodyRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (GlobalF.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, smooth, true))
            yield return new WaitForEndOfFrame();

        Debug.Log("done");
        StopTransitioningBody();
    }

    //Transition Expression
    bool isTransitioningExpression { get { return transitioningExpression != null; } }
    Coroutine transitioningExpression = null;

    public void TransitionExpression(Sprite sprite, float speed, bool smooth)
    {
        StopTransitioningExpression();
        transitioningExpression = CharacterManager.instance.StartCoroutine(TransitioningExpression(sprite, speed, smooth));
    }

    void StopTransitioningExpression()
    {
        if (isTransitioningExpression)
            CharacterManager.instance.StopCoroutine(transitioningExpression);
        transitioningExpression = null;
    }

    public IEnumerator TransitioningExpression(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allExpressionRenderers.Count; i++)
        {
            Image image = renderers.allExpressionRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.expressionRenderer = image;
                break;
            }
        }

        if (renderers.expressionRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.expressionRenderer.gameObject, renderers.expressionRenderer.transform.parent).GetComponent<Image>();
            renderers.allExpressionRenderers.Add(image);
            renderers.expressionRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (GlobalF.TransitionImages(ref renderers.expressionRenderer, ref renderers.allExpressionRenderers, speed, smooth, true))
            yield return new WaitForEndOfFrame();

        Debug.Log("done");
        StopTransitioningExpression();
    }

    //End Transition Expression
    public void Flip()
    {
        root.localScale = new Vector3(root.localScale.x * -1, 1, 1);
    }
    public void FlipLeft()
    {
        root.localScale = Vector3.one;
    }
    public void FlipRight()
    {
        root.localScale = new Vector3(root.localScale.x * -1, 1, 1);
    }

    public void FadeOut(float speed = 3, bool smooth = false)
    {
        Sprite alphaSprite = GetSprite("AlphaSprite");

        LastBodySprite = renderers.bodyRenderer.sprite;
        LastBodySprite = renderers.expressionRenderer.sprite;

        Debug.Log("Alpha : " + alphaSprite);
        TransitionBody(alphaSprite, speed, smooth);
        TransitionExpression(alphaSprite, speed, smooth);
    }
    Sprite LastBodySprite, LastFacialSprite = null;
    public void FadeIn(float speed = 3, bool smooth = false)
    {
        if(LastBodySprite != null && LastFacialSprite != null)
        {
            TransitionBody(LastBodySprite, speed, smooth);
            TransitionExpression(LastFacialSprite, speed, smooth);
        }
    }
    [System.Serializable]
    public class Renderers
    {

        // used as the only image for a single layer Character
        public RawImage renderer;

        // used as the body and expression images in a multiple layer Character

        //sprites use images.
        /// <summary>
        /// The body renderer for a multi layer character.
        /// </summary>
        public Image bodyRenderer;
        /// <summary>
        /// The expression renderer for a multi layer character.
        /// </summary>
        public Image expressionRenderer;

        public List<Image> allBodyRenderers = new List<Image>();
        public List<Image> allExpressionRenderers = new List<Image>();
    }
    public Renderers renderers = new Renderers();
}
