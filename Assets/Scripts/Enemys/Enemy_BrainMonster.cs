using UnityEngine;

public class Enemy_BrainMonster : Enemy
{
    public override void Move()
    {
        base.Move();
        transform.position = Vector3.MoveTowards(transform.position, GameController.instance.player.transform.position, Time.deltaTime * speed);
    }
}
