using System.Collections.Generic;
using UnityEngine;

//巡逻状态
public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        //进入巡逻状态时先切换为idle动画
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnState(EnemyController enemy)
    {
        //检测当前巡逻状态，若不在站立状态则继续巡逻
        if(!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animState = 1;
            enemy.MovetoTarget();
        }

        //如果已到达目标巡逻点则切换目标点（同时切换一次巡逻状态，可以使敌人保持在目标点保持idle状态一次）
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.SwitchState(enemy.pState);
        }
        if (enemy.targetList.Count > 0)
        {
            enemy.SwitchState(enemy.aState);
        }
        
    }
}
