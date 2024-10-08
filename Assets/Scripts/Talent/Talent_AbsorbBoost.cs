using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent_AbsorbBoost : Talent
{
    [SerializeField] private float absorbBoostRate;

    public override void Effect()
    {
        base.Effect();
        GameController.instance.player.expGainRange *= absorbBoostRate;
    }
}
