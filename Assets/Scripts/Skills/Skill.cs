using UnityEngine;

public enum Key
{
    Skill1, Skill2, Skill3,
}
public class Skill : MonoBehaviour
{
    [Header("Visual")]
    public Sprite skillIcon;
    public string skillName;
    [Multiline(5)]
    public string skillDescription;
    [Header("Audio")]
    public AudioClip au_skillUse;
    [Header("Game")]
    public float duration;
    [HideInInspector] public float timer;
    public float CD;
    [HideInInspector] public float CDTimer = 0;
    [HideInInspector] public bool isUsing = false;
    public Key key;

    private KeyCode keyCode;

    [Header("Child")]
    //

    protected Animator anim;
    //Patch
    private bool readyHint;

    protected Player player;
    void Start()
    {
        if(GetComponent<Animator>()!= null)
        {
            anim = GetComponent<Animator>();
        }
            
        switch (key)
        {
            case Key.Skill1:
                keyCode = GameController.instance.Skill1;
                break;
            case Key.Skill2:
                keyCode = GameController.instance.Skill2;
                break;
            case Key.Skill3:
                keyCode = GameController.instance.Skill3;
                break;
            default:
                break;
        }

        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if(GameController.instance.isPause)
        {
            return;
        }
        UseSkill();
    }
    /// <summary>
    /// 技能总控，每帧调用，不允许继承
    /// </summary>
    public void UseSkill()
    {

        if (Input.GetKeyDown(keyCode) && CDTimer < 0)
        {
            player.onSkillChange?.Invoke();
            readyHint = true;
            Enter();
            timer = duration;
            CDTimer = CD;
            isUsing = true;
        }

        if (isUsing)
        {
            player.onSkillChange?.Invoke();
            timer -= Time.deltaTime;
            Hold();
            if (timer < 0)
            {
                Exit();
                isUsing = false;
            }
        }
        else if (CDTimer >= 0)
        {
            player.onSkillChange?.Invoke();
            CDTimer -= Time.deltaTime;
        }
        else if(readyHint)
        {
            GameController.instance.au.PlayOneShot(GameController.instance.au_skillReady);
            player.onSkillChange?.Invoke();
            readyHint = false;
        }


    }
    /// <summary>
    /// 释放技能瞬间调用
    /// </summary>
    public virtual void Enter()
    {
        GameController.instance.au.PlayOneShot(au_skillUse);
    }
    /// <summary>
    /// 技能结束瞬间调用
    /// </summary>
    public virtual void Exit()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_skillExhaust);
    }
    /// <summary>
    /// 技能生效期间每帧调用
    /// </summary>
    public virtual void Hold()
    {

    }
}
