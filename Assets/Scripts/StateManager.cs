using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public float CostEnergy;
    public float InActiveTime;

    void Start()
    {
        StartCoroutine("changeToDefendState", InActiveTime);

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator changeToDefendState(int time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<Animator>().SetTrigger("isDefend");
        //enable detect area
        gameObject.transform.Find("DetectArea").gameObject.SetActive(true);
    }
}
