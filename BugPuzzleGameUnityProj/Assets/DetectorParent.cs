using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorParent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] internal Collider test;

    // Update is called once per frame
    void Update()
    { 
        transform.position = player.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hexagon")
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
