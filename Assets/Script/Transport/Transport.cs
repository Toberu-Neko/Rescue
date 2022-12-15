using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    [SerializeField] private Vector3 tpPosition;
    [SerializeField] private int tpScene;
    private PlayerData playerData;
    private PlayerStates playerStates;
    private LevelLoader levelLoader;
    void Start()
    {
        levelLoader = UIManager.instance.UI.GetComponent<LevelLoader>();
        playerStates = PlayerManager.instance.player.gameObject.GetComponent<PlayerStates>();
        playerData = PlayerManager.instance.playerData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && tpScene != 0) 
        {
            playerData.skillSlot = playerStates.skillSlot;
            playerData.currentScene = tpScene;
            playerData.position = tpPosition;
            playerData.currentHealth = playerStates.currentHealth;
            playerData.nowGoal = playerStates.nowGoal;
            playerData.playTime = playerStates.playTime;

            levelLoader.LoadLevel(2);
        }
    }
}
