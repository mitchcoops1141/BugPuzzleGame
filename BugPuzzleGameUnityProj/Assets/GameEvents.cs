using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action PlayerMoveFinsishedAction;
    public void PlayerMoveFinished()
    {
        if (PlayerMoveFinsishedAction != null)
        {
            PlayerMoveFinsishedAction();
        }
    }

    public event Action BugsMovedFinishedAction;
    public void BugsMovedFinished()
    {
        if (BugsMovedFinishedAction != null)
        {
            BugsMovedFinishedAction();
        }
    }

    public Func<Transform> GetPlayerLocationAction;
    public void GetPlayerLocation(Func<Transform> returnTransform)
    {
        GetPlayerLocationAction = returnTransform;
    }

    public Transform ReturnPlayerLocation()
    {
        if (GetPlayerLocationAction != null)
        {
            return GetPlayerLocationAction();
        }

        return null;
    }
}
