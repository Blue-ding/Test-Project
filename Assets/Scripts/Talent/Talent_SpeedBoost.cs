using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent_SpeedBoost : Talent
{
    [SerializeField]private float boostSpeed;
    public override void Effect()
    {
        base.Effect();
        GameController.instance.player.speed += boostSpeed;
    }

}
