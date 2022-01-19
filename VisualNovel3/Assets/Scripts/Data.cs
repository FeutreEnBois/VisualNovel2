using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;

    private void Awake()
    {
        instance = this;
    }

    public bool Marlo_a1 = false;
    public bool Interrogatoire_Marlo = false;
}
