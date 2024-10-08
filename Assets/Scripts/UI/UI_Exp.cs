using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Exp : UI_Game
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI tmpLevel;
    protected override void OnEnable()
    {
        base.OnEnable();
        slider = GetComponent<Slider>();
        GameController.instance.player.onExpChange += RefreshExpUI;
        GameController.instance.player.onLevelChange += RefreshLevelUI;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.instance.player.onExpChange -= RefreshExpUI;
        GameController.instance.player.onLevelChange -= RefreshLevelUI;

    }

    private void RefreshExpUI()
    {
        if (GameController.instance.player.level == GameController.instance.player.maxExp.Length)
        {
            slider.value = 1;
        }
        else
        {
            slider.value = (float)GameController.instance.player.exp / GameController.instance.player.maxExp[GameController.instance.player.level - 1];
        }
    }
    private void RefreshLevelUI()
    {
        tmpLevel.text = "Level: " + GameController.instance.player.level;
        if (GameController.instance.player.level == GameController.instance.player.maxExp.Length)
        {
            tmpLevel.text = "Level: MAX";
        }
    }

}
