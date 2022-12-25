using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryIfEnd : MonoBehaviour
{
    PlayerData playerData;
    void Start()
    {
        playerData = PlayerManager.instance.playerData;
        if (playerData.gameEnded)
            Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
