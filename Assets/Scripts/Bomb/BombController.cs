using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    [Header("Bomb Control")]
    public float startTime;//炸弹生成的时间
    public float waitTime;  //炸弹待机时间
    public float bombForce; //炸弹爆炸的力量
    public Vector3 bombUp;    //炸弹爆炸时额外增加的向上的方向
    public float bombDamage;

    [Header("Explosion Check")]
    public float explosionRadius;//爆炸范围
    public LayerMask targetLayer;//爆炸影响的物体图层

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("bomb_explosion");
            }
        }
        
    }

    public void OnDrawGizmos()
    {
        //绘制爆炸半径
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    //Animation Event
    public void Explosion()
    {
        //保证炸弹本身在爆炸时位置不变
        coll.enabled = false;
        rb.gravityScale = 0;

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        foreach (var item in aroundObjects)
        {
            Vector3 relativePos = item.transform.position - transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((relativePos + bombUp) * bombForce, ForceMode2D.Impulse);

            if(item.CompareTag("Bomb") && 
                item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<BombController>().Turnon();
            }

            if (item.CompareTag("Player"))
            {
                item.GetComponent<IDamageable>().GetHit(bombDamage);
            }
    
        }

    }

    public void DestroyBomb()
    {
        Destroy(gameObject);
    }


    public void Turnoff()
    {
        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("npc");

    }

    public void Turnon()
    {
        startTime = Time.time;
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("bomb");
    }
}
