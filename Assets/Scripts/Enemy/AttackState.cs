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
        //进入攻击状态时animator先切换为跑步动画
        //enemy.animState = 1;
    }

    public override void OnState(EnemyController enemy)
    {
        //没有攻击目标时切换回巡逻状态
        if (enemy.targetList.Count == 0)
        {
            enemy.SwitchState(enemy.pState);
        }
        //根据距离切换攻击目标
        if (enemy.targetList.Count > 0)
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
