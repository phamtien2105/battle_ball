using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodUnitController : MonoBehaviour
{
    // Start is called before the first frame update

    private Image image;
    private float Max = 5;
    private float value = 0.5f;
    float current;

    void Start()
    {
        current = 0;
        image = this.GetComponent<Image>();
        StartCoroutine("inCreaseEnergy");
    }

    // Update is called once per frame
    void Update()
    {



    }

    IEnumerator inCreaseEnergy()
    {

        float normalizedTime = 0f;
        //filled energy in 20s
        float duration = Max / value;
        while (normalizedTime <= 1f)
        {

            normalizedTime += Time.deltaTime / duration;

            var tempColor = image.color;
            tempColor.a = (byte)((normalizedTime * 255));
            image.color = tempColor;
            image.fillAmount = normalizedTime;
            yield return null;
        }

        // Debug.Log("tien debug deactive player");
        // if (current < Max)
        // {
        //     yield return new WaitForSeconds(1.0f);
        //     current += 0.5f;
        //     this.GetComponent<Image>().color = new Color32(255, 0, 0, (byte)((current * 255) / Max));
        //     this.GetComponent<Image>().fillAmount = current / Max;


        // }
        // else current = Max;

    }


}
