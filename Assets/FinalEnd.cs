using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEnd : MonoBehaviour
{
    public GameObject menuButton;

    public void EndGame()
    {
        Invoke(nameof(BackToMenu), 20f);
    }

    public void BackToMenu()
    {
        menuButton.SetActive(true);
    }
}
