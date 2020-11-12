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
        if (Time.time > startTime + waitTime)
        {
            anim.Play("bomb_explosion");
        }
    }

    public void OnDrawGizmos()
    {
        //绘制爆炸半径
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    //Animation Event
    void Explosion()
    {
        //保证炸弹本身在爆炸时位置不变
        coll.enabled = false;
        rb.gravityScale = 0;

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        foreach (var item in aroundObjects)
        {
            Vector3 relativePos = item.transform.position - transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((relativePos + bombUp) * bombForce, ForceMode2D.Impulse);

        }

    }

    public void DestroyBomb()
    {
        Destroy(gameObject);
    }

}
