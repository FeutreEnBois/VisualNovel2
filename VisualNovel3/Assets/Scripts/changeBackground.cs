using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeBackground : MonoBehaviour
{

    public GameObject currentBg;
    public GameObject goToBg;
    //public bool canGoToBg = false;
    // Start is called before the first frame update

    public void backGroundChanger()
    {
        //if (canGoToBg == false)
        //{
            currentBg.SetActive(false);
            goToBg.SetActive(true);
           // canGoToBg = true;
        //}
        /*else
        {
            goToBg.SetActive(false);
            currentBg.SetActive(true);
            canGoToBg = false;
        }*/
    }
}
