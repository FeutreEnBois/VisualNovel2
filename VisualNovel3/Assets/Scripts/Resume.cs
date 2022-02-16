using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject resume;
    private bool resumeEnabled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (resumeEnabled == true)
        {

            resume.SetActive(true);


        }
        else
        {
            resume.SetActive(false);
        }
        

    }
    public void EnqueteButton()
    {
        resumeEnabled = true;

    }
    public void GoBack()
    {
        resumeEnabled = false;

    }
}
