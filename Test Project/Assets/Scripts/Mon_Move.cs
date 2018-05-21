using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Move : MonoBehaviour {

    Animator animator;    
    Vector3 movement;
    GameObject traceTarget;

    int movementFlag = 0;
    public float movePower = 1f;
    public int creatureType;
    public int M_Health = 3;
    bool isTracing = false;
    bool isDying = false;
    bool isHiting = false;

    void Start () {
        animator = gameObject.GetComponentInChildren<Animator>();        
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
            transform.localScale = new Vector3(-8, 8, 1);
        }
        else if(dist == "Right")
        {
            moveVelocity = Vector3.right; //(-1,0,0)
            transform.localScale = new Vector3(8, 8, 1);
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
        if(other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
        if(other.gameObject.tag == "Bullet")
        {
            M_Health--;
            if(M_Health > 0)
            {
                isHiting = true;
                animator.SetBool("isHiting", true);
                animator.SetTrigger("isHiting");
            }
            else if (M_Health == 0)
            {
                isDying = true;
                animator.SetBool("isDying", true);
                animator.SetTrigger("isDying");
                //animator.Play("Mon_Die");
                Destroy(this.gameObject,1);
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
