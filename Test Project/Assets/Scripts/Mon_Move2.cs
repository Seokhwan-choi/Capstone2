using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mon_Move2 : MonoBehaviour
{

    Animator animator;
    GameObject traceTarget;
    Vector3 moveVelocity = Vector3.zero; //0,0,0으로 초기화    
    Vector3 movement;
    Rigidbody2D rigid;
    Headhpbar hps;
    public GameObject player;
    public Player playerScript;

    int movementFlag = 0;
    public float movePower = 5f;
    public int AttackType = 0;
    public int creatureType;
    public int M_Health = 20;
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
        StartCoroutine("ChangeMovement");
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
        StartCoroutine("ChangeMovement");
    }
    IEnumerator Attack() // 공격 코루틴
    {
        AttackType = Random.Range(0, 20); // 0, 1 ,2
        /// 1 => 이동 2 => 공격(칼) 3 => 공격(대쉬,점프) 이외 => 가만히 있기
        if (AttackType == 1) // move
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", false);

        }
        else if (AttackType == 2) // attack1
        {
            animator.SetBool("isAttacking1", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking2", false);
        }
        else if (AttackType == 3) // attack2
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking2", true);
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
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", false);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine("Attack");
    }
    void FixedUpdate()
    {
        Move();
        Die();
    }
    //움직이는 함수
    void Move()
    {   
        moveVelocity = Vector3.zero;//초기화
        string dist = "";
        //추적
        if (isTracing)
        {
            Vector2 loc = traceTarget.transform.position - transform.position;
            transform.position += (traceTarget.transform.position - transform.position).normalized * movePower * Time.deltaTime;
            if(loc.x >= 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            /*
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
                */
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
            M_Health--;
            StopCoroutine("Attack");
            checkTime += Time.deltaTime;
            //isHiting = true;
            //animator.SetBool("isHiting", true);
            animator.SetTrigger("isHit");
            if (checkTime > 0.5f)
            {
                StartCoroutine("Attack");
                checkTime = 0;
            }

            if (M_Health < 0)
            {
                isDying = true;
                //animator.SetBool("isDying", true);
                animator.SetTrigger("isDie");
                Destroy(this.gameObject, 1);
                Destroy(hps,1);
            }
        }
    }
    // 플레이어가 원안에 있을 때
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
            isTracing = true;
            StartCoroutine("Attack");
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
            StartCoroutine("ChangeMovement");

        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        isTracing = false;
    }    
    public void Die()
    {
        if (M_Health == 0)
        {
            StopCoroutine("ChangeMovement");
            isDying = true;
            animator.SetTrigger("isDie");
            SceneManager.LoadScene("Retry");
            Destroy(gameObject, 1f);
        }
    }
}