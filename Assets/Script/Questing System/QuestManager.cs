using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public DialogueScriptableObj[] dialogueScriptableObjs;

    private void Start()
    {
        for (int i = 0; i < dialogueScriptableObjs.Length; i++) 
        {
            //dialogueScriptableObjs[i].quest.goal.currentAmount = 0;
            //dialogueScriptableObjs[i].quest.completed = false;
        }
    }
}
