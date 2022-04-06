using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public GameObject inventaire;
    public GameObject journal;
    public GameObject boussole;
    public GameObject accuser;

    public GameObject PanelInventaire;
    public GameObject PanelJournal;
    public GameObject PanelDestination;
    public GameObject PanelAccuser;

    public GameObject[] PanelOptions = new GameObject[4];
    public static OptionManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PanelOptions[0] = PanelInventaire;
        PanelOptions[1] = PanelJournal;
        PanelOptions[2] = PanelDestination;
        PanelOptions[3] = PanelAccuser;
    }
    public void ToggleElement(string option)
    {
        switch (option)
        {
            case "Inventaire":
                inventaire.SetActive(true);
                break;
            case "Journal":
                journal.SetActive(true);
                break;
            case "Boussole":
                boussole.SetActive(true);
                break;
            case "Accuser":
                accuser.SetActive(true);
                break;
        }
    }

    public void TogglePanelOption(GameObject option)
    {
        for (int i = 0; i < PanelOptions.Length; i++)
        {
            if(option != PanelOptions[i])
            {
                PanelOptions[i].SetActive(false);
            }
            else
            {
                PanelOptions[i].SetActive(!PanelOptions[i].activeInHierarchy);
            }
        }


    }

}
