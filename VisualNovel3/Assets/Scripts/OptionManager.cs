using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public GameObject inventaire;
    public GameObject journal;
    public GameObject boussole;
    public GameObject accuser;

    public static OptionManager instance;
    private void Awake()
    {
        instance = this;
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

}
