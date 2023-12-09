using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    public TMP_Text questTitle;
    public TMP_Text questDesc;

    QuestScriptObj quest;

    public void AddQuest(QuestScriptObj newQuest)
    {
        quest = newQuest;

        questTitle.text = quest.title;
        questDesc.text = quest.currentStep.objDesc;
    }

    public void Clear()
    {
        quest = null;

        questTitle.text = "";
        questDesc.text = "";
    }
}
