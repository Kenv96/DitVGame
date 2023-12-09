using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueParent;
    public TMP_Text NPCNameText;
    public TMP_Text dialogueText;
    public TMP_Text playerNameText;
    public Image NPCSprite;
    public Image charSprite;

    [Header("Cameras")]
    public GameObject freeCam;
    public GameObject diaCam;


    private Queue<string> sentences;
    private Queue<int> speakers;

    private static DialogueManager instance;

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
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogueParent.SetActive(false);
        sentences = new Queue<string>();
        speakers = new Queue<int>();
    }

    public void StartDialogue(Dialogue dialogue, Sprite NPCsprite)
    {
        diaCam.SetActive(true);
        freeCam.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        dialogueParent.SetActive(true);
        NPCNameText.text = dialogue.name;
        playerNameText.text = "Lilline";
        NPCSprite.sprite = NPCsprite;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        foreach(int speaker in dialogue.speakers)
        {
            speakers.Enqueue(speaker);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        int speaker = speakers.Dequeue();
        //speaker 0 is player, text aligned right and sprite is full color. NPC sprite darkened)
        if (speaker == 0)
        {
            dialogueText.alignment = TextAlignmentOptions.TopRight;
            NPCSprite.color = NPCSprite.color * 0.5f;
            charSprite.color = Color.white;
        } 
        else
        {
            dialogueText.alignment = TextAlignmentOptions.TopLeft;
            charSprite.color = charSprite.color * 0.5f;
            NPCSprite.color = Color.white;
        }
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        freeCam.SetActive(true);
        diaCam.SetActive(false);
        dialogueParent.SetActive(false);
        Cursor.visible= false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("end of convo");
    }
}
