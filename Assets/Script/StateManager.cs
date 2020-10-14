using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Text CountText;
    private float timer;
    void Start()
    {
        Debug.Log("change alpha");
        CountText.text = "0s";
        timer = +Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // update timer text
        timer = +Time.deltaTime;
        CountText.text = (int)timer + "s";
    }
}
