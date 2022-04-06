using UnityEngine;

public class Place_SceneCrime : Place
{
    public Place_SceneCrime() : base()
    {
    }

    public override void OnStart()
    {
        Texture2D texture = Resources.Load("Images/UI/Backdrops/SceneCrime") as Texture2D;
        Texture2D transition = Resources.Load("Images/TransitionEffects/Blur") as Texture2D;
        TransitionManager.TransitionLayer(BCFC.instance.background, texture, transition,1,true);

        if (NovelController.instance.GetCondition("Preuve,Story,visite_scene_crime,false"))
        {
            NovelController.instance.LoadChapterFile("Chapter0_01c");
        }
        else
        {
            GameplayManager.instance.TogglePointAndClickScene();
            NovelController.instance.LoadChapterFile("Destination/Destination_SceneCrime");
        }

    }

    public override void OnQuit()
    {
        base.OnQuit();
        GameplayManager.instance.TogglePointAndClickScene();

    }
}
