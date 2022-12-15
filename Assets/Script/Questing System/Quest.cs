using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int index;
    //[HideInInspector]
    public bool isActive;

    public string title;
    [TextArea(2, 5)]
    public string description;

    public QuestGoal goal;
    public bool giveRegen;
    public bool giveThrowStone;
    public bool giveSwim;

    public bool completed;
}
