﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodUnitController : MonoBehaviour
{
    // Start is called before the first frame update

    private Image image;
    float current;

    void Start()
    {
        current = 0;
        image = this.GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = 0;
        image.color = tempColor;
        image.fillAmount = 1;

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            inCreaseEnergy(0.5f);
        }
    }


    public void inCreaseEnergy(float value)
    {


        current += value;
        var tempColor = image.color;

        image.color = new Color(tempColor.r , tempColor.g ,
         tempColor.b , current/EnergyController.CapPerUnit);
        image.fillAmount = current / EnergyController.CapPerUnit;

        Debug.Log("tang mau" + tempColor);
    }




}
