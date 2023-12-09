using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class QuestLogUI : MonoBehaviour
{
    public QuestSlot[] positions;

    QuestManager questManager;

    private void Start()
    {
        questManager = QuestManager.GetInstance();
        questManager.onQuestChangedCallback += UpdateUI;
    }

    void UpdateUI()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (i < questManager.questLog.Count)
            {
                positions[i].AddQuest(questManager.questLog[i]);
            }
            else
            {
                positions[i].Clear();
            }
        }
    }
}
