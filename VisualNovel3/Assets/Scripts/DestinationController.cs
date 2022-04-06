using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DestinationController : MonoBehaviour
{
    public string transition;
    public GameObject destinationPanel;

    [SerializeField] private Dictionary<string, DESTINATION> knownDestination = new Dictionary<string, DESTINATION>();

    public static Place placeBar = new Place_Bar();
    public static Place placeCrime = new Place_SceneCrime();
    public static Place placeAccuser = new Place_Accuser();

    public GameObject destinationSubPanelBar;
    public GameObject destinationSubPanelCrime;
    public GameObject destinationSubPanelAccuser;

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

    public void addAKnownDestination(string destName)
    {
        switch (destName)
        {
            case "Bar":
                destinationSubPanelBar.SetActive(true);
                return;
            case "SceneCrime":
                destinationSubPanelCrime.SetActive(true);
                return;
            case "Accuser":
                destinationSubPanelAccuser.SetActive(true);
                return;
        }

    }

    public void ChangeDestination(string name)
    {
        destinationPanel.SetActive(false);
        NovelController.instance.Command_ChangePlace(name);
    }

    public void ToggleDestinationPanel()
    {
        OptionManager.instance.TogglePanelOption(destinationPanel);
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
