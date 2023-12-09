using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCInteraction : MonoBehaviour, Interactable
{
    public string NPCName;

    public NPCConversation convo;
    public void Interact()
    {
        convo = GetComponent<NPCConversation>();
        ConversationManager.Instance.StartConversation(convo);
        ShowCursor();
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
