using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerDataTransfer
{
    public int currentScene;
    public float playTime;
    public bool gameEnded;

    public int maxHealth;
    public int currentHealth;

    public float[] position;

    public int skillSlot;
    public bool swim;
    public bool regen;
    public bool throwStone;
    public bool throwFire;
    public bool throwDebug;

    public string nowGoal;
    //public bool[] questFinished;

    public PlayerDataTransfer(PlayerStates playerStates)
    {
        currentScene = playerStates.currentScene;
        playTime = playerStates.playTime;
        gameEnded = playerStates.gameEnded;

        maxHealth = playerStates.maxHealth;
        currentHealth = playerStates.currentHealth;

        nowGoal = playerStates.nowGoal;

        skillSlot = playerStates.skillSlot;
        swim = playerStates.swimAble;
        regen = playerStates.regenAble;
        throwStone = playerStates.throwStoneAble;
        throwFire = playerStates.throwFireAble;
        throwDebug = playerStates.throwDebugAble;


        position = new float[3];
        position[0] = playerStates.transform.position.x;
        position[1] = playerStates.transform.position.y;
        position[2] = playerStates.transform.position.z;
    }
}
