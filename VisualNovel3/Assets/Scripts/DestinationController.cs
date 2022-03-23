using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DestinationController : MonoBehaviour
{
    public string transition;
    public GameObject destinationPanel;

    [SerializeField] private Dictionary<string, DESTINATION> knownDestination = new Dictionary<string, DESTINATION>();
    public static DestinationController instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("!!! trying to create multiple instance of DestinationController");
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ToggleDestinationPanel();

    }

    public void addAKnownDestination(string destName, string destDescription, Texture2D destBackground, Texture2D destTransition)
    {
        if (knownDestination.ContainsKey(destName)){
            Debug.LogWarning(destName + "is already known");
            return;
        }
        knownDestination[destName] = new DESTINATION(destName,destDescription,destBackground,destTransition);

    }
    public void ChangeDestination(string name)
    {
        NovelController.instance.Command_ChangePlace(name);
        NovelController.instance.LoadChapterFile("Destination/Destination_" + name);
    }

    public void ToggleDestinationPanel()
    {
        destinationPanel.SetActive(!destinationPanel.activeInHierarchy);
    }

    class DESTINATION
    {
        public string name;
        public string description;
        public Texture2D background;
        public Texture2D transition;

        public DESTINATION(string destName, string destDescription, Texture2D destBackground, Texture2D destTransition)
        {
            this.name = destName;
            this.description = destDescription;
            this.background = destBackground;
            this.transition = destTransition;
        }
    }
}
