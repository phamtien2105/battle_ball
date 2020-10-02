using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody body;
    public float speed;
    void Start()
    {
       body  = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moment= new Vector3(moveHorizontal,0.0f,moveVertical);
        body.AddForce(moment*speed);
      

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
