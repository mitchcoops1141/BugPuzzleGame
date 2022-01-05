using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 moveToPos = Vector3.zero;

    private void Start()
    {
        moveToPos = transform.position;
        GameEvents.current.MouseHoverBorderAction += UpdateMovePosition;
    }

    void UpdateMovePosition(Vector3 pos)
    {
        moveToPos = pos;
    }

    private void Update()
    {
        print(moveToPos);
        moveToPos.y = 0;

        transform.Translate(moveToPos * speed * Time.deltaTime);
    }
}
