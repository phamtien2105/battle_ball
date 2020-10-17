using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public List<GameObject> listEnergyItem;

    public static int CapPerUnit = 5;

    //tien fake to test quick
    private float valuePerSec = 0.5f;

    private float totalEnergy;

    public float current = 0;

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


    public float getCurrentEngery()
    {
        return this.current;
    }

    public void subEnergy(float value)
    {
        // for (int i = 0; current >= value && i < value / valuePerSec; i++)
        // {

        decreaseEnergy(value);
        // }
    }

    public void decreaseEnergy(float value)
    {
        int position = getItemPosition();

        this.current -= value;
        if (listEnergyItem[position].GetComponent<BloodUnitController>().current >= value)
        {
            listEnergyItem[position].GetComponent<BloodUnitController>().inCreaseEnergy(-1 * value);
        }
        else
        {
            float backupCurrentValue = listEnergyItem[position].GetComponent<BloodUnitController>().current;
            float remainValue = value - backupCurrentValue;
            if (remainValue > 0 && position != 0)
                decreaseEnergy(backupCurrentValue);
            listEnergyItem[position].GetComponent<BloodUnitController>().resetEmpty();
        }
    }

    int getItemPosition()
    {
        if (current <= 5)
        {
            return 0;
        }
        else if (current > 5 && current <= 10)
        {
            return 1;
        }
        else if (current > 10 & current <= 15)
        {
            return 2;
        }
        else if (current > 15 & current <= 20)
        {
            return 3;
        }
        else if (current > 20 & current <= 25)
        {
            return 4;
        }
        else if (current > 25)
        {
            return 5;
        }

        return 0;
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
        else if (current > 5 && current <= 10)
        {
            listEnergyItem[1].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 10 & current <= 15)
        {
            listEnergyItem[2].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 15 & current <= 20)
        {
            listEnergyItem[3].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 20 & current <= 25)
        {
            listEnergyItem[4].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }
        else if (current > 25 & current <= 30)
        {
            listEnergyItem[5].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }

        isRunning = false;
    }
}