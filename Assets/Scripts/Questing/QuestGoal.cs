using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public string objDesc;

    [Header("Kill")]
    public string targetEnemy;
    public int requiredAmountKill;
    //[HideInInspector]
    public int currentAmountKill;

    [Header("Gather")]
    public Item requiredItem;
    public int requiredAmountGather;
    //[HideInInspector]
    public int currentAmountGather;

    [Header("Deliver")]
    public string deliverTo;
    public Item itemToBeDelivered;
    public int amountToDeliver;

    [Header("Talk")]
    public string talkTo;
}

public enum GoalType
{
    Kill,
    Gather,
    Deliver,
    Talk
}
