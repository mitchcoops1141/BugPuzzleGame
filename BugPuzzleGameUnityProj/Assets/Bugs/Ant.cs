using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

enum AntStates
{
    IDLE,
    MOVING,
    DYING,
}

public class Ant : MonoBehaviour
{
    [SerializeField] Transform nextHexChecker;
    [SerializeField] LayerMask hexMask;
    [SerializeField] float moveSpeed;

    [ShowInInspector] Hexagon hexagon;

    Stack<AntStates> currentState = new Stack<AntStates>();

    // Start is called before the first frame update
    void Start()
    {
        //get hex
        Collider[] hexes = Physics.OverlapSphere(transform.position, 0.1f, hexMask, QueryTriggerInteraction.Collide);
        hexagon = hexes[0].GetComponent<Hexagon>();
        hexagon.SetObjectOnCell(this.gameObject);


        //sub to event
        GameEvents.current.PlayerMoveFinsishedAction += PlayerFinished; 

        //start in idle
        PushState(AntStates.IDLE);
    }

    void PlayerFinished()
    {
        PushState(AntStates.MOVING);
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteState(currentState.Peek());
    }

    void PushState(AntStates state)
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

    void EnterState(AntStates state)
    {
        switch (state)
        {
            case AntStates.IDLE:
                break;
            case AntStates.MOVING:
                //look at the player
                transform.LookAt(GameEvents.current.GetPlayerLocationAction());

                //check sphere for AVAILABLE hexes
                Collider[] nextHexes = Physics.OverlapSphere(nextHexChecker.position, 0.25f, hexMask, QueryTriggerInteraction.Collide);
                //if 2 found. choose 50/50
                foreach(Collider nextHex in nextHexes)
                {
                    Hexagon hex = nextHex.GetComponent<Hexagon>();
                    if (hex.GetObjectOnCell() == null)
                    {
                        hexagon.SetObjectOnCell(null);
                        hexagon = hex;
                        hexagon.SetObjectOnCell(this.gameObject);
                    }
                    else if(hex.GetObjectOnCell().tag == "Player")
                    {
                        hexagon.SetObjectOnCell(null);
                        hexagon = hex;
                        hexagon.SetObjectOnCell(this.gameObject);
                        print("LOSE");
                    }
                        
                }

                transform.LookAt(hexagon.transform);
                break;
            case AntStates.DYING:
                break;
            default:
                break;
        }
    }

    void ExecuteState(AntStates state)
    {
        switch (state)
        {
            case AntStates.IDLE:
                break;
            case AntStates.MOVING:
                Moving();
                break;
            case AntStates.DYING:
                break;
            default:
                break;
        }

    }

    void ExitState(AntStates state)
    {
        switch (state)
        {
            case AntStates.IDLE:
                break;
            case AntStates.MOVING:
                //set the position to be perfect in middle of hex
                transform.position = hexagon.transform.position;

                hexagon.SetObjectOnCell(this.gameObject);

                GameEvents.current.BugsMovedFinished();
                break;
            case AntStates.DYING:
                break;
            default:
                break;
        }
    }


    //----STATE FUNCTIONS----
    void Idle()
    {

    }

    void Moving()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, hexagon.transform.position) < 0.025f)
        {
            PopState();
        }
    }
    

    void Die()
    {

    }
    private void OnDestroy()
    {
        GameEvents.current.PlayerMoveFinsishedAction -= PlayerFinished;
    }
}
