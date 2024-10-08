public class Enemy_TreeMosnter : Enemy
{
    public override void Move()
    {
        base.Move();
        if ((GameController.instance.player.transform.position - transform.position).magnitude < 6)
        {
            anim.SetBool("Awake", true);
        }
    }
}
