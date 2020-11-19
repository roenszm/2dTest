using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击状态
public class AttackState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        //Debug.Log("发现攻击目标！");
        enemy.targetPoint = enemy.targetList[0];
        //切换为进入攻击状态
        enemy.animState = 2;
    }

    public override void OnState(EnemyController enemy)
    {
        if (enemy.hasBomb)
            return;

        //没有攻击目标时切换回巡逻状态
        if (enemy.targetList.Count == 0)
        {
            enemy.SwitchState(enemy.pState);
        }

        if (enemy.targetList.Count == 1)
        {
            enemy.targetPoint = enemy.targetList[0];
        }
        //根据距离切换攻击目标
        if (enemy.targetList.Count > 1)
        {
            for (int i = 0; i < enemy.targetList.Count; i++)
            {
                if (Mathf.Abs(enemy.transform.position.x - enemy.targetList[i].position.x) <
                    Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.targetList[i];
                }

            }
        }

        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            enemy.SkillAction();
        }

        enemy.MovetoTarget();


    }
}
