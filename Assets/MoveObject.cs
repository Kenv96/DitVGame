using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject fadeToBlack;
    public GameObject player;
    public void Move(Vector3 moveTo, GameObject moveable)
    {
        moveable.transform.position = moveTo;
    }

    public void ActivateTP()
    {
        StartCoroutine(FadeTPToCave());
    }

    public IEnumerator FadeTPToCave()
    {
        fadeToBlack.GetComponent<Animator>().Play("ScreenFadeFast");
        yield return new WaitForSeconds(1f);
        MovePlayer();
    }

    public void MovePlayer()
    {
        player.transform.position = new Vector3(-502, 0, -399);
    }
}
