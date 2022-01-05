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

    public event Action<Vector3> MouseHoverBorderAction;
    public void MouseHoverBorder(Vector3 direction)
    {
        if (MouseHoverBorderAction != null)
        {
            MouseHoverBorderAction(direction);
        }
    }
}
