using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelectedSpell : MonoBehaviour
{
    public Image spellIMG;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        spellIMG= GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMove.GetInstance().currentChar == PlayerMove.Character.Lilline)
        {
            if(PlayerMove.GetInstance().chosenSpell == 0)
            {
                spellIMG.sprite = sprites[0];
            }
            else
            {
                spellIMG.sprite = sprites[1];
            }
        }
        else if (PlayerMove.GetInstance().currentChar == PlayerMove.Character.Arden)
        {
            if (PlayerMove.GetInstance().chosenSpell == 0)
            {
                spellIMG.sprite = sprites[2];
            }
            else
            {
                spellIMG.sprite = sprites[3];
            }
        }
        else
        {
            if (PlayerMove.GetInstance().chosenSpell == 0)
            {
                spellIMG.sprite = sprites[4];
            }
            else
            {
                spellIMG.sprite = sprites[5];
            }
        }
    }
}
