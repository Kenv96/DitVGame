using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadButton;
    public Image black;
    private bool playing;
    public bool loadData;
    public void Update()
    {
        if (playing)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + 0.01f);
            if (black.color.a >= 0.99) SceneManager.LoadScene(1);
        }
        loadButton.interactable = (ES3.KeyExists("playerTransform"));
    }
    public void PlayGame()
    {
        playing = true;
        loadData = false;
    }

    public void LoadGame()
    {
        playing = true;
        loadData = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
