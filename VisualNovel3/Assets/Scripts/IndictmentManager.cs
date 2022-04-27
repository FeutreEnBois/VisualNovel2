using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndictmentManager : MonoBehaviour
{
    private string accuser;
    public GameObject AccusationPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            ToggleKillerChoosingPanel();

    }

    public void ToggleKillerChoosingPanel()
    {
        OptionManager.instance.TogglePanelOption(AccusationPanel);
    }

    public void ChooseASuspect(Text suspect)
    {
        if (Inventory.instance.preuvesCount < 2)
        {
            return;
        }
        accuser = suspect.text;
        Debug.Log(accuser);
        if(accuser == "Marlo")
        {
            NovelController.instance.LoadChapterFile("Accusation/Accusation_" + accuser);
        }
        else
        {
            NovelController.instance.LoadChapterFile("Interrogatoire/"+ accuser + "/Accusation/Accusation_" + accuser);
        }
        AccusationPanel.SetActive(false);
    }


}
