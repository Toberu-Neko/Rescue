using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [Header("增加或減少技能時記得改腳本")]
    private GameObject[] mainSkill;
    private TextMeshProUGUI[] mainSkillText;
    [SerializeField] private Image[] mainSkillImage;


    private Button[] subSkillButton;


    [SerializeField] private Sprite regenIcon;
    [SerializeField] private Sprite swimIcon;

    private PlayerCombat playerCombat;
    private PlayerStates playerStates;
    void Awake()
    {
        mainSkill = new GameObject[4];
        mainSkillText = new TextMeshProUGUI[4];
        mainSkillImage = new Image[4];
        subSkillButton = new Button[4];
        int mainSkillCount = 0;
        int subSkillCount = 0;
        foreach (Transform findingObj in gameObject.transform)
        {
            if (findingObj.CompareTag("SkillUIMainSkill"))
            {
                mainSkill[mainSkillCount] = findingObj.gameObject;
                mainSkillText[mainSkillCount] = findingObj.Find("Skill內文 Text").GetComponent<TextMeshProUGUI>();
                mainSkillImage[mainSkillCount] = findingObj.Find("SkillImage").GetComponent<Image>();
                mainSkillCount++;
            }
            if (findingObj.CompareTag("SkillUISubSkill"))
            {
                subSkillButton[subSkillCount] = findingObj.gameObject.GetComponent<Button>();
                subSkillCount++;
            }
        }

        playerCombat = PlayerManager.instance.player.GetComponent<PlayerCombat>();
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        //Debug.Log(playerStates.swimAble);

    }
    private void Update()
    {
        if (!playerStates.throwStoneAble || playerStates.skillSlot == 1)
        {
            subSkillButton[1].interactable = false;
        }
        else if (playerStates.throwStoneAble)
        {
            subSkillButton[1].interactable = true;
        }
        if (!playerStates.throwFireAble || playerStates.skillSlot == 2)
        {
            subSkillButton[2].interactable = false;
        }
        else if (playerStates.throwFireAble)
        {
            subSkillButton[2].interactable = true;
        }
        if (!playerStates.throwDebugAble || playerStates.skillSlot == 3)
        {
            subSkillButton[3].interactable = false;
        }
        else if (playerStates.throwDebugAble)
        {
            subSkillButton[3].interactable = true;
        }
    }
    private void OnEnable()
    {
        int mainSkillCount = 0;
        if (playerStates.regenAble)
        {
            mainSkill[mainSkillCount].SetActive(true);
            mainSkillImage[mainSkillCount].sprite = regenIcon;
            mainSkillImage[mainSkillCount].color= Color.white;
            mainSkillText[mainSkillCount].text = "治癒";
            mainSkillCount++;
        }
        if (playerStates.swimAble)
        {
            mainSkill[mainSkillCount].SetActive(true);
            mainSkillImage[mainSkillCount].sprite = swimIcon;
            mainSkillImage[mainSkillCount].color = Color.white;
            mainSkillText[mainSkillCount].text = "游泳";
            mainSkillCount++;
        }





        for (int i = mainSkillCount; i < mainSkill.Length; i++)
        {
            mainSkill[i].SetActive(false);
        }

    }
    public void ChangeSkill(int skillSlot)
    {
        playerCombat.SelectSkill(skillSlot);
    }

}
