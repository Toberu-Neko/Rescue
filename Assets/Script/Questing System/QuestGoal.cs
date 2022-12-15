using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public GoalType goalType;
    public TalkToTarget talkTo;


    public int requiredAmount;
    public int currentAmount;

    
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
    public void Talked()
    {
        if(goalType == GoalType.Talk)
        {
            currentAmount++;
        }
    }
    public enum GoalType
    {
        Kill,
        Talk
    }
    public enum TalkToTarget
    {
        None,
        Mango,
        Wowo,
        Avocado
    }
}
