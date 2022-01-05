using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    bool leftMouse;
    // Update is called once per frame
    void Update()
    {
        //leftMouse = Input.GetMouseButtonDown(0);
    }

    internal bool LeftMouse() { return Input.GetMouseButtonDown(0); }
}
