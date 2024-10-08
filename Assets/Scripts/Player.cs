using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public System.Action onFlip;
    public System.Action onHPChange;
    public System.Action onEquipmentChange;
    public System.Action onBulletChange;
    public System.Action onSkillChange;
    public System.Action onExpChange;
    public System.Action onLevelChange;

    /// <summary>
    /// ����ˢ��������Ϸ��UI�����ڳ�ʼ��
    /// </summary>
    public void EventInitiate()
    {
        onHPChange?.Invoke();
        onEquipmentChange?.Invoke();
        onBulletChange?.Invoke();
        onSkillChange?.Invoke();
        onExpChange?.Invoke();
        onLevelChange?.Invoke();
    }

    [Header("UIFrozen")]
    public bool Frozen;

    [Header("Game")]
    public int maxHP;
    public int currentHP;
    public float speed;
    public float protectTime;
    public float expGainRange;
    [HideInInspector] private float protectTimer;
    [HideInInspector] public int exp;
    [HideInInspector] public int level;
    public int[] maxExp;
    public int atk;
    [HideInInspector] public int faceDir = 1;

    [HideInInspector] public Weapon weapon;
    [HideInInspector] public Skill[] skills;

    //
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Material oriMaterial;

    //Patch
    private float moveAudioTimer;


    void Start()
    {
        if (Frozen)
        {
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        oriMaterial = sr.material;
    }

    void Update()
    {
        if (Frozen||GameController.instance.isPause)
        {
            return;
        }


        if (protectTimer >= 0)
        {
            protectTimer -= Time.deltaTime;
        }

        MoveControl();

        if(Input.GetKey(GameController.instance.Fire))
        {
            if (faceDir * GameController.instance.relaMouseDir.x < 0)
            {
                Flip();
                faceDir *= -1;
            }
        }
        else
        {
            if (faceDir * rb.velocity.x < 0)
            {
                Flip();
                faceDir *= -1;
            }
        }
    }
    /// <summary>
    /// ��ɫ�ƶ����ƣ�ÿ֡����
    /// </summary>
    private void MoveControl()
    {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (moveAudioTimer > 0)
            {
                moveAudioTimer -= Time.deltaTime;
            }
            else
            {
                moveAudioTimer = 2.2f / speed;
                GameController.instance.au.PlayOneShot(GameController.instance.au_moveOnGrass[Random.Range(0, GameController.instance.au_moveOnGrass.Count)]);
            }
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }


    }
    /// <summary>
    /// ʹ��ɫ�ܻ�����ʧ����ֵ
    /// </summary>
    /// <param name="damage"></param>
    public void BeDamaged(int damage)
    {
        if (protectTimer > 0 || Frozen)
        {
            return;
        }

        GameController.instance.au.PlayOneShot(GameController.instance.au_beHit);

        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            onHPChange?.Invoke();
            Dead();
        }
        else
        {
            onHPChange?.Invoke();
            Protect(protectTime);
            StartCoroutine(FlashFX());
        }
    }
    /// <summary>
    /// ��ɫ�ܻ���˸��Ч
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {
        sr.material = GameController.instance.FlashFX;
        yield return new WaitForSeconds(.1f);
        sr.material = oriMaterial;
        while (protectTimer > 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            yield return new WaitForSeconds(.1f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.8f);
            yield return new WaitForSeconds(.1f);
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }
    /// <summary>
    /// ���ƽ�ɫ
    /// </summary>
    /// <param name="amount"></param>
    public void BeHeal(int amount)
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_beHeal);
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        onHPChange?.Invoke();
    }
    /// <summary>
    /// ʹ������������
    /// </summary>
    /// <param name="amount"></param>
    public void BeStrong(int amount)
    {
        maxHP += amount;
        currentHP += amount;
        onHPChange?.Invoke();
    }
    /// <summary>
    /// ��ɫ���������������ת��ʧ�ܽ���
    /// </summary>
    public void Dead()
    {
        GameController.instance.End(false);
    }
    /// <summary>
    /// ʹ��ɫ�����ܻ��޵�ʱ��
    /// </summary>
    /// <param name="duration">�޵�ʱ�䳤��</param>
    public void Protect(float duration)
    {
        protectTimer = duration;
    }

    /// <summary>
    /// ʹ��ɫ��þ���
    /// </summary>
    /// <param name="amount">��ȡ��</param>
    public void GainExp(int amount)
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_gainExp);
        exp += amount;
        onExpChange?.Invoke();
        if (level < maxExp.Length && exp == maxExp[level - 1])
        {
            LevelUp();
        }
    }
    /// <summary>
    /// ʹ��ɫ����
    /// </summary>
    public void LevelUp()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_levelUp);
        exp -= maxExp[level - 1];
        onExpChange?.Invoke();
        level++;
        onLevelChange?.Invoke();
        GameController.instance.LevelUP();

    }
    /// <summary>
    /// ��ת����
    /// </summary>
    private void Flip()
    {
        onFlip?.Invoke();
        transform.Rotate(0, 180, 0);
    }
    /// <summary>
    /// ˢ�½�ɫ�������뼼������
    /// </summary>
    public void ReFreshEquipment()
    {
        weapon = GetComponentInChildren<Weapon>();
        skills = GetComponentsInChildren<Skill>();
        onEquipmentChange?.Invoke();
    }
}
