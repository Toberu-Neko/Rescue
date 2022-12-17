using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    PlayerStates playerStates;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Enter!");
            playerStates.saveAble = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStates.saveAble = false;
        }
    }
    void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
    }

}
