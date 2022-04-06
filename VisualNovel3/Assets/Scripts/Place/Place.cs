public class Place
{

    public Place()
    {
    }

    public virtual void OnStart() { }
    public virtual void OnQuit() { }

    public void Load(Place lastPlace)
    {
        if(lastPlace != null)
        {
            lastPlace.OnQuit();
        }
        OnStart();
    }
}
