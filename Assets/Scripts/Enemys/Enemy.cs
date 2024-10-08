using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType
{
    Around, Horizental, Still, Bullet
}

public class Enemy : MonoBehaviour
{
    [Header("Game")]
    public float speed;
    public int currentHP;
    public int maxHP;
    public int damage;
    public int level = 1;
    public int difficulty;
    public int expAmount;
    [SerializeField] private EnemyType type;

    [Header("Child")]
    //

    protected Animator anim;
    protected SpriteRenderer sr;
    private Material oriMaterial;
    private bool isFlashing;
    protected bool isDead;

    public System.Action onFlip;
    [HideInInspector] public int faceDir = 1;
    [HideInInspector] public Vector3 moveDir;

    protected virtual void Start()
    {
        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
        sr = GetComponent<SpriteRenderer>();
        oriMaterial = sr.material;

        maxHP = (int)(maxHP * (0.9f + level / 10f));
        speed *= 0.9f + level / 10f;
        currentHP = maxHP;
    }

    private void Update()
    {
        if (isDead||GameController.instance.isPause)
        {
            return;
        }

        Move();
        moveDir = GameController.instance.player.transform.position - transform.position;

        Filp();

        if (!GameController.instance.player.Frozen && (transform.position - GameController.instance.player.transform.position).magnitude > Camera.main.orthographicSize * 3)
        {
            if (type != EnemyType.Still)
            {
                Destroy(gameObject);
            }
        }
        if (GameController.instance.player.Frozen && (transform.position - GameController.instance.player.transform.position).magnitude < 10)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// ���˷�ת����
    /// </summary>
    private void Filp()
    {
        if (type == EnemyType.Around)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            transform.Rotate(0, 90 - 90 * faceDir, 90);
            if (faceDir * moveDir.x < 0)
            {
                faceDir *= -1;
                onFlip?.Invoke();
            }
        }
        if (type == EnemyType.Horizental)
        {
            if (faceDir * moveDir.x < 0)
            {
                transform.Rotate(0, 180, 0);
                faceDir *= -1;
                onFlip?.Invoke();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.CompareTag("Bullet"))
        {
            if (type == EnemyType.Bullet)
            {
                return;
            }
            BeDamaged(collision.GetComponent<Bullet>().damage);
            collision.GetComponent<Bullet>().ExEffect(this);
            collision.gameObject.SetActive(false);
            collision.gameObject.transform.position = GameController.instance.player.transform.position;
        }
        if (collision.CompareTag("Player"))
        {
            DealDamage();
        }
    }

    /// <summary>
    /// ʹ�����ܻ�������ʧ����ֵ
    /// </summary>
    /// <param name="damage">�˺���</param>
    public virtual void BeDamaged(int damage)
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_enemyBeHit[Random.Range(0,GameController.instance.au_enemyBeHit.Count)]);

        currentHP -= damage;
        StartCoroutine(FlashFX());
        if (currentHP <= 0)
        {
            Dead();
        }
    }
    /// <summary>
    /// �����ܻ���˸��Ч
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {
        if (!(isFlashing))
        {
            isFlashing = true;
            sr.material = GameController.instance.FlashFX;
            yield return new WaitForSeconds(.1f);
            sr.material = oriMaterial;
            isFlashing = false;
        }
    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="amount">������</param>
    public virtual void BeHeal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    /// <summary>
    /// ����������������
    /// </summary>
    public virtual void Dead()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_enemyDead);
        for (int i = 0; i < expAmount; i++)
        {
            Instantiate(GameController.instance.exp).transform.position = (Vector3)Random.insideUnitCircle * 2 + transform.position;
        }
        if(anim!= null)
        {
            anim.SetBool("isDead", true);
            anim.speed = 1;
        }
        else
        {
            DeadEnd();
        }
        sr.color = Color.white;
        isDead = true;
    }
    /// <summary>
    /// ����������������ʱ����
    /// </summary>
    public virtual void DeadEnd()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// ���������˺�
    /// </summary>
    public virtual void DealDamage()
    {
        GameController.instance.player.BeDamaged(damage);
    }
    /// <summary>
    /// ����������Ϊ��ÿ֡����
    /// </summary>
    public virtual void Move()
    {
        //Each enemy is special;
    }
}
