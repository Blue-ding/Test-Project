using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Debug")]
    public bool debug;
    [Header("UI_Game")]
    public UI_Game GameUI;
    public UI_Pause PauseUI;
    public UI_LevelUpPanel LevelUpPanel;
    public UI_End EndUI;
    [Header("GameStats")]
    public int score;
    [HideInInspector] public float timer;
    [HideInInspector] public int difficulty;//begin with 1;
    public List<CharData> charDatas;
    public List<Talent> talents;
    private List<Talent> talentPool;
    public static CharData chosenChar;
    public Dictionary<Weapon, List<Bullet>> bulletPools = new();
    public Player player;
    [Header("Enemy Prefabs")]
    public GameObject exp;
    public List<GameObject> stillEnemies;
    public List<GameObject> enemies;
    [Header("Cursor")]
    public Texture2D defCursor;
    public Texture2D aimCursor;
    [Header("FX")]
    public Material defMat;
    public Material FlashFX;
    [Header("Music")]
    public AudioClip mus_main;
    public AudioClip mus_game;
    [Header("Sounds")]
    public AudioSource au;
    [Space]
    public AudioClip au_UIClick;
    public AudioClip au_win;
    public AudioClip au_lose;
    [Space]
    public List<AudioClip> au_moveOnGrass;
    public AudioClip au_gainExp;
    public AudioClip au_levelUp;
    public AudioClip au_getTalent;
    public AudioClip au_beHit;
    public AudioClip au_beHeal;
    [Space]
    public AudioClip au_explode;
    [Space]
    public List<AudioClip> au_enemyBeHit;
    public AudioClip au_enemyDead;
    public AudioClip au_enemyExplode;
    [Space]
    public AudioClip au_skillReady;
    public AudioClip au_skillExhaust;
    [Header("HotKeys")]
    public KeyCode Fire;
    public KeyCode Reload;
    public KeyCode Skill1;
    public KeyCode Skill2;
    public KeyCode Skill3;
    [Header("Mouse")]
    public Vector3 worldMousePos;
    public Vector3 relaMouseDir;
    public Quaternion relaMouseRot;

    //Patch
    private bool startEquipmentBugFixer;
    private bool isUpLvel;
    [HideInInspector] public bool isPause;

    public float timeTick;
    public System.Action onTimeTick;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

    }

    private void Start()
    {
        if (!player.Frozen)
        {
            Initiate();
        }
        else
        {
            UIInitiate();
        }

    }
    private void Update()
    {
        //
        if (player.Frozen)
        {
            return;
        }
        //其后为游戏内内容

        if (!startEquipmentBugFixer)
        {
            player.ReFreshEquipment();
            player.EventInitiate();
            startEquipmentBugFixer = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }



        if (timeTick > 0)
        {
            timeTick -= Time.deltaTime;
        }
        else
        {
            onTimeTick?.Invoke();
            timeTick = 1;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            if (difficulty < 19)
            {
                difficulty++;
                timer = 60;
            }
            else
            {
                End(true);
            }
        }


        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        relaMouseDir = (worldMousePos - player.transform.position).normalized;
        relaMouseRot = Quaternion.LookRotation(Vector3.forward, relaMouseDir);


        //debug
        Debuging();

    }

    /// <summary>
    /// 初始化一场新游戏，不允许在其他场景调用，进入游戏场景会自动调用
    /// </summary>
    public void Initiate()
    {
        au.clip = mus_game;
        au.Play();

        Time.timeScale = 1;
        timer = 60;
        difficulty = 0;
        player.Frozen = false;

        talentPool = new();
        foreach (Talent talent in talents)
        {
            for (int i = 0; i < talent.maxAmount; i++)
            {
                talentPool.Add(talent);
            }
        }

        for (int i = 0; i < player.transform.childCount; i++)
        {
            Destroy(player.transform.GetChild(i).gameObject);
        }
        Instantiate(chosenChar.skill, player.transform);
        Instantiate(chosenChar.defWeapon, player.transform);
        player.transform.position = Vector2.zero;
        player.speed = chosenChar.speed;
        player.maxHP = chosenChar.maxHP;
        player.atk = chosenChar.atk;

        player.currentHP = player.maxHP;
        player.exp = 0;
        player.level = 1;

        player.ReFreshEquipment();

        player.EventInitiate();

        SetAimCursor(true);

        StartCoroutine(SpanEnemy());
    }
    /// <summary>
    /// 初始化除游戏场景外其他GameController
    /// </summary>
    public void UIInitiate()
    {
        au.clip = mus_main;
        au.Play();

        Time.timeScale = 1;
        SetAimCursor(false);

        StartCoroutine(UISpanEnemy());

    }

    /// <summary>
    /// 设置鼠标指针是否为准心
    /// </summary>
    /// <param name="activity"></param>
    public void SetAimCursor(bool activity)
    {
        if (activity)
        {
            Cursor.SetCursor(aimCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    /// <summary>
    /// 简易闪烁特效，适用于不连续闪烁且图像不透明度为1
    /// </summary>
    /// <param name="im"></param>
    /// <returns></returns>
    public IEnumerator EasyFlash(Image im)
    {
        im.material = FlashFX;
        yield return new WaitForSeconds(.1f);
        im.material = defMat;
    }
    /// <summary>
    /// 选择转往胜利或失败结算画面
    /// </summary>
    /// <returns></returns>
    public void End(bool WinOrLose)
    {
        au.Stop();

        EndUI.WinOrLose = WinOrLose;

        EndUI.gameObject.SetActive(true);
    }
    /// <summary>
    /// 持续生成一般怪物，用于主界面
    /// </summary>
    public IEnumerator UISpanEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];

            Enemy enemyScript = Instantiate(enemy).GetComponent<Enemy>();
            enemyScript.gameObject.transform.position = new Vector3(-35, 15) + 2 * Camera.main.orthographicSize * (Vector3)Random.insideUnitCircle.normalized;
        }
    }
    /// <summary>
    /// 持续生成一般怪物，Initiate方法中启动
    /// </summary>
    public IEnumerator SpanEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (difficulty < 15)
            {
                if (Random.Range(5 + difficulty, 20) < 15)
                {
                    continue;
                }
            }
            GameObject enemy;
            while (true)
            {
                enemy = enemies[Random.Range(0, enemies.Count)];
                if (enemy.GetComponent<Enemy>().difficulty > difficulty)
                {
                    continue;
                }
                break;
            }

            Enemy enemyScript = Instantiate(enemy).GetComponent<Enemy>();
            enemyScript.level = (int)(difficulty - 2 + Random.Range(0.5f, 2.2f));
            enemyScript.gameObject.transform.position = player.transform.position + 2 * Camera.main.orthographicSize * (Vector3)Random.insideUnitCircle.normalized;
            if (enemyScript.level < 1)
            {
                enemyScript.level = 1;
            }


            if (player.Frozen)
            {
                break;
            }
        }
    }
    /// <summary>
    /// 生成静止怪物，由Tilemap调用
    /// </summary>
    public void SpanStillEnmey(Vector3 position)
    {
        GameObject enemy;
        if (Random.Range(0, 1500) < (1500 - difficulty - 10))
        {
            return;
        }
        while (true)
        {
            enemy = stillEnemies[Random.Range(0, stillEnemies.Count)];
            if (enemy.GetComponent<Enemy>().difficulty > difficulty)
            {
                continue;
            }
            break;
        }
        Enemy enemyScript = Instantiate(enemy).GetComponent<Enemy>();
        enemyScript.level = (int)(difficulty - 2 + Random.Range(0.5f, 2.2f));
        enemyScript.gameObject.transform.position = position;

    }
    /// <summary>
    /// 随机选择池中一个天赋
    /// </summary>
    /// <returns></returns>
    public Talent RollTalent()
    {
        Talent theTalent = null;
        int tryTimes = 0;
        while (tryTimes < 10)
        {
            tryTimes++;
            theTalent = talentPool[Random.Range(0, talentPool.Count)];
            if (theTalent.availableDifficulty > difficulty)
            {
                continue;
            }
            break;
        }
        return theTalent;
    }
    /// <summary>
    /// 依据等级计算分数
    /// </summary>
    /// <returns></returns>
    public int FinalScore()
    {
        int score = 0;
        for (int i = 0; i < player.level - 1; i++)
        {
            score += player.maxExp[i];
        }
        score += player.exp;
        return score;
    }
    public void PauseGame()
    {
        isPause = true;
        SetAimCursor(false);
        au.Pause();
        Time.timeScale = 0;
        PauseUI.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        isPause = false;
        if (!isUpLvel)
        {
            SetAimCursor(true);
            Time.timeScale = 1;
        }
        au.Play();
        PauseUI.gameObject.SetActive(false);
    }

    public void LevelUP()
    {
        isPause = true;
        isUpLvel = true;
        SetAimCursor(false);
        Time.timeScale = 0;
        LevelUpPanel.gameObject.SetActive(true);
    }

    public void FinishLevelUP()
    {
        isPause = false;
        isUpLvel = false;
        SetAimCursor(true);
        Time.timeScale = 1;
        LevelUpPanel.gameObject.SetActive(false);
    }

    public void Win()
    {
        isPause = true;
        au.PlayOneShot(au_win);
        player.Frozen = true;
        Time.timeScale = 0;
        SetAimCursor(false);
        GameUI.gameObject.SetActive(false);
    }

    public void Lose()
    {
        isPause = true;
        au.PlayOneShot(au_lose);
        player.Frozen = true;
        Time.timeScale = 0;
        SetAimCursor(false);
        GameUI.gameObject.SetActive(false);
    }

    private void Debuging()
    {
        if (!debug)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            player.BeHeal(10);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            difficulty++;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            timer -= 10;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (Skill item in player.skills)
            {
                item.CDTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            player.GainExp(player.maxExp[player.level - 1] - player.exp);
        }
    }
}
