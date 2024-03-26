using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct Line
{
    public string ncpName;
    public LocalizedString localizedncpName;
    [TextArea(3,6)]
    public string sentences;
    public LocalizedString localizedSentences;
}
[CreateAssetMenu(fileName ="新對話", menuName = "新增對話")]
public class DialogueScriptableObj : ScriptableObject
{
    [Header("對話基本資料")]
    public string dialogueName;
    public QuestGoal.TalkToTarget NpcName;
    public bool isTutorial;
    public bool completed;

    [Header("任務資料（暫時請勿使用）")]
    [HideInInspector] public bool haveQuest;
    [HideInInspector] public Quest quest;

    [Header("對話給予能力")]
    public bool giveRegen;
    public bool giveThrowStone;
    public bool giveSwim;
    public bool giveFire;
    public bool giveDebug;

    [Header("對話簡易提示目標")]
    public bool haveGoal;
    public string nowGoal;
    public LocalizedString localizedNowGoal;

    [Header("對話")]
    public Line[] lines;
}
