using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public float CostEnergy;
    public float InActiveTime;

    public static bool isBallAvaiable;
    public static Vector3 BallPosition;
    

    void Start()
    {
        StartCoroutine("changeToDefendState", InActiveTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (isBallAvaiable)
            chaseBall();
    }


    IEnumerator changeToDefendState(int time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<Animator>().SetTrigger("isDefend");
        //enable detect area
        gameObject.transform.Find("DetectArea").gameObject.SetActive(true);
    }

    public void chaseBall()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                   BallPosition, 1.5f * Time.deltaTime);
        Vector3 rotationDestination = BallPosition;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6.0f);
    }
}
