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

    public void ToggleKillerChoosingPanel()
    {
        AccusationPanel.SetActive(!AccusationPanel.activeInHierarchy);
    }
    public void ChooseASuspect(TextMeshProUGUI suspect)
    {
        if (Inventory.instance.preuvesCount < 2)
        {
            return;
        }
        accuser = suspect.text;
        Debug.Log(accuser);
        NovelController.instance.LoadChapterFile("Interrogatoire/"+ accuser + "/Accusation/Accusation_" + accuser);
        AccusationPanel.SetActive(false);
    }


}
