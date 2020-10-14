using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAreaController : MonoBehaviour
{
    // Start is called before the first frame update

    //public GameObject Plane;

    void Start()
    {
        GameObject plane = GameObject.Find("playerArea");

        this.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        float radius = plane.GetComponent<Renderer>().bounds.size.x * 0.35f;
        this.transform.localScale += new Vector3(radius, 0, radius);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
