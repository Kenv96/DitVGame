using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelect : MonoBehaviour
{
    public int spellNum;
    public PlayerMove player;
    public Image icon;

    // Update is called once per frame
    void Update()
    {
        if (player.chosenSpell == spellNum)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
        }
    }
}
