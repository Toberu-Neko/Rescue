using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    public GameObject[] save_Load_Button;
    private TextMeshProUGUI[] playTimeText, skillText, goalText;
    void Awake()
    {
        playTimeText = new TextMeshProUGUI[save_Load_Button.Length];
        skillText = new TextMeshProUGUI[save_Load_Button.Length];
        goalText = new TextMeshProUGUI[save_Load_Button.Length];
        for (int i = 0; i < save_Load_Button.Length; i++)
        {
            //Debug.Log(save_Load_Button.Length);

            playTimeText[i] = save_Load_Button[i].transform.Find("遊玩時間Text").gameObject.GetComponent<TextMeshProUGUI>();
            skillText[i] = save_Load_Button[i].transform.Find("持有能力Text").gameObject.GetComponent<TextMeshProUGUI>();
            goalText[i] = save_Load_Button[i].transform.Find("目標Text").gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
    private void OnEnable()
    {
        RefreshSave();
    }
    public void RefreshSave()
    {
        for (int i = 0; i < save_Load_Button.Length; i++)
        PreviewSave(i);
    }
    public void PreviewSave(int saveSlot)
    {
        PlayerDataTransfer data = SaveSystem.LoadPlayer(saveSlot);

        float playTime;
        string skillText = "";
        bool haveSkill = false;

        if (data != null)
        {
            playTime = data.playTime;
            playTime -= playTime % 1;
            #region 技能狀態判定
            if (data.regen)
            {
                skillText += "治癒、";
                haveSkill = true;
            }
            if (data.swim)
            {
                skillText += "游泳、";
                haveSkill = true;
            }
            if(data.throwStone)
            {
                skillText += "投石、";
                haveSkill = true;
            }
            if (data.throwFire)
            {
                skillText += "噴火、";
                haveSkill = true;
            }
            if (haveSkill)
            {
                skillText = skillText.Remove(skillText.Length - 1);
                skillText += "。";
            }
            if(!haveSkill)
            {
                skillText = "無技能。";
            }
            #endregion

            DisplaySaveSlot(true, saveSlot, playTime, skillText, data.nowGoal);
        }
        if(data == null)
        {
            DisplaySaveSlot(false, saveSlot, -1, "skill", "goal");
        }
    }
    void DisplaySaveSlot(bool haveSave, int saveSlot, float playTime, string skill, string goal)
    {
        if (haveSave)
        {
            #region 時間換算
            int day =0, hour =0, minute=0, second=0;
            string secondColon = ":", minColon = ":", hourColon = "";
            while(playTime >= 60)
            {
                playTime -= 60;
                minute++;
                if (minute >= 60)
                {
                    minute -= 60;
                    hour++;
                    if(hour >= 24)
                    {
                        hour -= 24;
                        day++;
                    }
                }
            }
            second = (int)playTime;
            if(second < 10)
            {
                secondColon = ":0";
            }
            if(minute < 10)
            {
                minColon = ":0";
            }
            if(hour < 10)
            {
                hourColon = ":0";
            }
            #endregion

            playTimeText[saveSlot].text = "遊玩時間：" + day + hourColon + hour + minColon + minute + secondColon + second;
            skillText[saveSlot].text = "持有能力：" + skill;
            goalText[saveSlot].text = "目標：" + goal;
        }
        if (!haveSave)
        {
            playTimeText[saveSlot].text = "";
            skillText[saveSlot].text = "";
            goalText[saveSlot].text = "無存檔";
        }
    }




}
