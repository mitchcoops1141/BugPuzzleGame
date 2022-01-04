using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

enum States
{
    IDLE,
    SHOOTING,
    SELECTION,
    MOVING
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] internal InputComponent input;
    [SerializeField] LayerMask hexMask;
    [SerializeField] float moveSpeed;

    [ShowInInspector] Hexagon hexagon;

    Stack<States> currentState = new Stack<States>();

    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        //get hex
        Collider[] hexes = Physics.OverlapSphere(transform.position, 0.1f, hexMask, QueryTriggerInteraction.Collide);
        hexagon = hexes[0].GetComponent<Hexagon>();
        hexagon.SetObjectOnCell(this.gameObject);

        GameEvents.current.BugsMovedFinishedAction += BugsFinished;
        GameEvents.current.GetPlayerLocationAction += ReturnLocation;

        //start selection state
        PushState(States.SHOOTING);
    }

    bool popState = true;
    void BugsFinished()
    {
        if (popState)
            StartCoroutine("PopStateIE");
    }

    IEnumerator PopStateIE()
    {
        popState = false;

        yield return new WaitForSeconds(0.4f);

        currentState.Clear();
        PushState(States.SHOOTING);

        popState = true;
    }

    Transform ReturnLocation() { return transform; }

    // Update is called once per frame
    void Update()
    {
        ExecuteState(currentState.Peek());
    }

    void PushState(States state)
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

    void EnterState(States state)
    {
        switch (state)
        {
            case States.IDLE:
                break;
            case States.SHOOTING:
                break;
            case States.SELECTION:
                hexagon.HighlightSurroundHexes(true);
                hexagon.SetObjectOnCell(null);
                break;
            case States.MOVING:
                transform.LookAt(hexagon.transform);
                break;
            default:
                break;
        }
    }

    void ExecuteState(States state)
    {
        switch (state)
        {
            case States.IDLE:
                Idle();
                break;
            case States.SHOOTING:
                Shoot();
                break;
            case States.SELECTION:
                Selection();
                break;
            case States.MOVING:
                Move();
                break;
            default:
                break;
        }

    }

    void ExitState(States state)
    {
        switch (state)
        {
            case States.IDLE:
                break;
            case States.SHOOTING:
                break;
            case States.SELECTION:
                break;
            case States.MOVING:
                //set the position to be perfect in middle of hex
                transform.position = hexagon.transform.position;
                hexagon.SetObjectOnCell(this.gameObject);
                GameEvents.current.PlayerMoveFinished();
                break;
            default:
                break;
        }
    }

    //---------STATES--------
    void Idle()
    {

    }

    void Shoot()
    {
        if (input.LeftMouse())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, hexMask))
            {
                if (hitInfo.transform.GetComponent<Hexagon>().GetObjectOnCell())
                {
                    if (hitInfo.transform.GetComponent<Hexagon>().GetObjectOnCell().tag == "Bug")
                    {
                        Hexagon hex = hitInfo.transform.GetComponent<Hexagon>();

                        Destroy(hex.GetObjectOnCell());
                        hex.SetObjectOnCell(null);

                        PushState(States.SELECTION);

                        score++;

                        print("Score: " + score);
                    }

                }
            }

        }
    }

    void Selection()
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
                    PushState(States.MOVING);

                    if (hitInfo.transform.GetComponent<Hexagon>().GetObjectOnCell())
                    {
                        Destroy(hexagon.GetObjectOnCell());
                        score++;

                        print("Score: " + score);
                    }

                }
            }

        }
    }

    void Move()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, hexagon.transform.position) < 0.025f)
        {
            PushState(States.IDLE);
        }
    }
}
