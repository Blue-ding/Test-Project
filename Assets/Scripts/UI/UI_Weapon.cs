using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon : UI_Game
{
    public Image imWeapon;
    public TextMeshProUGUI tmpName;
    public TextMeshProUGUI tmpBullet;
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.instance.player.onEquipmentChange += ChangeName;
        GameController.instance.player.onBulletChange += ChangeBullet;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.instance.player.onEquipmentChange -= ChangeName;
        GameController.instance.player.onBulletChange -= ChangeBullet;
    }

    private void ChangeName()
    {
        imWeapon.sprite = GameController.instance.player.weapon.weaponIcon;
        tmpName.text = GameController.instance.player.weapon.weaponName;
    }

    private void ChangeBullet()
    {
        tmpBullet.text = GameController.instance.player.weapon.bulletAmount + "/" + GameController.instance.player.weapon.maxBulletAmount;
    }


}
