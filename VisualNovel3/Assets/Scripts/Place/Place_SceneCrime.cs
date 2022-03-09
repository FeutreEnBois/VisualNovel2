using UnityEngine;

public class Place_SceneCrime : Place
{
    public Place_SceneCrime(Place lastPlace) : base(lastPlace)
    {
    }

    public override void OnStart()
    {
        Texture2D texture = Resources.Load("Images/UI/Backdrops/SceneCrime") as Texture2D;
        Texture2D transition = Resources.Load("Images/TransitionEffects/Blur") as Texture2D;
        TransitionManager.TransitionLayer(BCFC.instance.background, texture, transition);
        GameplayManager.instance.TogglePointAndClickScene();
    }

    public override void OnQuit()
    {
        base.OnQuit();
        GameplayManager.instance.TogglePointAndClickScene();

    }
}
