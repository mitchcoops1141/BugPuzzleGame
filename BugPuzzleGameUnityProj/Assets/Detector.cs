using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] Vector3 grid;
    [SerializeField] GameObject hex;
    [SerializeField] LayerMask hexMask;
    public void Detect()
    {
        var pos = new Vector3(
            Mathf.Round(transform.position.x / grid.x) * grid.x,
            Mathf.Round(transform.position.y / grid.y) * grid.y,
            Mathf.Round(transform.position.z / grid.z) * grid.z
            );

        transform.position = pos;


        Collider[] hexes = Physics.OverlapSphere(transform.position, 0.25f, hexMask, QueryTriggerInteraction.Collide);

        //if length is 0 (no objects)
        if (hexes.Length == 0)
        {
            //insantiate new hex
            Instantiate(hex, transform.position, transform.rotation);
        }
        //if length is 1 (inactive hex)
        else if (hexes.Length == 1)
        {
            if (hexes[0].tag == "Hexagon")
            {
                for (int i = 0; i < hexes[0].transform.childCount; i++)
                {
                    hexes[0].transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        //shoot a colliding sphere. if collide with a hex. remove hex.
        //if no collide. add hex
    }


    private void Start()
    {
        GameEvents.current.PlayerMoveFinsishedAction += Detect;
        Detect();
    }
}
