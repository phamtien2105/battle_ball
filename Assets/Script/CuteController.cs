using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuteController : MonoBehaviour
{
    
    private bool isKeyPressed;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            this.isKeyPressed = true;
            
        }

        if (isKeyPressed)
        this.transform.Rotate(0,50*Time.deltaTime,0);            

        
    }
}
