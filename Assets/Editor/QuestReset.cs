using UnityEditor;
using UnityEngine;

public class QuestReset : MonoBehaviour
{
    [MenuItem("Questing/Reset All Quests")]
    private static void ResetAll()
    {
        QuestScriptObj[] scriptableObjects = Resources.FindObjectsOfTypeAll<QuestScriptObj>();

        foreach (QuestScriptObj scriptableObject in scriptableObjects)
        {
            scriptableObject.ResetQuest();
            EditorUtility.SetDirty(scriptableObject);
        }
    }
}
