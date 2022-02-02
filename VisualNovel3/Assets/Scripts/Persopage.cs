using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persopage : MonoBehaviour
{
    public GameObject persopage;
    public GameObject journal;
    public GameObject headHolder;
    public GameObject enquete;

    public int ID;
    public bool persopageEnabled = false;
    public int persoID;

    private GameObject[] head;
    private GameObject[] page;
    
    
    private int allHeads;
    private int allPages;
   

    void Start()
    {
        allPages = 12;
        allHeads = 12;
        head = new GameObject[allHeads];
        page = new GameObject[allPages];

        for (int i = 0; i < allHeads; i++)
        {
            head[i] = headHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < allPages; i++)
        {
            page[i] = persopage.transform.GetChild(i).gameObject;
        }


    }

     void Update()
    {
       
            
        
   

        if (persopageEnabled == true)
        {

            persopage.SetActive(true);
            journal.SetActive(false);
            enquete.SetActive(false);


        }
        else
        {
            persopage.SetActive(false);
            journal.SetActive(true);
            enquete.SetActive(true);
        }
    }   
    public void clickHead(GameObject head)
    {

        ID = head.GetComponent<Head>().ID;
        persopageEnabled = !persopageEnabled;
        Debug.Log("Hey MOn pote");
        for (int i = 0; i < allPages; i++) {
            if (page[i].GetComponent<Page>().ID != ID)
            {
                page[i].SetActive(false);


            }
            else
            {
                page[i].SetActive(true);
            } 
            }

    

    }
    public void GoBack()
    {
        ID = 0;
        persopageEnabled = !persopageEnabled;


    }

    
}
