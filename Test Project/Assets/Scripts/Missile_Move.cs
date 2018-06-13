using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_Move : MonoBehaviour {

    public GameObject Frog;
    public Mon_Move2 FrogSc;


    void Start()
    {
        Frog = GameObject.FindGameObjectWithTag("Monster");
        FrogSc = Frog.GetComponent<Mon_Move2>();
    }

    void Update()
    {
        transform.Translate(Vector3.left * 4 * Time.deltaTime);
        Destroy(this.gameObject, 10);
    }
}
