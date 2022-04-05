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

        if (NovelController.instance.GetCondition("Preuve,Story,Bar,false")){
            NovelController.instance.LoadChapterFile("Chapter0_01b");
        }
    }

    public override void OnQuit()
    {
    }
}
