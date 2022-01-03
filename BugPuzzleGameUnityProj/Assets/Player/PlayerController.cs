using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

enum State
{
    SELECTION,
    MOVING
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] internal InputComponent input;
    [SerializeField] LayerMask hexMask;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] detectors;
    [SerializeField] GameObject hex;
    [SerializeField] Vector3 grid;

    [ShowInInspector] Hexagon hexagon;

    Stack<State> currentState = new Stack<State>();
    
    // Start is called before the first frame update
    void Start()
    {
        Collider[] hexes = Physics.OverlapSphere(transform.position, 0.1f, hexMask, QueryTriggerInteraction.Collide);

        hexagon = hexes[0].GetComponentInParent<Hexagon>();

        PushState(State.SELECTION);
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteState(currentState.Peek());
    }

    void PushState(State state)
    {
        if (currentState.Count > 0)
            //exit the current state
            ExitState(currentState.Peek());

        //push the new state
        currentState.Push(state);

        //enter the new state
        EnterState(currentState.Peek());
    }

    void PopState()
    {
        ExitState(currentState.Peek());

        currentState.Pop();

        EnterState(currentState.Peek());
    }

    void EnterState(State state)
    {
        switch (state)
        {
            case State.SELECTION:
                hexagon.HighlightSurroundHexes(true);
                break;
            case State.MOVING:
                transform.LookAt(hexagon.transform);
                break;
            default:
                break;
        }
    }

    void ExecuteState(State state)
    {
        switch (state)
        {
            case State.SELECTION:
                SelectionState();
                break;
            case State.MOVING:
                MovingState();
                break;
            default:
                break;
        }

    }

    void ExitState(State state)
    {
        switch (state)
        {
            case State.SELECTION:
                break;
            case State.MOVING:
                //set the position to be perfect in middle of hex
                transform.position = hexagon.transform.position;

                //add new hexes
                foreach(Transform detector in detectors)
                {
                    var pos = new Vector3(
                        Mathf.Round(detector.transform.position.x / grid.x) * grid.x,
                        Mathf.Round(detector.transform.position.y / grid.y) * grid.y,
                        Mathf.Round(detector.transform.position.z / grid.z) * grid.z
                        );
                    detector.position = pos;

                    Collider[] hexes = Physics.OverlapSphere(detector.position, 0.25f, hexMask, QueryTriggerInteraction.Collide);
                    if (hexes.Length == 0)
                    {
                        //insantiate new hex
                        Instantiate(hex, detector.position, detector.rotation);
                    }
                }
                break;
            default:
                break;
        }
    }

    //---------STATES--------
    void SelectionState()
    {
        if (input.LeftMouse())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, hexMask))
            {
                if (hitInfo.transform.GetComponent<Hexagon>().IsHexHighlighted())
                {
                    //unlight the hexes
                    hexagon.HighlightSurroundHexes(false);
                    //set selected hex to clicked on Hex
                    hexagon = hitInfo.transform.GetComponent<Hexagon>();
                    //push the moving state
                    PushState(State.MOVING);
                }
            }

        }
    }

    void MovingState()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, hexagon.transform.position) < 0.025f)
        {
            PopState();
        }
    }
}
