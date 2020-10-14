﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{

    public List<GameObject> listEnergyItem;

    public static int CapPerUnit = 5;

    private float valuePerSec = 0.5f;

    private float totalEnergy;

    private float current = 0;

    bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        totalEnergy = listEnergyItem.Count * 5;

    }

    // Update is called once per frame
    void Update()
    {
       if (!isRunning)
           StartCoroutine("inCreaseEnergy", 1.0f);
    }

    IEnumerator inCreaseEnergy()
    {
        isRunning = true;
        yield return new WaitForSeconds(1);
        current += valuePerSec;

        if (current <= 5)
        {
            listEnergyItem[0].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 5 && current < 10)
        {
            listEnergyItem[1].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 10 & current < 15)
        {
            listEnergyItem[2].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 15 & current < 20)
        {
            listEnergyItem[3].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 20 & current < 25)
        {
            listEnergyItem[4].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 25 & current < 30)
        {
            listEnergyItem[5].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }

        isRunning = false;
    }

}