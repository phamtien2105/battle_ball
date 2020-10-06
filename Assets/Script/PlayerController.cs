using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody body;
    public float speed;

    public GameObject pickup;

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
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);

            Ray  ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit,1000f))
            {

                Instantiate(pickup,new Vector3(hit.point.x,0.5f, hit.point.z),Quaternion.identity);
            }

            
        }
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
