using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{

    public float damage;
    private int attackDir;
    public bool bombAvailable;

    public void OnTriggerEnter2D(Collider2D targetObj)
    {
        
        if (targetObj.CompareTag("Player"))
        {
            Debug.Log("玩家受到伤害！");
            targetObj.GetComponent<IDamageable>().GetHit(damage);
        }
        
        if (targetObj.CompareTag("Bomb") && bombAvailable)
        {
            if (transform.position.x > targetObj.transform.position.x)
            {
                attackDir = -1; //炸弹在玩家左侧
            }
            else
            {
                attackDir = 1;
            }
            targetObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackDir, 1) * 10, ForceMode2D.Impulse);

        }
    }
}
