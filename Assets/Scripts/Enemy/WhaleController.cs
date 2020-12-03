using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : EnemyController, IDamageable
{
    //尺寸的增加值
    public float scale;
    public void GetHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    //吞下炸弹animation event
    public void Swalow()
    {
        if (targetPoint.CompareTag("Bomb"))
        {
            targetPoint.GetComponent<BombController>().Turnoff();
            targetPoint.gameObject.SetActive(false);

            if (transform.localScale.y < 2f)
            {
                transform.localScale *= scale;
            }
        }
    }


}
