using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    //General Quest info
    //public List<Interactable> questTriggers;
    public QuestGoal[] questStepsSetup;
    //public Queue<QuestGoal> questSteps = new Queue<QuestGoal>();
    public QuestGoal currentStep;
    public int questStepCount;

    public bool isActive;
    public bool isComplete;

    public string title;
    //public string description;
    public int goldReward;

    public void StartQuest()
    {
        if(!isActive && !isComplete)
        {
            currentStep = questStepsSetup[0];
            isActive = true;
        }
    }

    public void CompleteStep()
    {
        if (isActive && questStepCount < (questStepsSetup.Length - 1))
        {
            //currentStep.stepComplete = true;
            questStepCount++;
            currentStep = questStepsSetup[questStepCount];
            //QuestManager.GetInstance().UpdateUI(this);
        }
        else
        {
           // QuestManager.GetInstance().FinishQuest(this);
        }
    }

    //Steps
    public void EnemyKilled(string tarEnemy)
    {
        if(currentStep.goalType == GoalType.Kill)
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
                if(currentStep.currentAmountGather>= currentStep.requiredAmountGather)
                    CompleteStep();
            }
        }
    }

    public void DeliverItem(NPCInteraction targetNPC)
    {
        if (currentStep.goalType == GoalType.Deliver)
        {
            if (targetNPC.NPCName == currentStep.deliverTo)
            {
                if (Inventory.instance.items.Contains(currentStep.itemToBeDelivered))
                {
                    Inventory.instance.Remove(currentStep.itemToBeDelivered);
                    CompleteStep();
                }
            }
        }
    }

    public void TalkedTo(NPCInteraction targetNPC)
    {
        if (currentStep.goalType == GoalType.Talk)
        {
            if (targetNPC.NPCName == currentStep.talkTo)
            {
                CompleteStep();
            }
        }
    }
}
