using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Hexagon : MonoBehaviour
{
    [Header("Mesh and Materials")]
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material highlightedMat;
    [SerializeField] Material defaultMat;
    bool isHighlighted = false;

    [Header("Layer Mask")]
    [SerializeField] LayerMask hexLayerMask;

    [ShowInInspector] private GameObject objectOnCell;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
                if (hex.transform.parent != this.transform)
                    hex.GetComponentInParent<Hexagon>().HighlightHex();
            }
        }
        else
        {
            foreach (Collider hex in surroundingHexes)
            {
                if (hex.transform.parent != this.transform)
                    hex.GetComponentInParent<Hexagon>().DefaultHex();
            }
        }

    }

    public bool IsHexHighlighted()
    {
        return isHighlighted;
    }

    public GameObject GetObjectOnCell() { return objectOnCell; }
}
