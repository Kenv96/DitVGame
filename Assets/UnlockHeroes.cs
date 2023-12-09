using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHeroes : MonoBehaviour
{
    public void UnlockArden(bool unlock)
    {
        PlayerMove.GetInstance().ardUnlocked = unlock;
    }

    public void UnlockFelicia(bool unlock)
    {
        PlayerMove.GetInstance().felUnlocked = unlock;
    }
}
