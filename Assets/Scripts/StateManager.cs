using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum EnumPLayMode
    {
        Attacker,
        Defender
    };

    public enum EnumKind
    {
        Player,
        Enermy
    };

    public float CostEnergy;
    public float InActiveTime;

    public float SpeedWithBall;

    public EnumPLayMode MyEnumPLayMode;

    public EnumKind MyKind;

    private GameObject TargetGate;

    public bool isHaveBall;

    private GameObject OpponentFence;

    public float NormalSpeed;
    public float CarryingSpeed;

    public static GameObject BallObject;

    private bool needCatchAttacker;

    public float ReInactiveTime;

    public float ReturnSpeed;

    private Vector3 originPosition;

    public bool needToReturnOriginPosition;

    public GameObject nearestAttacker;

    public GameObject AttckerHaveBallObject;

    public bool returnOriginPositionAfterMove;


    void Start()
    {
        isHaveBall = false;
        StateManager.BallObject = GameObject.FindWithTag("Ball").gameObject;

        if (MyKind == EnumKind.Enermy)
        {
            TargetGate = GameObject.Find("PlayerGate").gameObject;
            OpponentFence = GameObject.Find("EnermyFence").gameObject;
        }
        else
        {
            TargetGate = GameObject.Find("EnermyGate").gameObject;
            OpponentFence = GameObject.Find("PlayerFence").gameObject;
        }

        //  MyEnumMode = EnumMode.Attack;
        StartCoroutine(onInactiveState(InActiveTime, gameObject));

        originPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("InactiveAnimation"))
        {
            if ( MyEnumPLayMode == EnumPLayMode.Attacker && !StateManager.BallObject.GetComponent<BallController>().isKeep)
                chaseBall();
            else if (isHaveBall && StateManager.BallObject.GetComponent<BallController>().isKeep)
            {
                // chase ball finish and move other gate
                moveToOpponentGate();
            } //ball be holded by same attacker-> go to other land
            else if (!isHaveBall && MyEnumPLayMode == EnumPLayMode.Attacker &&
                     StateManager.BallObject.GetComponent<BallController>().isKeep)
            {
                moveToOpponentLand();
            }

            if (needCatchAttacker)
                catchAttacker();
            
            if (returnOriginPositionAfterMove)
                    returnOriginPosition();;
        }

        if (gameObject.GetComponentInChildren<BallController>() == null)
        {
            isHaveBall = false;
        }else isHaveBall = true;
    }


    IEnumerator onInactiveState(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);

        if (MyEnumPLayMode == EnumPLayMode.Defender)
        {
            obj.GetComponent<Animator>().SetTrigger("isDefend");
        }
        else obj.GetComponent<Animator>().SetTrigger("isChase");
    }

    public void chaseBall()
    {
        if (!BallObject.GetComponent<BallController>().isKeep && MyEnumPLayMode == EnumPLayMode.Attacker)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                StateManager.BallObject.transform.position, NormalSpeed * Time.deltaTime);
            Vector3 rotationDestination = StateManager.BallObject.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * NormalSpeed);
        }
    }

    private void moveToOpponentGate()
    {
        //enable highlight
        gameObject.transform.Find("HighLight").gameObject.SetActive(true);
        //  Debug.Log("moveToOpponentGate" + TargetGate.transform.position);
        transform.position = Vector3.MoveTowards(transform.position,
            TargetGate.transform.position, CarryingSpeed * Time.deltaTime);
        Vector3 rotationDestination = TargetGate.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * CarryingSpeed);
    }


    private void moveToOpponentLand()
    {
        //rotate to target
        // Vector3 rotationDestination = TargetGate.transform.position;
        // Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * CarryingSpeed);
        Vector3 v3 = OpponentFence.gameObject.transform.position - transform.position;
        v3.y = 0.0f;
        
        transform.rotation = Quaternion.LookRotation(-v3);

        //  Debug.Log("moveToOpponentLand" + TargetGate.transform.position);
        if (MyKind == EnumKind.Enermy)
            transform.position -= new Vector3(0, 0, NormalSpeed * Time.deltaTime);
        else
            transform.position += new Vector3(0, 0, NormalSpeed * Time.deltaTime);
    }

    public void onExitInactive()
    {
        //enable detect area
        if (MyEnumPLayMode == EnumPLayMode.Defender)
            gameObject.transform.Find("DetectArea").gameObject.SetActive(true);
        else
        {
            gameObject.transform.Find("DetectArea").gameObject.SetActive(false);
            gameObject.transform.Find("RedTriangle").gameObject.SetActive(true);
        }
    }

    public void onEnterInactive()
    {
        gameObject.transform.Find("DetectArea").gameObject.SetActive(false);
        gameObject.transform.Find("HighLight").gameObject.SetActive(false);
        gameObject.transform.Find("RedTriangle").gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hit" + collider.gameObject.tag);
        if (MyEnumPLayMode == EnumPLayMode.Attacker && collider.gameObject.CompareTag("Ball"))
        {
            Debug.Log("hit ball");
            collider.gameObject.transform.parent = gameObject.transform;

            StateManager.BallObject.GetComponent<BallController>().isKeep = true;
            isHaveBall = true;
        }
        else if (isHaveBall && collider.gameObject.CompareTag("Gate"))
        {
            Debug.Log("win");
            //destroy attacker if hit to the fence
            GameManager.isBallHItGate = true;
           // Time.timeScale = 0;

            if (MyKind == EnumKind.Enermy)
            {
                GameManager.enermyScore++;
            }
            else GameManager.playerScore++;

            GameManager.NeedResetGame = true;
            //  Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Fence"))
        {
            //destroy attacker if hit to the fence
            if (MyKind == EnumKind.Enermy)
                GameManager.listEnermy.Remove(gameObject);
            else
                GameManager.listPlayer.Remove(gameObject);
            ;             gameObject.SetActive(false);
        }
        else if (isHaveBall && collider.gameObject.CompareTag("DetectArea"))
        {
            //destroy attacker if hit to the fence
            if (!collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0)
                .IsName("InactiveAnimation"))
            {
                collider.gameObject.GetComponentInParent<StateManager>().needCatchAttacker = true;
                collider.gameObject.GetComponentInParent<StateManager>().AttckerHaveBallObject = transform.gameObject;
            }
        }

        //enermy collision with defender       
        if (collider.gameObject.GetComponent<StateManager>() != null)
            if (collider.gameObject.GetComponent<StateManager>().MyEnumPLayMode != MyEnumPLayMode)
            {
                //2 obj must active
                if (collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0)
                        .IsName("InactiveAnimation")
                    || gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0)
                        .IsName("InactiveAnimation"))
                {
                    return;
                }


                // 1 in 2 have ball 

                if (isHaveBall || collider.gameObject.GetComponent<StateManager>().isHaveBall)
                {

                    isHaveBall = false;
                    collider.gameObject.GetComponent<StateManager>().isHaveBall = false;
                    
                    Debug.Log("defender hit attacker have ball");

                    StateManager.BallObject.transform.parent = null;
                    StateManager.BallObject.GetComponent<BallController>().isKeep = false;

                    if (gameObject.GetComponent<StateManager>().MyEnumPLayMode == EnumPLayMode.Defender)
                    {
                        gameObject.GetComponent<StateManager>().needToReturnOriginPosition = true;
                        gameObject.GetComponent<StateManager>().needCatchAttacker = false;
                        
                       
                    }
                    
                    if (gameObject.GetComponent<StateManager>().MyEnumPLayMode == EnumPLayMode.Attacker)
                    {
                        Debug.Log("pass ball");

                        gameObject.GetComponent<StateManager>().moveBalltoNext();
                        
                    }
                    
                    if (collider.gameObject.GetComponent<StateManager>().MyEnumPLayMode == EnumPLayMode.Defender)
                    {
                        collider.GetComponent<StateManager>().needToReturnOriginPosition = true;
                        collider.GetComponent<StateManager>().needCatchAttacker = false;
                    }
                    
                    if (collider.gameObject.GetComponent<StateManager>().MyEnumPLayMode == EnumPLayMode.Attacker)
                    {
                        Debug.Log("pass ball");
                        collider.gameObject.GetComponent<StateManager>().moveBalltoNext();
                    }
                    //reset ball info


                    gameObject.GetComponent<Animator>().SetTrigger("isInactive");
                    collider.gameObject.GetComponent<Animator>().SetTrigger("isInactive");
                    StartCoroutine(onInactiveState(gameObject.GetComponent<StateManager>().ReInactiveTime, gameObject));
                    StartCoroutine(onInactiveState(collider.gameObject.GetComponent<StateManager>().ReInactiveTime,
                        collider.gameObject));
                }
            }


        //if collection with detecter
    }

    private void OnTriggerStay(Collider collider)
    {
        if (isHaveBall && collider.gameObject.CompareTag("DetectArea"))
        {
            Debug.Log("tien debug need to catch attacker");
            if (!collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0)
                .IsName("InactiveAnimation"))
            {
                collider.gameObject.GetComponentInParent<StateManager>().needCatchAttacker = true;
                collider.gameObject.GetComponentInParent<StateManager>().AttckerHaveBallObject = transform.gameObject;

            }
        }
    }

    public void returnOriginPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            originPosition, ReturnSpeed * Time.deltaTime);
        Vector3 rotationDestination = originPosition;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ReturnSpeed);

        if (Vector3.Distance(originPosition, transform.position) < 0.01)
        {
            Debug.Log("step to move go returnOriginPosition");
            needToReturnOriginPosition = false;
        }
    }

    private void catchAttacker()
    {
        if (AttckerHaveBallObject.GetComponent<StateManager>().isHaveBall)
        {
            Debug.Log("defender go to catch emermy");
            transform.position = Vector3.MoveTowards(transform.position,
                AttckerHaveBallObject.transform.position, NormalSpeed * Time.deltaTime);
            Vector3 rotationDestination = AttckerHaveBallObject.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * NormalSpeed);
        }
        else
        {
            returnOriginPositionAfterMove = true;
        }
    }

    private void moveBalltoNext()
    {
        //find neast active attacker


        if (MyKind == EnumKind.Enermy)
            nearestAttacker = findNearestAttacker(gameObject, GameManager.listEnermy);
        else
            nearestAttacker = findNearestAttacker(gameObject, GameManager.listPlayer);
        // //

        if (nearestAttacker == null)
        {
            // defender win

            Debug.Log("tien debug defenfer win");
            //Time.timeScale = 0;
            if (MyKind == EnumKind.Enermy)
            {
                GameManager.enermyScore++;
            }
            else GameManager.playerScore++;

            GameManager.NeedResetGame = true;


        }
        else if (StateManager.BallObject != null)
        {
            // Debug.Log("list attacker " + GameManager.listAttacker.Count);
            // Debug.Log("nearestAttacker " + nearestAttacker.transform.position);
            //remove parent of ball
            isHaveBall = false;

            StateManager.BallObject.GetComponent<BallController>().desObject = nearestAttacker;
            StateManager.BallObject.GetComponent<BallController>().isKeep = false;
            StateManager.BallObject.GetComponent<BallController>().needPassBall = true;
        }
    }


    private GameObject findNearestAttacker(GameObject myAttacker, List<GameObject> ListObjs)
    {

        Debug.Log("find next to object");
        if (ListObjs.Count == 1)
            return null;

        GameObject nearestObject = null;
        float minDistance = 1000;

        List<GameObject> tempList = ListObjs;

        //remove itself
        tempList.Remove(gameObject);


        foreach (var attacker in tempList)
        {
            if (!attacker.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("InactiveAnimation"))
            {
                var distance = Vector3.Distance(myAttacker.transform.position, attacker.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = attacker;
                }
            }
        }

        return nearestObject;
    }
}