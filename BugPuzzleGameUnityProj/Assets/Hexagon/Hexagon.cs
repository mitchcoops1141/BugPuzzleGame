using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Hexagon : MonoBehaviour
{
    [SerializeField] GameObject ant;

    [Header("Mesh and Materials")]
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material highlightedMat;
    [SerializeField] Material defaultMat;
    bool isHighlighted = false;

    [Header("Layer Mask")]
    [SerializeField] LayerMask hexLayerMask;



    [ShowInInspector] private GameObject objectOnCell;
    [ShowInInspector] private bool walkable = true;

    // Start is called before the first frame update
    void Start()
    {
        int x = Random.Range(1, 51);
        if (x > 43)
        {
            if (objectOnCell == null)
                Instantiate(ant, transform.position, transform.rotation);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWalkable(bool canWalk)
    {
        walkable = canWalk;
    }


    public void HighlightHex()
    {
        mesh.material = highlightedMat;
        isHighlighted = true;
    }
    public void DefaultHex()
    {
        mesh.material = defaultMat;
        isHighlighted = false;
    }
    
    public void HighlightSurroundHexes(bool highlight)
    {
        Collider[] surroundingHexes = Physics.OverlapSphere(transform.position, 1f, hexLayerMask, QueryTriggerInteraction.Collide);

        if (highlight)
        {
            foreach (Collider hex in surroundingHexes)
            {
                if (hex.transform != this.transform)
                    hex.GetComponentInParent<Hexagon>().HighlightHex();
            }
        }
        else
        {
            foreach (Collider hex in surroundingHexes)
            {
                if (hex.transform != this.transform)
                    hex.GetComponentInParent<Hexagon>().DefaultHex();
            }
        }

    }

    public bool IsHexHighlighted()
    {
        return isHighlighted;
    }

    public GameObject GetObjectOnCell() { return objectOnCell; }
    public void SetObjectOnCell(GameObject obj) { objectOnCell = obj; }
}
