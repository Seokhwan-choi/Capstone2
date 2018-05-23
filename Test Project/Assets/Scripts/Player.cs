using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public float movePower = 7.5f; // 움직이는 속도
    public float jumpPower = 20f; //  점프 세기
    public float maxSlideTime = 0.3f; // 슬라이딩 시간
    public float shootDelay = 0.5f; // 총알 딜레이
    public int jumpCount = 2; // 점프 가능 횟수
    public int Health_Power = 5;

    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    Vector3 movement;

    bool isUnBeatTime = false;    

    public bool isJumping = false;
    public bool isSliding = false;
    public bool doubleJump = false;
    public bool isShoot = false;
    public bool isWallSliding = false;
    public bool facingright = false;
    public bool inputLeft = false;
    public bool inputRight = false;
    public bool inputJump = false;
    public bool inputAttack = false;
    public bool inputDash = false;

    public bool isAttack = false;
    public bool isJumpCombo = false;

    public GameObject dustEffect;
    public GameObject Bullet;

    public GameObject wallCheck_right;
    public GameObject wallCheck_left;
    public bool wallCheck;
    public LayerMask wallLayerMask;

    public GameObject PowerMagnet;
    public Text PowerCounter;

    [SerializeField]
    GameObject SlideCollider;

    float slideTimer = 0.0f;
    float shootTimer = 0.0f;
    float AttackTime = 0.0f;
    float HoldTime = 0.0f;

    int PowerNumber;
    
	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        UIButton ui = GameObject.FindGameObjectWithTag("Managers").GetComponent<UIButton>();
        ui.init();
        HPbar hp = GameObject.FindGameObjectWithTag("HPbar").GetComponent<HPbar>();
        hp.init();
	}

    // Update is called once per frame
    void Update()
    {
        // 캐릭터 Idle,왼쪽이동,오른쪽이동
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else if ((Input.GetAxisRaw("Horizontal") > 0))
        {
            animator.SetBool("isMoving", true);
        }
        else if ( (Input.GetAxisRaw("Horizontal") < 0))
        {
            animator.SetBool("isMoving", true);
        }

        // 캐릭터 점프
        //if(!inputJump || Input.GetButtonDown("Jump"))
        //{
        //    animator.SetBool("isJumping", false);
        //}
        if ((Input.GetButtonDown("Jump")) && jumpCount > 0)
        {
            isJumping = true; // 점프 상태 true
            // 먼지 Effect를 캐릭터 위치에 생성
            Instantiate(dustEffect, new Vector3(transform.position.x, transform.position.y - 2.5f
                    , transform.position.z), Quaternion.identity);
            animator.SetBool("isJumping", true); // 점프 상태임을 animator에서 확인
            animator.SetTrigger("doJumping"); // 점프 애니메이션 실행
            animator.SetBool("isRide", false);
            //inputJump = false;
        }

        // 캐릭터 슬라이딩
        //if (!inputDash)
        //{
        //    animator.SetBool("isSliding", false);
        //}
        if (Input.GetButtonDown("Fire2") && !animator.GetBool("isSliding"))
        {
            slideTimer = 0f; // 슬라이딩 시간 0으로 초기화
            isSliding = true; // 슬라이딩 상태 true
            animator.SetBool("isSliding", true); // 슬라이딩 애니메이션 실행

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            SlideCollider.GetComponent<CircleCollider2D>().enabled = true;
            //inputDash = false;
        }

        // 캐릭터 슬라이딩 동작
        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            movePower = 20.0f; // 캐릭터의 이동속도를 높여주고 먼지 Effect를 생성
            Instantiate(dustEffect, new Vector3(transform.position.x, transform.position.y - 2.5f
                    , transform.position.z), Quaternion.identity);
            // 약 0.3초가량 이동한다.
            if (slideTimer > maxSlideTime)
            {
                isSliding = false;
                animator.SetBool("isSliding", false);
                movePower = 7.5f;

                gameObject.GetComponent<BoxCollider2D>().enabled = true; // 원래 크기의 히트박스
                SlideCollider.GetComponent<CircleCollider2D>().enabled = false; // 슬라이딩할때 히트박스
            }
            // 사용자가 임의로 멈추면 슬라이딩 멈춘다.
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                isSliding = false;
                animator.SetBool("isSliding", false);
                movePower = 7.5f;

                gameObject.GetComponent<BoxCollider2D>().enabled = true;  // 원래 크기의 히트박스
                SlideCollider.GetComponent<CircleCollider2D>().enabled = false; // 슬라이딩할때 히트박스
            }
        }

        //  캐릭터 탄환발사
        //if (!inputAttack)
        //{
        //    animator.SetBool("isShoot", false);            
        //}
        //else if (inputAttack && !isShoot)
        //{
        //    isShoot = true;
        //    inputAttack = false;
        //}

        // 캐릭터 공격
        if (Input.GetButtonDown("Fire1") && !isAttack && !animator.GetBool("isJumping") && !Input.GetButton("Up"))
        {
            animator.SetBool("Attack", true);
            isAttack = true;
        }
        // 콤보 공격
        if (isAttack)
        {
            if (animator.GetInteger("AttackState") == 4)
            {
                animator.SetBool("Attack", false);
                animator.SetInteger("AttackState", 0);
                isAttack = false;
                AttackTime = 0;
            }
            AttackTime += Time.deltaTime;
            if ( AttackTime >= 0.3f)
            {
                animator.SetBool("Attack", false);
                animator.SetInteger("AttackState", 0);
                isAttack = false;
                AttackTime = 0;
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                    AttackTime = 0;
                }
            }
        }

        // 캐릭터 적 띄우기
        if(Input.GetButtonDown("Fire1") && Input.GetButton("Up") && !isJumpCombo && !animator.GetBool("isJumping") )
        {
            
            animator.SetBool("JumpUpper", true);
            animator.SetBool("isJumping", true);
            isJumpCombo = true;

            rigid.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            jumpCount = 0;
         
        }
        // 점프 중일 때 공격
        if(Input.GetButtonDown("Fire1") && animator.GetBool("isJumping") &&!isJumpCombo)
        {
            animator.SetBool("JumpUpper", true);
            animator.SetInteger("JumpState", 1);
            isJumpCombo = true;
            jumpCount = 0;
        }
        // 캐릭터 점프 콤보
        if (isJumpCombo)
        {          
            if (animator.GetInteger("JumpState") == 4)
            {
                animator.SetBool("JumpUpper", false);
                animator.SetInteger("JumpState", 0);
                isJumpCombo = false;
                AttackTime = 0;
                rigid.gravityScale = 2.0f;
            }
            AttackTime += Time.deltaTime;
            if (AttackTime >= 0.75f)
            {
                animator.SetBool("JumpUpper", false);
                animator.SetInteger("JumpState", 0);
                isJumpCombo = false;
                AttackTime = 0;
                if (animator.GetBool("isJumping"))
                {
                    rigid.gravityScale = 1.0f;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    JumpAttack();
                    AttackTime = 0;
                    rigid.gravityScale = 0.25f;
                }
            }
        }
        // 캐릭터 벽 타기
        if (animator.GetBool("isJumping"))
        {
            if (facingright)
            {
                if (wallCheck = Physics2D.OverlapCircle(wallCheck_right.transform.position, 0.1f, wallLayerMask))
                {
                    jumpCount = 2;
                    animator.SetBool("isRide", true);
                    if (facingright && Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (wallCheck)
                            HandlewallSliding();
                    }
                }
                else
                {
                    animator.SetBool("isRide", false);
                    isWallSliding = false;
                }

            }
            else if (!facingright)
            {
                if (wallCheck = Physics2D.OverlapCircle(wallCheck_left.transform.position, 0.1f, wallLayerMask))
                {
                    jumpCount = 2;
                    animator.SetBool("isRide", true);
                    if (!facingright && Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (wallCheck)
                            HandlewallSliding();
                    }
                }
                else
                {
                    animator.SetBool("isRide", false);
                    isWallSliding = false;
                }
            }
        }
 
        if( !wallCheck && !animator.GetBool("isJumping"))
        {
            isWallSliding = false;
        }

        // Power magnet
        PowerCounter.text = PowerNumber.ToString();
        PowerMagnet.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    // 캐릭터 벽 타기
    void HandlewallSliding()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, -5.0f);

        if (facingright)
        {
            Instantiate(dustEffect, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.35f
                        , transform.position.z), Quaternion.identity);
            spriteRenderer.flipX = true;
        }
        else if (!facingright)
        {
            Instantiate(dustEffect, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.35f
                        , transform.position.z), Quaternion.identity);
            spriteRenderer.flipX = false;
        }

        isWallSliding = true;

    }
    void FixedUpdate()
    {
        Move();
        Jump();
        Shoot();
    }
    // 캐릭터 이동
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (inputLeft || (Input.GetAxisRaw("Horizontal") < 0))
        {
            moveVelocity = Vector3.left;

            spriteRenderer.flipX = true;
            facingright = false;
            
        }
        else if (inputRight || (Input.GetAxisRaw("Horizontal") > 0))
        {
            moveVelocity = Vector3.right;

            spriteRenderer.flipX = false;
            facingright = true;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    // 캐릭터 콤보공격
    void Attack()
    {    
        
        switch (animator.GetInteger("AttackState"))
        {
            case 1:
                animator.SetInteger("AttackState", 2);
                break;
            case 2:
                animator.SetInteger("AttackState", 3);
                break;
            case 3:
                animator.SetInteger("AttackState", 4);
                break;
            case 4:
                animator.SetInteger("AttackState", 0);
                break;
            default:
                animator.SetInteger("AttackState", 1);
                break;
        }
        
    }
    // 캐릭터 점프 콤보공격
    void JumpAttack()
    {
        switch (animator.GetInteger("JumpState"))
        {
            case 1:
                animator.SetInteger("JumpState", 2);
                break;
            case 2:
                animator.SetInteger("JumpState", 3);
                break;
            case 3:
                animator.SetInteger("JumpState", 4);
                break;
            case 4:
                animator.SetInteger("JumpState", 0);
                break;
            default:
                animator.SetInteger("JumpState", 1);
                break;
        }

    }
    // 캐릭터 점프
    void Jump()
    {
        if (!isJumping)
            return;// 이미 점프 중이라면 돌아감
        rigid.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isJumping = false;
        jumpCount--;      
    }

    // 쥬금
    void Die()
    {
        animator.SetTrigger("Death");
    }

    // 캐릭터 총알발사
    void Shoot()
    {
        if (!isShoot)
            return;
        shootTimer = 5.0f;
        if (shootTimer > shootDelay)
        {
            if (spriteRenderer.flipX)
            {
                Instantiate(Bullet, new Vector3(transform.position.x - 2.5f, transform.position.y - 0.35f
                        , transform.position.z), Quaternion.identity);
                isShoot = false;
                shootTimer = 0;
            }
            else
            {
                Instantiate(Bullet, new Vector3(transform.position.x + 2.5f, transform.position.y - 0.35f
                        , transform.position.z), Quaternion.identity);
                isShoot = false;
                shootTimer = 0;
            }
        }
        shootTimer += Time.deltaTime;
    }

    // 바닥 밟았을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Attach : " + other.gameObject.layer);

        if (other.gameObject.layer == 9)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRide", false);
            doubleJump = false;
            Instantiate(dustEffect, new Vector3(transform.position.x, transform.position.y - 2.5f
                    , transform.position.z), Quaternion.identity);
            jumpCount = 2;
            isJumping = false;
            wallCheck = false;
            rigid.gravityScale = 1.0f;
        }

        if (other.gameObject.tag.Equals("Power"))
        {
            Destroy(other.gameObject);
            PowerNumber += 1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Attach : " + other.gameObject.layer);
    }

    void OnCollisionEnter2D(Collision2D other)//몬스터랑 충돌시
    {
        if (other.gameObject.tag == "Monster" && animator.GetBool("Attack") == false)
        {
            Vector2 killVelocity = new Vector2(0, 0);
            if (!facingright)
            {
                killVelocity = new Vector2(10f, 0);
                
                animator.SetTrigger("Hit");
                Health_Power--;
            }
            else if (facingright)
            {
                killVelocity = new Vector2(-10f, 0);
                spriteRenderer.flipX = false;
                animator.SetTrigger("Hit");

                Health_Power--;
            }
            rigid.AddForce(killVelocity, ForceMode2D.Impulse);
            if (Health_Power <= 0)
            {
                Die();
            }
            isUnBeatTime = true;
            StartCoroutine("UnBeatTime");
        }        
    }
    IEnumerator UnBeatTime()//무적시간 코루틴
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        isUnBeatTime = false;
        yield return null;
    }

    
}
