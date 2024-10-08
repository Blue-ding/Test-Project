using System.Collections.Generic;
using UnityEngine;

public class UI_LevelUpPanel : MonoBehaviour
{
    [SerializeField] private List<UI_TinyToolTip> tooltips;
    private List<Talent> talents = new();

    private void OnEnable()
    {
        talents = new();
        int tryTimes = 0;
        for (int i = 0; i < 3; i++)
        {
            tryTimes++;
            talents.Add(GameController.instance.RollTalent());
            if (tryTimes < 10 && talents.Find(t => t.name == talents[i].name))
            {
                i--;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            tooltips[i].icon.sprite = talents[i].icon;
            tooltips[i].tmpTitle.text = talents[i].talentName;
            tooltips[i].tmpDescription.text = talents[i].description;

        }
    }

    public void ActiveEffect(int index)
    {
        talents[index].Effect();
        GameController.instance.FinishLevelUP();
    }

    public void Pass()
    {
        GameController.instance.FinishLevelUP();
    }
}
