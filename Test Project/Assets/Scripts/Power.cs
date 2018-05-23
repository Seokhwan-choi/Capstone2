using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    Rigidbody2D rb;
    GameObject Player;
    Vector2 PlayerDirection;
    float timeStamp;
    bool find;
    bool check = false;

    float checkTime;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (find)
        {
            PlayerDirection = -(transform.position - Player.transform.position).normalized;
            rb.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y) * 10f * (Time.time / timeStamp);
        }

        if (!check)
        {
            check = true;
            rb.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, 7.5f);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

        }

        checkTime += Time.deltaTime;
        if(checkTime > 1f)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            checkTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Magnet"))
        {
            timeStamp = Time.time;
            Player = GameObject.Find("Player");
            find = true;
        }
    }
}
