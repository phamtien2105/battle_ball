using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public Text CountText;
    private float timer;
    public GameObject Player;
    public GameObject Enermy;
    private bool isKeyPressed;
    // Start is called before the first frame update
    void Start()
    {
        CountText.text = "0s";
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // update timer text
        timer += Time.deltaTime;
        CountText.text = (int)timer + "s";

        //
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            int layer_mask = LayerMask.GetMask("playerLayer");
            int enermy_mask = LayerMask.GetMask("enermyLayer");

            if (Physics.Raycast(ray, out hit, 1000f, enermy_mask))
            {

                Instantiate(Enermy, new Vector3(hit.point.x, 0.5f, hit.point.z), Quaternion.identity);
            }
            else if (Physics.Raycast(ray, out hit, 1000f, layer_mask))
            {
                Instantiate(Player, new Vector3(hit.point.x, 0.5f, hit.point.z), Quaternion.identity);
            }


        }

    }
}
