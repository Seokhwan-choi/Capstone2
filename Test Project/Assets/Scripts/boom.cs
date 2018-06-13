using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boom : MonoBehaviour
{

    public float lifetime = 0.5f;
    float checktime = 0.0f;


    // Update is called once per frame
    void Update()
    {
        if (checktime > lifetime)
        {
            Destroy(this.gameObject);
        }
        checktime += Time.deltaTime;
    }
}