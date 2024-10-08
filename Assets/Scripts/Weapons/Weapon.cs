using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Visual")]
    public Sprite weaponIcon;
    public string weaponName;
    [Multiline(5)]
    public string weaponDescription;
    [Header("Audio")]
    public AudioClip au_reload;
    public AudioClip au_fire;
    public AudioClip au_ready;
    [Header("Game")]
    public float offsetRatius;
    public float damageRate;
    public float bulletSpeed;
    public float atkCD;
    [HideInInspector] public float atkCDTimer;
    public float reloadTime;
    [HideInInspector] public float reloadTimer;
    public int maxBulletAmount;
    [HideInInspector] public int bulletAmount;
    public Bullet bullet;
    private List<Bullet> bulletPool = new();

    [Header("Child")]
    //
    private Animator anim;
    private Player player;

    private int aimDir = 1;

    void Start()
    {

        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();

        bulletAmount = maxBulletAmount;

        Bullet firstBullet = Instantiate(bullet);
        firstBullet.gameObject.SetActive(false);
        bulletPool.Add(firstBullet);

        GameController.instance.bulletPools.Add(this, bulletPool);
    }

    private void OnDestroy()
    {
        GameController.instance.bulletPools.Remove(this);
    }

    void Update()
    {
        if(GameController.instance.isPause)
        {
            return;
        }

        if (Input.GetKey(GameController.instance.Fire))
        {
            Attack();
        }

        if (Input.GetKeyDown(GameController.instance.Reload) || (bulletAmount == 0 && !anim.GetBool("isReloading")))
        {
            Reload();
        }

        if (atkCDTimer > 0)
        {
            atkCDTimer -= Time.deltaTime;
        }

        if (reloadTimer >= 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        else if (reloadTimer < 0 && reloadTimer > -100)
        {
            FinishReload();
            reloadTimer = -101;
        }


        transform.rotation = GameController.instance.relaMouseRot;
        transform.Rotate(0, 0, 90);
        if (player.faceDir * aimDir < 0)
        {
            transform.GetChild(0).Rotate(180, 0, 0);
            aimDir *= -1;
        }
    }
    /// <summary>
    /// 按下攻击键时调用，包含非法返回
    /// </summary>
    public void Attack()
    {
        if (atkCDTimer > 0 || reloadTimer > 0)
        {
            return;
        }
        Fire();
        anim.Play("Weapon_Normal", 0, 0);
        bulletAmount--;
        player.onBulletChange?.Invoke();
        atkCDTimer = atkCD;

    }
    /// <summary>
    /// 生成子弹实例
    /// </summary>
    private void Fire()
    {
        GameController.instance.au.PlayOneShot(au_fire);
        Bullet theBullet = null;
        bool find = false;
        foreach (Bullet item in bulletPool)
        {
            if (!item.gameObject.activeSelf)
            {
                find = true;
                theBullet = item;
                theBullet.gameObject.SetActive(true);
                break;
            }
        }

        if (!find)
        {
            theBullet = Instantiate(bullet);
            bulletPool.Add(theBullet);
        }

        theBullet.transform.position = transform.GetChild(0).position + (Vector3)Random.insideUnitCircle * offsetRatius;
        theBullet.damage = (int)(damageRate * player.atk);
        theBullet.speed = bulletSpeed;
        theBullet.dir = GameController.instance.relaMouseDir;
    }
    /// <summary>
    /// 换弹，包含非法返回
    /// </summary>
    private void Reload()
    {
        if (reloadTimer > 0 || bulletAmount == maxBulletAmount)
        {
            return;
        }

        GameController.instance.au.PlayOneShot(au_reload);

        reloadTimer = reloadTime;
        anim.SetBool("isReloading", true);
    }
    /// <summary>
    /// 换弹结束
    /// </summary>
    private void FinishReload()
    {
        GameController.instance.au.PlayOneShot(au_ready);
        anim.SetBool("isReloading", false);
        bulletAmount = maxBulletAmount;
        player.onBulletChange?.Invoke();
    }

}
