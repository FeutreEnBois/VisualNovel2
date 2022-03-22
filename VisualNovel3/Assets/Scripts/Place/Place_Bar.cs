using UnityEngine;

public class Place_Bar : Place
{
    public Place_Bar(Place lastPlace) : base(lastPlace)
    {
    }

    public override void OnStart()
    {
        Texture2D texture = Resources.Load("Images/UI/Backdrops/Bar") as Texture2D;
        Texture2D transition = Resources.Load("Images/TransitionEffects/Blur") as Texture2D;
        TransitionManager.TransitionLayer(BCFC.instance.background, texture, transition);
        Debug.Log("ta mere");
    }

    public override void OnQuit()
    {
    }
}
