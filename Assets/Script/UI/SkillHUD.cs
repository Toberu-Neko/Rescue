using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHUD : MonoBehaviour
{
    [SerializeField] private Sprite regen;
    [SerializeField] private Sprite swim;
    [SerializeField] private GameObject insImageObj;
    private PlayerStates playerStates;
    private GameObject regenIcon;
    private GameObject swimIcon;
    bool regenSpawned;
    bool swimSpawned;
    private void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();

        regenSpawned = false;
    }
    private void Update()
    {
        if (playerStates.regenAble && !regenSpawned)
        {
            regenSpawned = true;
            regenIcon = Instantiate(insImageObj, transform);
            regenIcon.GetComponent<Image>().sprite = regen;
        }
        if(playerStates.swimAble && !swimSpawned)
        {
            swimSpawned = true;
            swimIcon = Instantiate(insImageObj, transform);
            swimIcon.GetComponent<Image>().sprite = swim;
        }
        if(!playerStates.regening && regenSpawned)
        {
            regenIcon.GetComponent<Image>().color = Color.gray;
        }
        if (playerStates.regening && regenSpawned)
        {
            regenIcon.GetComponent<Image>().color = Color.white;
        }
    }
}
