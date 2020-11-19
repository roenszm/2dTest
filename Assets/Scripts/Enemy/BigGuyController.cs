using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyController : EnemyController, IDamageable
{
    public Transform pickupPoint;
    public float throwPower;

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

    //拾起炸弹Animation Event
    public void PickupBomb()
    {
        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {
            targetPoint.position = pickupPoint.position;
            targetPoint.SetParent(pickupPoint);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
        }
    }

    //扔掉炸弹Animation Event
    public void ThrowAway()
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);
            if(FindObjectOfType<PlayerController>().transform.position.x - transform.position.x < 0)
            {
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * throwPower, ForceMode2D.Impulse);
            }
            else
            {
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * throwPower, ForceMode2D.Impulse);
            }
        }
        hasBomb = false;
    }
}
