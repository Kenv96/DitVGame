using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConvoTrigger : MonoBehaviour
{
    public NPCConversation convo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            convo = GetComponent<NPCConversation>();
            ConversationManager.Instance.StartConversation(convo);
            ShowCursor();
        }
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
