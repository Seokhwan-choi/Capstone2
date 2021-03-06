﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    Rigidbody2D rb;
    GameObject Player;
    Vector2 PlayerDirection;
    float timeStamp;
    bool find;
    bool check = false;
    bool stop = false;

    float checkTime;

    [SerializeField] item power;

    private Inventory Iv;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Iv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (find)
        {
            PlayerDirection = -(transform.position - Player.transform.position).normalized;
            rb.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y) * 10f * (Time.time / timeStamp);
        }

        if (stop == false)
        {
            rb.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, 7.5f);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
        }

        

            checkTime += Time.deltaTime;
        if (checkTime > 0.5f)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            stop = true;
        }


        //if (!check)
        //{
        //    check = true;


        //}

        //checkTime += Time.deltaTime;
        //if(checkTime > 1f)
        //{
        //    gameObject.GetComponent<CircleCollider2D>().enabled = true;
        //    checkTime = 0;
        //}
    }

    public void AddItem()
    {
        if(!Iv.AddItem(power))
        {
            Debug.Log("아이템이 가득 찼습니다.");
        }
        else { }
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
