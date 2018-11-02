using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 14.5f;
    public float lifetime = 1.5f;    
    Player player;
    Vector3 dir;

    float life = 0.0f;

    void onBecameinvisible()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        player = GameObject.Find("Player_gun").GetComponent<Player>();
        if (!player.spriteRenderer.flipX)
        {
            dir = Vector3.right;
        }
        else if (player.spriteRenderer.flipX)
        {
            dir = Vector3.left;
        }
    }
    // Update is called once per frame
    void Update () {
        transform.position += dir * speed * Time.deltaTime;
        if ( life > lifetime)
        {
            Destroy(this.gameObject);
            life = 0;
        }
        life += Time.deltaTime;
        
	}
    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            Destroy(this.gameObject);
        }
    }*/
}
