using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_L1 : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("BGM_L1"); 
        AudioManager.instance.Play("BGM_Rain");
    }
}
