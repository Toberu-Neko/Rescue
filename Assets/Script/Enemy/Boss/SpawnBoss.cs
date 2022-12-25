using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] private PlayerData playerData;
    void Start()
    {
        //playerData = PlayerManager.instance.player.GetComponent<PlayerStates>();

        if (!playerData.gameEnded)
        {
            Instantiate(boss, transform.position, transform.rotation);
        }
    }

    void Update()
    {
        
    }
}
