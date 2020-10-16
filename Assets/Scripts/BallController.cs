using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public bool isBallAvaiable;
    //indicate this object keep the ball 
    public bool isKeep;

    private float ballSpeed;


    public bool needPassBall;

    public GameObject desObject;


    // Start is called before the first frame update
    void Start()
    {
        ballSpeed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (needPassBall)
        {
            isKeep = false;
            Debug.Log("ball is moving");
            moveBallToAttacker(desObject);
            if (transform.GetComponentInParent<StateManager>() != null)
                transform.GetComponentInParent<StateManager>().isHaveBall = false;
        }
    }

    public void moveBallToAttacker(GameObject des)
    {
        Debug.Log("moveBallToAttacker");
        transform.position = Vector3.MoveTowards(transform.position,
            des.transform.position, ballSpeed * Time.deltaTime);
        Vector3 rotationDestination = des.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(des.transform.rotation, targetRotation, Time.deltaTime * ballSpeed);

        if (Vector3.Distance(transform.position, des.transform.position) < 0.01)
        {
            Debug.Log("step to move go returnOriginPosition");
            needPassBall = false;
            isKeep = true;
        }
    }
}