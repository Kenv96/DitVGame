using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DialogueEditor;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<QuestScriptObj> questLog = new List<QuestScriptObj>();
    public QuestLogUI logUI;

    public delegate void OnQuestChanged();
    public OnQuestChanged onQuestChangedCallback;

    public int activeQuests;

    public PlayerMove player;

    private static QuestManager instance;

    [Header("Sound")]
    public AudioSource sound;
    public AudioClip[] sounds;

    // Make sure this script is attached to a GameObject in the scene.
    // This method is called automatically when the script starts.
    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // If no instance exists, set this as the instance
        instance = this;

        // Make sure the GameObject doesn't get destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

    // Use this method to get the singleton instance
    public static QuestManager GetInstance()
    {
        return instance;
    }

    public void AcceptQuest(QuestScriptObj givenQuest)
    {
        givenQuest.StartQuest();
        activeQuests++;
        questLog.Add(givenQuest);
        //ConversationManager.Instance.SetBool(givenQuest.GetQuestCode().ToString() + "a", true);
        if (onQuestChangedCallback != null)
            onQuestChangedCallback.Invoke();
        //logUI.AddToLog(givenQuest);
        //logUI.UpdateDesc(givenQuest);
    }

    public void UpdateUI()
    {
        if (onQuestChangedCallback != null)
            onQuestChangedCallback.Invoke();
    }

    public void EnemyKilled(string tarEnemy)
    {
        foreach (QuestScriptObj quest in questLog)
        {
            if (quest.currentStep.goalType == GoalType.Kill)
            {
                quest.EnemyKilled(tarEnemy);
            }
        }
        return;
    }

    public bool CheckQuestObjective()
    {
        foreach(QuestScriptObj quest in questLog)
        {
            if (quest.currentStep.goalType == GoalType.Deliver || quest.currentStep.goalType == GoalType.Talk) 
            {

            }
        }
        return false;
    }


    public bool CheckQuestItem(Item item)
    {
        foreach (QuestScriptObj quest in questLog)
        {
            if (quest.currentStep.goalType == GoalType.Gather)
            {
                quest.ItemCollected(item); 
            }
        }
        return false;
    }

    public void SubmitQuest(NPCInteraction npc)
    {
        foreach(QuestScriptObj quest in questLog)
        {
            if (quest.currentStep.deliverTo == npc.NPCName || quest.currentStep.talkTo == npc.NPCName)
            {
                return;
            }
        }
    }

    public void FinishQuest(QuestScriptObj givenQuest)
    {
        givenQuest.isActive = false;
        givenQuest.isComplete = true;
        questLog.Remove(givenQuest);
        sound.PlayOneShot(sounds[0]);
        Inventory.instance.AddGold(givenQuest.goldReward);
        ConversationManager.Instance.SetBool(givenQuest.GetQuestCode().ToString() + "a", false);
        ConversationManager.Instance.SetBool(givenQuest.GetQuestCode().ToString() + "c", true);
        if (onQuestChangedCallback != null)
            onQuestChangedCallback.Invoke();
        activeQuests--;
        Debug.Log("Quest complete!");
    }
}
