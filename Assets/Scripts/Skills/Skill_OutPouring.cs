using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_OutPouring : Skill
{
    private float oriAtkCD;
    private float oriOffset;
    public override void Enter()
    {
        base.Enter();
        oriAtkCD = player.weapon.atkCD;
        player.weapon.atkCD = .05f;
        oriOffset = player.weapon.offsetRatius;
        player.weapon.offsetRatius = 1f;
        anim.SetBool("Pouring", true);
    }

    public override void Exit()
    {
        base.Exit();
        player.weapon.atkCD = oriAtkCD;
        player.weapon.offsetRatius = oriOffset;
        anim.SetBool("Pouring", false);
    }

    public override void Hold()
    {
        base.Hold();
        player.weapon.Attack();
        if(player.weapon.bulletAmount == 0)
        {
            anim.SetBool("Pouring", false);
        }
    }
}
