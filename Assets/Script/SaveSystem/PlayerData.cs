using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : MonoBehaviour
{
    public int currentScene;
    public float playTime;
    public bool gameEnded;

    public int maxHealth;
    public int currentHealth;
    public Vector3 position;

    public string nowGoal;
    //public bool[] questFinished;

    [Header("技能獲得狀態")]
    public int skillSlot;
    public bool swim;
    public bool regen;
    public bool throwStone;
    public bool throwFire;
    public bool throwDebug;


}
