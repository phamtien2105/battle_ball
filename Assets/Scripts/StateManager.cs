using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum EnumMode { Attack, Defend };
    public float CostEnergy;
    public float InActiveTime;

    public float SpeedWithBall;

    public static bool isBallAvaiable;

    public bool isHoldBall;
    public static Vector3 BallPosition;

    private EnumMode MyEnumMode;

    public GameObject TargetGate;


    void Start()
    {

        MyEnumMode = EnumMode.Attack;
        StartCoroutine("onInactiveState", InActiveTime);
    }

    // Update is called once per frame
    void Update()
    {

        chaseBall();
        if (isHoldBall)
        {
            MoveToGate();
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

        if (isBallAvaiable && MyEnumMode == EnumMode.Attack
        && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("InactiveAnimation"))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                       BallPosition, 1.5f * Time.deltaTime);
            Vector3 rotationDestination = BallPosition;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6.0f);
        }
    }

    private void MoveToGate()
    {


        transform.position = Vector3.MoveTowards(transform.position,
                   TargetGate.transform.position, 1.5f * Time.deltaTime);
        Vector3 rotationDestination = TargetGate.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * SpeedWithBall);

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
        Debug.Log("hit ball");
        if (collider.gameObject.CompareTag("Ball"))
        {
            
            StateManager.isBallAvaiable = false;
            isHoldBall = true;
            collider.gameObject.transform.parent = gameObject.transform;


        }
    }
}
