using UnityEngine;

public class Enemy_Boomer : Enemy
{
    [SerializeField]private float boomCD;
    [SerializeField] private float detectRadius;
    [SerializeField] private float boomRadius;
    private bool isBooming;
    private bool boomed;
    private float boomCDTimer;

    protected override void Start()
    {
        base.Start();
        boomCDTimer = boomCD;
    }
    public override void Move()
    {
        base.Move();
        if ((transform.position - GameController.instance.player.transform.position).magnitude > detectRadius && !isBooming)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameController.instance.player.transform.position, Time.deltaTime * speed);
        }
        else if (boomCDTimer > 0)
        {
            isBooming = true;
            boomCDTimer -= Time.deltaTime;
            sr.color = Color.red;
            anim.speed = 0;
            StartCoroutine(FlashFX());
        }
        else if (boomCDTimer < 0 && !boomed)
        {
            sr.color = Color.white;
            Boom();
            isDead = true;
            boomed = true;
            
        }
    }



    private void Boom()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_explode);
        GameController.instance.au.PlayOneShot(GameController.instance.au_enemyExplode);

        anim.speed = 1;
        anim.SetBool("isBooming", true);

        Collider2D[] hit2Ds = Physics2D.OverlapCircleAll(transform.position, boomRadius);

        foreach (Collider2D item in hit2Ds)
        {
            if (item.GetComponent<Player>() != null)
            {
                item.GetComponent<Player>().BeDamaged(damage * 2);
            }
            else if (item.GetComponent<Enemy>() != null)
            {
                if(item.GetComponent<Enemy>() != this)
                {
                    item.GetComponent<Enemy>().BeDamaged(damage * 10);

                }
            }
        }
    }
}
