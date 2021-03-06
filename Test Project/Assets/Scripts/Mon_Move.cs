﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Move : MonoBehaviour {

    Animator animator;
    Animation ani;
    Rigidbody2D rigid;
    Vector3 movement;
    GameObject traceTarget;

    float ch = 0.0f;

    public GameObject Power;

    int movementFlag = 0;
    public float movePower = 1f;
    public int creatureType;
    public int M_Health = 3;
    bool isTracing = false;
    bool isDying = false;
    bool isHiting = false;

    float Death_time = 0.0f;

    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        ani = gameObject.GetComponent<Animation>();
        StartCoroutine("ChangeMovement");  
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);
        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        yield return new WaitForSeconds(3f);        
        StartCoroutine("ChangeMovement");
    }

    void FixedUpdate()
    {
        
    }

	void Update () {
        Move();
        //MoveLimit();        
    }

    
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Rignt";
        }
        else
        {
            if (movementFlag == 1)
            {
                dist = "Left";
            }
            else if (movementFlag == 2)
            {
                dist = "Right";
            }
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left; //(-1,0,0)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(dist == "Right")
        {
            moveVelocity = Vector3.right; //(-1,0,0)
            transform.localScale = new Vector3(1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    void MoveLimit()
    {
        Vector2 temp;
        temp.x = Mathf.Clamp(transform.position.x, -1, 1);
        temp.y = Mathf.Clamp(transform.position.y, -2f, 2f);
        transform.position = temp;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        Vector2 mon = new Vector2(0,0);
        if(other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
        if(other.gameObject.tag == "Attack_check")
        {
            M_Health--;
            Sound1.instance.PlaySound();
            isHiting = true;
            animator.SetBool("isHiting", true);
            animator.SetTrigger("isHiting");
            movePower = 0f;
            mon = new Vector2(4f, 0);
            rigid.AddForce(mon, ForceMode2D.Impulse);
            movePower = 1f;
            if (M_Health < 0)
            {
                isDying = true;
                Sound2.instance.PlaySound();
                animator.SetBool("Death", true);
                animator.SetTrigger("Dying");
                while (true)
                {
                    
                    Death_time += Time.deltaTime;
                    if (Death_time >= 1.0f)
                    {
                        Instantiate(Power, transform.position, Quaternion.identity);
                        Destroy(this.gameObject, 1f);
                        break;
                   }
                }
            }
        }

        if (other.gameObject.tag == "Attack_check_left")
        {
            M_Health--;
            isHiting = true;
            animator.SetBool("isHiting", true);
            animator.SetTrigger("isHiting");

            Sound1.instance.PlaySound();
            movePower = 0f;
            mon = new Vector2(-4f, 0);
            rigid.AddForce(mon, ForceMode2D.Impulse);
            movePower = 1f;
            if (M_Health < 0)
            {
                isDying = true;
                animator.SetBool("Death", true);
                animator.SetTrigger("Dying");
                Sound2.instance.PlaySound();
                while (true)
                {
                    Death_time += Time.deltaTime;
                    if (Death_time >= 1.0f)
                    {
                        Instantiate(Power, transform.position, Quaternion.identity);
                        Destroy(this.gameObject, 1f);
                        break;
                    }
                }
                
            }
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            animator.SetBool("isMoving", true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }
    public void Die()
    {
        if(M_Health > 0)
        {
            M_Health--;            
        }
        else if(M_Health == 0)
        {
            StopCoroutine("ChangeMovement");            
            isDying = true;
            animator.SetBool("isDying", true);
            animator.SetTrigger("isDying");

            SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            renderer.flipY = true;

            BoxCollider2D coll = gameObject.GetComponent<BoxCollider2D>();
            coll.enabled = false;

            Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
            Vector2 dieVelocity = new Vector2(0, -30f);
            rigid.AddForce(dieVelocity, ForceMode2D.Impulse);

            Destroy(gameObject, 5f);
        }        
    }
}
