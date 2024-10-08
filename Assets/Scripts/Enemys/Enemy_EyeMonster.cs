using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EyeMonster : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float atkCD;
    private float atkCDTimer = 0;
    [SerializeField] private float atkDistance;
    public override void Move()
    {
        base.Move();

        if((transform.position - GameController.instance.player.transform.position).magnitude > atkDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameController.instance.player.transform.position, Time.deltaTime * speed);
        }
        else
        {
            if(atkCDTimer > 0)
            {
                atkCDTimer -= Time.deltaTime;
            }
            else
            {
                Enemy bulletScript = Instantiate(bullet).GetComponent<Enemy>();
                bulletScript.transform.position = transform.position;
                bulletScript.GetComponent<Rigidbody2D>().velocity = GameController.instance.player.transform.position - transform.position;
                bulletScript.speed = speed * 3;
                atkCDTimer = atkCD;
            }
        }


    }
}
