using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class SelectKillerSlotContainer : MonoBehaviour
{
    public string name;
    public List<string> preuves = new List<string>();
    public List<TextMeshProUGUI> preuvesText = new List<TextMeshProUGUI>();
    public int nbrPreuve = 0;
    // Start is called before the first frame update

    private void OnEnable()
    {
        
    }

    public void addPreuve(string p)
    {
        Debug.Log(p);
        //preuvesText[nbrPreuve].text = p;
        nbrPreuve++;
    }


}
