using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent_StrongWill : Talent
{
    [SerializeField] private float protectTimeRate; 
    public override void Effect()
    {
        base.Effect();
        GameController.instance.player.protectTime *= protectTimeRate;
    }
}
