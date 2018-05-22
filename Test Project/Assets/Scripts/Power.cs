using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    Rigidbody2D rb;
    GameObject Player;
    Vector2 PlayerDirection;
    float timeStamp;
    bool find;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (find)
        {
            PlayerDirection = -(transform.position - Player.transform.position).normalized;
            rb.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y) * 10f * (Time.time / timeStamp);
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
