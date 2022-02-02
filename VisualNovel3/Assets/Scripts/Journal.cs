using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    public GameObject journal;
    private bool journalEnabled;
    

    private int allHeads;
    private int enabledHeads;
    private GameObject[] head;

    public GameObject headHolder;
    public int persopage = 0;
    // Start is called before the first frame update
    void Start()
    {
        allHeads = 5;
        head = new GameObject[allHeads];
        
        for (int i = 0; i < allHeads; i++)
        {
            head[i] = headHolder.transform.GetChild(i).gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            journalEnabled = !journalEnabled;

        if (journalEnabled == true)
        {
            
            journal.SetActive(true);
            

        }
        else
        {
            journal.SetActive(false);
        }
        
        

    }
   

}
