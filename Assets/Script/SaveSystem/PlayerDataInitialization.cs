using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataInitialization : MonoBehaviour
{
    public PlayerData playerData;
    [SerializeField] private Vector3 L1Position;

    public void DataInitialization()
    {
        playerData.currentScene = 1;
        playerData.playTime = 0;
        playerData.gameEnded = false;

        playerData.maxHealth = 150;
        playerData.currentHealth = playerData.maxHealth / 2;
        playerData.position = L1Position;
        playerData.nowGoal = " ";

        //playerData.questFinished = new bool[QuestManager.instance.dialogueScriptableObjs.Length];
        /*for (int i = 0; i < playerData.questFinished.Length; i++)
        {
            playerData.questFinished[i] = false;
        }*/

        playerData.skillSlot = 0;
        playerData.swim = false;
        playerData.throwStone = false;
        playerData.regen = false;
        playerData.throwFire = false;
        playerData.throwDebug = false;
    }
}
