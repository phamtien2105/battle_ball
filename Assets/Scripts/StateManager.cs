using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum EnumMode { Attack, Defend };

    public enum EnumKind { Player, Enermy };
    public float CostEnergy;
    public float InActiveTime;

    public float SpeedWithBall;

    public EnumMode MyEnumMode;

    public EnumKind MyKind;

    private GameObject TargetGate;

    private bool isHaveBall;

    private GameObject OpponentFence;

    public float NormalSpeed;

    private GameObject BallObject;


    void Start()
    {

        BallObject = GameObject.FindWithTag("Ball").gameObject;

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
        StartCoroutine("onInactiveState", InActiveTime);

    }

    // Update is called once per frame
    void Update()
    {

        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("InactiveAnimation"))
        {
            if (!BallObject.GetComponent<BallController>().isKeep)
                chaseBall();
            else if (isHaveBall && BallObject.GetComponent<BallController>().isKeep)
            {
                // chase ball finish and move other gate
                moveToOpponentGate();
            }//ball be holded by same attacker-> go to other land
            else if (!isHaveBall && MyEnumMode == EnumMode.Attack && BallObject.GetComponent<BallController>().isKeep)
            {
                moveToOpponentLand();
            }

        }
    }


    IEnumerator onInactiveState(int time)
    {
        yield return new WaitForSeconds(time);

        if (MyEnumMode == EnumMode.Defend)
        {

            gameObject.GetComponent<Animator>().SetTrigger("isDefend");

        }
        else gameObject.GetComponent<Animator>().SetTrigger("isChase");
    }

    public void chaseBall()
    {

        if (!BallObject.GetComponent<BallController>().isKeep && MyEnumMode == EnumMode.Attack
        )
        {
            transform.position = Vector3.MoveTowards(transform.position,
                       BallObject.transform.position, 1.5f * Time.deltaTime);
            Vector3 rotationDestination = BallObject.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6.0f);
        }
    }

    private void moveToOpponentGate()
    {
        //  Debug.Log("moveToOpponentGate" + TargetGate.transform.position);
        transform.position = Vector3.MoveTowards(transform.position,
                   TargetGate.transform.position, 1.5f * Time.deltaTime);
        Vector3 rotationDestination = TargetGate.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * SpeedWithBall);


    }


    private void moveToOpponentLand()
    {

        //  Debug.Log("moveToOpponentLand" + TargetGate.transform.position);
        if (MyKind == EnumKind.Enermy)
            transform.position -= new Vector3(0, 0, NormalSpeed * Time.deltaTime);
        else
            transform.position += new Vector3(0, 0, NormalSpeed * Time.deltaTime);



    }

    public void onExitInactive()
    {
        //enable detect area
        if (MyEnumMode == EnumMode.Defend)
            gameObject.transform.Find("DetectArea").gameObject.SetActive(true);
        else
        {

            gameObject.transform.Find("DetectArea").gameObject.SetActive(false);
            gameObject.transform.Find("RedTriangle").gameObject.SetActive(true);
        }
    }



    public void setEnumMode(EnumMode mode)
    {
        this.MyEnumMode = mode;
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hit" + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Ball"))
        {
            Debug.Log("hit ball");
            collider.gameObject.transform.parent = gameObject.transform;

            BallObject.GetComponent<BallController>().isKeep = true;
            isHaveBall = true;


        }
        else if (collider.gameObject.CompareTag("Fence"))
        {

            //destroy attacker if hit to the fence
            Destroy(gameObject);
        }
    }
}
