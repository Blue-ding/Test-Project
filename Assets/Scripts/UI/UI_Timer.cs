using TMPro;
using UnityEngine;

public class UI_Timer : UI_Game
{
    public TextMeshProUGUI tmpTime;
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.instance.onTimeTick += Tick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.instance.onTimeTick -= Tick;
    }

    private void Tick()
    {
        tmpTime.text = (19 - GameController.instance.difficulty).ToString() + ":" + string.Format("{0:D2}", (int)GameController.instance.timer);
    }
}
