using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : UI_Game
{
    [SerializeField] private Image backGround;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite usingSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Image iconBackGround;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI CD;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.instance.player.onSkillChange += RefreshSkillUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.instance.player.onSkillChange -= RefreshSkillUI;
    }

    private void RefreshSkillUI()
    {
        Skill[] skills = GameController.instance.player.skills;
        iconBackGround.sprite = skills[0].skillIcon;
        icon.sprite = skills[0].skillIcon;
        backGround.sprite = normalSprite;

        if (!skills[0].isUsing && skills[0].CDTimer > 0)//冷却期间
        {
            frame.sprite = normalSprite;
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0.5f);
            frame.fillAmount = 0;
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0.5f);
            icon.fillAmount = 1 - skills[0].CDTimer / skills[0].CD;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.7f);
            CD.text = string.Format("{0:N1}s", skills[0].CDTimer);
        }
        else if (skills[0].isUsing)//使用期间
        {
            frame.sprite = usingSprite;
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 1f);
            frame.fillAmount = skills[0].timer / skills[0].duration;
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 1f);
            icon.fillAmount = 1;
            icon.color = new Color(icon.color.r,icon.color.g, icon.color.b, 1f);
            CD.text = "";
        }
        else//可用期间
        {
            frame.sprite = normalSprite;
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 1f);
            frame.fillAmount = skills[0].timer / skills[0].duration;
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 1f);
            icon.fillAmount = 1;
            icon.color = new Color(icon.color.r,icon.color.g, icon.color.b, 1f);
            CD.text = "";
        }
    }
}
