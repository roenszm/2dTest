using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    //获得当前对象
    private Rigidbody2D rb;
    private Animator anim;
    

    /*
    [Header("Player Attributes")]
    public float velocityX;
    public float velocityY;
    */
    [Header("Attribute")]
    public float health;
    public bool isDead;
    //移动速度
    public float speed;
    //跳跃初始力量
    public float jumpForce;

    [Header("Ground Check")]
    //检测player是否落地的检测点
    public Transform groundCheckPoint;
    //检测为盒形区域，其长和宽
    public float length;    //长，x方向
    public float width;     //宽，y方向
    //检测半径
    //public float checkRadius;
    //检测的地面layer
    public LayerMask groundLayer;

    [Header("State Check")]
    //player是否落地
    public bool isGround;
    //是否处于跳跃状态
    //public bool isJump;
    //player是否执行跳跃
    public bool canJump;

    [Header("Effects")]
    public GameObject jumpSmog;
    public GameObject fallSmog;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackCD;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            return;
        }
        CheckInput();
        //GetPlayerAttributes();
    }

    public void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        PhysicsCheck();
        Movement();
        Jump();
    }
    
    //检测输入
    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    //player的左右移动
    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    // -1 ~ 1
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

    }

    //player跳跃
    void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //isJump = true;
            jumpSmog.SetActive(true);
            jumpSmog.transform.position = transform.position + new Vector3(0, -0.48f, 0);
            rb.gravityScale = 5;
            canJump = false;
        }
    }

    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);
            nextAttack = Time.time + attackCD;
        }
    }

    //检测并调整一些物理属性
    void PhysicsCheck()
    {
        //盒形检测，用于解决跳到平台地面边缘时不能检测为落地的问题
        isGround = Physics2D.OverlapBox(groundCheckPoint.position, new Vector2(length, width), 0, groundLayer);
        //isGround = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            //isJump = false;
        }
        // 从高处掉落时也遵从和跳跃一样的重力手感
        else
        {
            rb.gravityScale = 5;
        }
    }

    //落地时显示落地的烟雾动画 Animation Event
    void ShowFallSmog()
    {
        fallSmog.SetActive(true); 
        fallSmog.transform.position = transform.position + new Vector3(0, -0.75f, 0);

    }

    //获得player在游戏时的属性值，用于在unity中play时能查看，测试用
    /*
    void GetPlayerAttributes()
    {
        velocityX = rb.velocity.x;
        velocityY = rb.velocity.y;
    }
    */

    //画出检测点的检测范围
    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        Gizmos.DrawWireCube(groundCheckPoint.position, new Vector3(length, width, 0));
    }

    public void GetHit(float damage)
    {
        //当玩家处于正在受伤状态时，不能受到其他伤害
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

            UIManager.instance.UpdateHealth(health);
        }
       
    }
}
