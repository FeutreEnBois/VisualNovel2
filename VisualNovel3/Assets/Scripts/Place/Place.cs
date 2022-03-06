public class Place
{
    protected Place lastPlace = null;

    public Place(Place lastPlace)
    {
        this.lastPlace = lastPlace;
    }

    public virtual void OnStart() { }
    public virtual void OnQuit() { }

    public void Load()
    {
        if(lastPlace != null)
        {
            lastPlace.OnQuit();
        }
        OnStart();
    }
}
