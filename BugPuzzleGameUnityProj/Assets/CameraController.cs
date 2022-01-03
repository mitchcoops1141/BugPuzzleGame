using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float cameraSpeed;


    // Update is called once per frame
    void Update()
    {
        Vector3 lerpToPos = player.position;
        lerpToPos.y = transform.position.y;

        //lerp POS
        transform.position = Vector3.Lerp(transform.position, lerpToPos, Time.deltaTime * cameraSpeed);
    }
}
