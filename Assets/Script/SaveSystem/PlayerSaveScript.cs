using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveScript : MonoBehaviour
{

    [SerializeField]private PlayerData playerData;
    private PlayerStates playerStates;
    private LevelLoader levelLoader;
    private void Start()
    {
        levelLoader = UIManager.instance.UI.GetComponent<LevelLoader>();
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        playerData = PlayerManager.instance.playerData;
        //Debug.Log(playerData.currentHealth);
    }
    public void AutoSave()
    {
        SavePlayer(0);
    }
    public void AutoLoad()
    {
        LoadPlayer(0);
    }
    public void SavePlayer(int saveSlot)
    {
        playerStates.currentScene = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SaveSystem.SavePlayer(playerStates, saveSlot);
    }
    public void LoadPlayer(int saveSlot)
    {
        PlayerDataTransfer data = SaveSystem.LoadPlayer(saveSlot);
        playerData = PlayerManager.instance.playerData;
        levelLoader = UIManager.instance.UI.GetComponent<LevelLoader>();

        if(data == null)
        {
            return;
        }
        //Debug.Log(data.nowGoal);
        //Debug.Log(playerData.currentHealth);

        playerData.playTime = data.playTime;
        playerData.currentHealth = data.currentHealth;
        playerData.maxHealth = data.maxHealth;
        playerData.gameEnded = data.gameEnded;

        playerData.skillSlot = data.skillSlot;
        playerData.swim = data.swim;
        playerData.regen = data.regen;
        playerData.throwStone = data.throwStone;
        playerData.throwFire = data.throwFire;
        playerData.throwDebug = data.throwDebug;

        playerData.nowGoal = data.nowGoal;


        playerData.currentScene = data.currentScene;
        //for (int i = 0; i < playerData.questFinished.Length; i++) 
        //{
         //   playerData.questFinished[i] = data.questFinished[i];
            /*Debug.Log(i);
            Debug.Log(data.questFinished.Length);
            Debug.Log(playerData.questFinished[i]);
            Debug.Log(data.questFinished[i]);*/
        //} 

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        playerData.position = position;

        levelLoader.LoadLevel(playerData.currentScene);


    }



}
