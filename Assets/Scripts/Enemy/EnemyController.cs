using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    private EnemyBaseState state;
    private GameObject alarmSign;

    [Header("Attribute")]
    public float health;
    public bool isDead;
    public bool hasBomb;

    [Header("Components")]
    public Animator anim;
    public int animState;   //Animator中的state参数:0-idle,1-run,2-attack

    [Header("Movement")]
    public float speed;             //移动速度
    public Transform pointA, pointB;    //怪物的巡逻区间点
    public Transform targetPoint;   //怪物运动的目标点
    public List<Transform> targetList = new List<Transform>();  //目标列表，可能同时有多个目标

    [Header("Attack Control")]
    public float attackCD;
    private float nextAttack = 0;
    public float attackRange, skillRange;

    public PatrolState pState = new PatrolState();
    public AttackState aState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }
    public void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(pState);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            return;
        }
        state.OnState(this);
        anim.SetInteger("state", animState);
    }

    //改变敌人状态为目标状态
    public void SwitchState(EnemyBaseState s)
    {
        state = s;
        state.EnterState(this);
    }

    public void MovetoTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();
    }

    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(transform.position.x - pointA.position.x) < Mathf.Abs(transform.position.x - pointB.position.x))
        {
            targetPoint = pointB;
        }
        else
        {
            targetPoint = pointA;
        }
    }

    public void AttackAction()
    {
        //以怪物中心点为圆心，attackRange为半径的检测范围
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                Debug.Log("进行普通攻击！");
                //设置参数，播放攻击动画
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackCD;
            }
        }
    }

    public virtual void SkillAction()
    {
        //以怪物中心点为圆心，skillRange为半径的检测范围
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                Debug.Log("对炸弹使用技能！");
                //设置参数，播放攻击动画
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackCD;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //作为trigger的collider检测到有对象时将其加入targetList
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!targetList.Contains(collision.transform) && !hasBomb && !isDead && !GameManager.instance.gameOver)
        {
            targetList.Add(collision.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        targetList.Remove(collision.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead&& !GameManager.instance.gameOver)

            StartCoroutine(ShowAlarm());
    }

    //协程
    IEnumerator ShowAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }

}
