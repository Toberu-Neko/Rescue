using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class riverDia : MonoBehaviour
{
    PlayerStates playerStates;
    // Start is called before the first frame update
    void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStates.swimAble)
            gameObject.SetActive(false);
    }
}
