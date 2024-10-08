using UnityEngine;

public class Skill_Overload : Skill
{
    private float oribulletSpeed;
    private float oriDamageRate;
    private float oriAtkCD;
    public override void Enter()
    {
        base.Enter();
        oribulletSpeed = player.weapon.bulletSpeed;
        player.weapon.bulletSpeed *= 1.5f;
        oriDamageRate = player.weapon.damageRate;
        player.weapon.damageRate *= 1.2f;
        oriAtkCD = player.weapon.atkCD;
        player.weapon.atkCD *= 0.7f;
    }
    public override void Hold()
    {
        base.Hold();
        player.weapon.bulletAmount = player.weapon.maxBulletAmount;
        player.onBulletChange?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();
        player.weapon.bulletSpeed = oribulletSpeed;
        player.weapon.damageRate = oriDamageRate;
        player.weapon.atkCD = oriAtkCD;
    }
}
