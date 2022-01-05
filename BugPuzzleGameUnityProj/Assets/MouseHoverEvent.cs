using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverEvent : MonoBehaviour
{
    public void Hover(string str)
    {
        if (str == "Right")
            GameEvents.current.MouseHoverBorder(Vector3.right);
        else if (str == "Left")
            GameEvents.current.MouseHoverBorder(Vector3.left);
        else if (str == "Up")
            GameEvents.current.MouseHoverBorder(Vector3.forward);
        else if (str == "Down")
            GameEvents.current.MouseHoverBorder(Vector3.back);
        else if (str == "TopLeft")
            GameEvents.current.MouseHoverBorder(Vector3.left + Vector3.forward);
        else if (str == "TopRight")
            GameEvents.current.MouseHoverBorder(Vector3.right + Vector3.forward);
        else if (str == "BottomLeft")
            GameEvents.current.MouseHoverBorder(Vector3.left + Vector3.back);
        else if (str == "BottomRight")
            GameEvents.current.MouseHoverBorder(Vector3.right + Vector3.back);
    }

    public void ExitHover()
    {
        GameEvents.current.MouseHoverBorder(Vector3.zero);
    }
}
