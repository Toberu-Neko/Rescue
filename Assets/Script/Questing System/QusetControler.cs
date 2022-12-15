using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QusetControler : MonoBehaviour
{
    //public Quest quest;
    //public PlayerStates playerStates;
    
    private GameObject UI;
    private GameObject questHUD;
    private TextMeshProUGUI questTitle;
    private TextMeshProUGUI questDescription;
    private void Start()
    {
        UI = UIManager.instance.UI;
        questHUD = UI.transform.Find("HUD/QuestHUD").gameObject;
        questHUD.SetActive(false);
        questTitle = UI.transform.Find("HUD/QuestHUD/任務名稱Text").GetComponent<TextMeshProUGUI>();
        questDescription = UI.transform.Find("HUD/QuestHUD/任務內容Text").GetComponent<TextMeshProUGUI>();
    }
    public void QusetAccepted(Quest quest)
    {
        questHUD.SetActive(true);
        questTitle.text = quest.title;
        questDescription.text = quest.description;
        quest.isActive = true;
    }
}
