using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent_DamageBoost : Talent
{
    [SerializeField]private int damageBoost;

    public override void Effect()
    {
        base.Effect();
        GameController.instance.player.atk += damageBoost;
    }
}
