using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_MainMenu : MonoBehaviour
{

    void Start()
    {
        AudioManager.instance.Play("BGM_MainMenu"); 
        AudioManager.instance.Stop("BGM_StartAni");
        AudioManager.instance.Stop("BGM_L0");
        AudioManager.instance.Stop("BGM_L1");
        AudioManager.instance.Stop("BGM_FinalAni");
    }

    public void StartAniBGM()
    {
        AudioManager.instance.Stop("BGM_MainMenu");
        AudioManager.instance.Play("BGM_StartAni");
    }

}
