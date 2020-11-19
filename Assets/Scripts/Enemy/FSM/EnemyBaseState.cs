
public abstract class EnemyBaseState
{
    //进入某个状态时的抽象方法
    public abstract void EnterState(EnemyController enemy);

    //处于某个状态中的抽象方法
    public abstract void OnState(EnemyController enemy);
   
}
