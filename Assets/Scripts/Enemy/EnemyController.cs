using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    private EnemyBaseState state;

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
    void Update()
    {
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
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                Debug.Log("进行普通攻击！");
                //播放攻击动画
                nextAttack = Time.time + attackCD;
            }
        }
    }

    public void SkillAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                Debug.Log("对炸弹使用技能！");
                //播放攻击动画
                nextAttack = Time.time + attackCD;
            }
        }
    }

    //作为trigger的collider检测到有对象时将其加入targetList
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!targetList.Contains(collision.transform))
        {
            targetList.Add(collision.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        targetList.Remove(collision.transform);
    }

}
