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
    public GameObject pickupEnermy;

    private int count;

    public Text TextCount;

    private Queue<GameObject> queuePicker;

    private GameObject targetPicker;
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        count = 0;
        TextCount.text = "Count: " + count;
        queuePicker = new Queue<GameObject>();

    }

    void FixedUpdate()
    {


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moment = new Vector3(moveHorizontal, 0.0f, moveVertical);
        body.AddForce(moment * speed);

        if (targetPicker == null && queuePicker.Count != 0)
            targetPicker = queuePicker.Dequeue();


    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            GameObject picker;
            if (Physics.Raycast(ray, out hit, 1000f))
            {


                int randomColorLayerIndex = Random.Range(1, 4);

                if (randomColorLayerIndex != 1)
                {
                    picker = Instantiate(pickup, new Vector3(hit.point.x, 0.5f, hit.point.z), Quaternion.identity);
                    picker.GetComponent<Animator>().SetLayerWeight(randomColorLayerIndex, 1);
                }
                else
                {
                    picker = Instantiate(pickupEnermy, new Vector3(hit.point.x,0.5f, hit.point.z), Quaternion.identity);
                    picker.tag="enermy";
                }

                queuePicker.Enqueue(picker);
            }
        }



        if (targetPicker != null && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("die"))
        {
            GetComponent<Animator>().SetBool("isWalk", true);
            transform.position = Vector3.MoveTowards(transform.position,
            targetPicker.transform.position, 1.5f * Time.deltaTime);
            Vector3 rotationDestination = targetPicker.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDestination - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6.0f);
        }
        else if (queuePicker.Count == 0)
            GetComponent<Animator>().SetBool("isWalk", false);

        // }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("pickup"))
        {

            collider.gameObject.GetComponent<Animator>().SetBool("isDie", true);
            count++;
            TextCount.text = "Count: " + count;
        }
        else if (collider.gameObject.CompareTag("enermy"))
        {
            Debug.Log("tien debug collision vs");
            collider.gameObject.GetComponent<Animator>().SetBool("isDie", true);

            this.gameObject.GetComponent<Animator>().SetBool("isDie", true);
            StartCoroutine(diePlayer(9));


            // this.gameObject.SetActive(true);
        }

    }

    public void onFireParticleCollision()
    {

    }

    private IEnumerator diePlayer(int second)
    {
        yield return new WaitForSeconds(second);
        Debug.Log("tien debug deactive player");
        this.gameObject.GetComponent<Animator>().SetBool("isDie", false);
    }



}
