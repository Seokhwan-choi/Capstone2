using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_Move : MonoBehaviour {

    public GameObject Frog;
    public Mon_Move2 FrogSc;
    public GameObject Boom;

    public Vector3 v;


    void Start()
    {
        Frog = GameObject.FindGameObjectWithTag("Monster");
        FrogSc = Frog.GetComponent<Mon_Move2>();

        if (FrogSc.dist.Equals("Left"))
        {
            v = Vector3.left;
        }
        else if (FrogSc.dist.Equals("Right"))
        {
            v = Vector3.right;
        }
    }

    void Update()
    {
        transform.position += v * 4 * Time.deltaTime;
        Destroy(this.gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.gameObject.tag.Equals("Player"))
        {
            Instantiate(Boom, new Vector3(transform.position.x, 
                transform.position.y), Quaternion.identity);
            Destroy(this.gameObject);
        }*/
    }
}
