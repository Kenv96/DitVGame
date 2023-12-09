using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public Button saveButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!PlayerMove.GetInstance().safe)
        {
            saveButton.interactable = false;
        }
        else
        {
            saveButton.interactable = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void menuButton()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
        SceneManager.LoadScene("Main Menu");
    }

    public void OnResumeClicked()
    {
       Resume();
    }

    public void OnSettingsClicked()
    {
        
    }

    public void OnSaveClicked()
    {
        SaveManager.GetInstance().Save();
    }
    
    public void OnLoadClicked()
    {
        LoadGame();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        if (ES3.KeyExists("playerTransform")) ES3.LoadInto("playerTransform", PlayerMove.GetInstance().gameObject.transform);
        Debug.Log("Loaded");
    }
}
