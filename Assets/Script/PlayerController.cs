using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody body;
    public float speed;

    private int count;

    public Text TextCount;
    void Start()
    {
       body  = this.GetComponent<Rigidbody>();
       count = 0;
       TextCount.text ="Count: " + count;
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

  void OnTriggerEnter(Collider collider)
  {
      if (collider.gameObject.CompareTag("pickup"))
      {

            // Destroy(collider.gameObject);
            collider.gameObject.SetActive(false);
            count++;
            TextCount.text ="Count: " + count;
      }

      
  }

}
