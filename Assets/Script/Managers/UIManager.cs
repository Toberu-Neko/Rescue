using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    public GameObject UI;
    public DialogueManager dialogueManager;
    public QusetControler qusetControler;
}
