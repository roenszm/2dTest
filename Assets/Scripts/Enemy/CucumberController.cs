using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberController : EnemyController, IDamageable
{
    //吹灭炸弹animation event
    public void Setoff()
    {
        //保证在进行吹灭时，当前的目标是炸弹
        if (targetPoint.CompareTag("Bomb"))
        {
            targetPoint.GetComponent<BombController>().Turnoff();
        }
        
    }

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

}
