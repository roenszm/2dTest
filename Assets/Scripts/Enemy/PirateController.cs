﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : EnemyController, IDamageable
{
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
