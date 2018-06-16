using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File_Missile : MonoBehaviour {

    public int damage = 20;
    //총알의 속도
    public float speed = 30.0f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * speed);
    }
}
