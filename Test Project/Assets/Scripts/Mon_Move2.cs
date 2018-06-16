using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using debug = UnityEngine.Debug;

public class Mon_Move2 : MonoBehaviour
{

    Animator animator;
    GameObject traceTarget;
    public Vector3 moveVelocity = Vector3.zero; //0,0,0으로 초기화    
    Vector3 movement;
    Rigidbody2D rigid;
    Headhpbar hps;
    Vector2 loc;
    public GameObject player;
    public Player playerScript;
    float Death_time = 0.0f;
    public GameObject bullet;
    public Transform firePos;
    int movementFlag = 0;
    public float movePower = 5f;
    public int AttackType = 0;
    public int creatureType;
    public int M_Health = 20;
    public string dist = "Left";
    int Jumpcount = 0;

    public bool isTracing = false;
    bool isDying = false;
    bool isHiting = false;
    bool isAttack = false;

    float checkTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();        
        hps = GameObject.FindGameObjectWithTag("HPbar").GetComponent<Headhpbar>();
        hps.init();        
        StartCoroutine("ChangeMovement", 1f);
    }

    IEnumerator ChangeMovement() //움직이는 코루틴
    {
        movementFlag = Random.Range(0, 3);
        if (movementFlag == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine("ChangeMovement", 1f);
    }
    IEnumerator Attack() // 공격 코루틴
    {
        AttackType = Random.Range(0, 200); // 0, 1 ,2
        /// 1 => 이동 2 => 공격(칼) 3 => 공격(대쉬,점프) 이외 => 가만히 있기
        if (AttackType == 1) // move
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isMissile", false);
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", false);

        }
        else if (AttackType == 2) // attack1
        {
            animator.SetBool("isAttacking1", true);            
            animator.SetBool("isMoving", false);
            animator.SetBool("isMissile", false);
            animator.SetBool("isAttacking2", false);
        }
        else if (AttackType == 3) // attack2
        {
            animator.SetBool("isMoving", false);            
            animator.SetBool("isAttacking2", true);
            animator.SetBool("isMissile", false);
            animator.SetBool("isAttacking1", false);
            transform.position += moveVelocity * 4 * Time.deltaTime;
            if (Jumpcount > 0)
            {
                rigid.velocity = Vector2.zero;
                Vector2 jumpVelocity = new Vector2(0, 6);
                rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            }
            Jumpcount--;
        }
        else if (AttackType == 4)
        {
            checkTime += Time.deltaTime;
            animator.SetTrigger("Missile");            
            if (checkTime > 0.5f)
            {                
                StartCoroutine("Attack", 1f);
                checkTime = 0;
            }
            Fire();
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", false);
            animator.SetBool("isMissile", false);
        }
        debug.Log(AttackType);
        yield return new WaitForSeconds(5f);

        StartCoroutine("Attack", 3f);
    }      
    void FixedUpdate()
    {
        Move();        
    }
    //움직이는 함수
    void Move()
    {   
        moveVelocity = Vector3.zero;//초기화
        //추적
        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
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
            transform.localScale = new Vector3(-2, 2, 1);
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right; //(-1,0,0)
            transform.localScale = new Vector3(2, 2, 1);
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }        
    }
    // 플레이어가 원안에 들어왔을 때 
    void OnTriggerEnter2D(Collider2D other)
    {
        Jumpcount = 5;

        if (other.gameObject.tag == "Attack_check")
        {
            Sound1.instance.PlaySound();
            M_Health--;            
            StopCoroutine("Attack");
            checkTime += Time.deltaTime;            
            animator.SetTrigger("isHit");
            if (checkTime > 0.5f)
            {
                StartCoroutine("Attack", 1f);
                checkTime = 0;
            }
            transform.position += moveVelocity * 2 * Time.deltaTime;
            if (M_Health == 0)
            {                
                isDying = true;                
                animator.SetTrigger("isDie");
                Sound2.instance.PlaySound();
                Death_time += Time.deltaTime;
                StopCoroutine("ChangeMovement");
                Destroy(this.gameObject, 1f);
                SceneManager.LoadScene("Win");
                Destroy(hps, 1);
            }
        }
    }
    // 플레이어가 원안에 있을 때
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.tag == "Player")
            {
                traceTarget = other.gameObject;
                StopCoroutine("ChangeMovement");
                isTracing = true;
                StartCoroutine("Attack", 1f);
            }
        }
    }
    // 플레이어가 원 밖으로 나갈 때
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StopCoroutine("Attack");
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", false);
            animator.SetBool("isMissile", false);
            StartCoroutine("ChangeMovement", 1f);

        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        isTracing = false;
    }
    void Fire()
    {
        CreateBullet();
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}