using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public List<GameObject> listEnergyItem;

    public static int CapPerUnit = 5;

    //tien fake to test quick
    private float valuePerSec = 0.5f;

    public float current = 0;

    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
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
        decreaseEnergy(value);
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
            listEnergyItem[position].GetComponent<BloodUnitController>().resetEmpty();


            if (remainValue > 0 && position != 0)
                decreaseEnergy(remainValue);
        }

        for (int i = position + 1; i < listEnergyItem.Count; i++)
            listEnergyItem[i].GetComponent<BloodUnitController>().resetEmpty();
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
        else if (current > 25 & current <= 30)
        {
            return 5;
        }

        return 0;
    }

    IEnumerator inCreaseEnergy()
    {
        isRunning = true;
        yield return new WaitForSeconds(1);


        int position = getItemPosition();
        //
        float currentItemValue = listEnergyItem[position].GetComponent<BloodUnitController>().current;

        if (currentItemValue + valuePerSec > CapPerUnit)
        {
            float needFillValue = CapPerUnit - currentItemValue;
            listEnergyItem[position].GetComponent<BloodUnitController>().inCreaseEnergy(needFillValue);
            if (position + 1 <= listEnergyItem.Count)
                listEnergyItem[position + 1].GetComponent<BloodUnitController>()
                    .inCreaseEnergy(valuePerSec - needFillValue);
        }
        else
        {
            listEnergyItem[position].GetComponent<BloodUnitController>().inCreaseEnergy(valuePerSec);
        }

        current += valuePerSec;

        isRunning = false;
    }

    public void Reset()
    {
        current = 0;
        foreach (var item in listEnergyItem)
        {
            item.GetComponent<BloodUnitController>().resetEmpty();
        }
    }
}