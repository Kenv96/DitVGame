using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DialogueEditor;

[CreateAssetMenu(fileName = "New Quest", menuName = "Questing/Quest")]

public class QuestScriptObj : ScriptableObject
{
    [SerializeField]
    private int questCode = 0;
    public string title;
    public Item[] itemReward;
    public int goldReward;

    public bool isActive;
    public bool isComplete;

    public QuestGoal[] questStepsSetup;

    public QuestGoal currentStep;

    public int questStepCount;

    private void Awake()
    {
        isActive = false;
        isComplete = false;
    }

    public int GetQuestCode()
    {
        return questCode;
    }

    public void StartQuest()
    {
        if (!isActive && !isComplete)
        {
            isActive = true;
            currentStep = questStepsSetup[0];
        }
    }

    public void CompleteStep()
    {
        if (isActive && questStepCount < (questStepsSetup.Length - 1))
        {
            questStepCount++;
            currentStep = questStepsSetup[questStepCount];
            QuestManager.GetInstance().UpdateUI();
        }
        else
        {
            QuestManager.GetInstance().FinishQuest(this);
        }
    }

    //Steps
    public void EnemyKilled(string tarEnemy)
    {
        if (currentStep.goalType == GoalType.Kill)
        {
            if (tarEnemy == currentStep.targetEnemy)
            {
                currentStep.currentAmountKill++;
                if (currentStep.currentAmountKill >= currentStep.requiredAmountKill)
                    CompleteStep();
            }
        }
    }

    public void ItemCollected(Item required)
    {
        if (currentStep.goalType == GoalType.Gather)
        {
            if (required == currentStep.requiredItem)
            {
                currentStep.currentAmountGather++;
                if (currentStep.currentAmountGather >= currentStep.requiredAmountGather)
                    CompleteStep();
            }
        }
    }

    public void DeliverItem(Item requiredItem)
    {
        if (currentStep.goalType == GoalType.Deliver)
        {
            if(Inventory.instance.CheckInv(requiredItem, currentStep.amountToDeliver))
            {
                Inventory.instance.RemoveMultiple(requiredItem, currentStep.amountToDeliver);
                CompleteStep();
            }     
        }
    }
    

    public void TalkedTo()
    {
        if (currentStep.goalType == GoalType.Talk)
        {
            CompleteStep();
        }
    }

    public void ResetQuest()
    {
        isActive = false;
        isComplete = false;
        questStepCount = 0;
        currentStep = questStepsSetup[0];
        currentStep.currentAmountGather = 0;
        currentStep.currentAmountKill = 0;
    }
}
