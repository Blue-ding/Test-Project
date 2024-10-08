using UnityEngine;
using UnityEngine.UI;

public class UI_CharChooseButton : MonoBehaviour
{
    [HideInInspector] public CharData data;
    [HideInInspector] public UI_Tooltip tooltip;
    [HideInInspector] public Button startButton;
    [SerializeField] private Sprite normalFrame;
    [SerializeField] private Sprite chosenFrame;
    [SerializeField] private Image frameIm;
    [SerializeField] private Image gachaIm;
    private static System.Action onActive;

    private void OnEnable()
    {
        onActive += Disactive;
    }

    private void OnDisable()
    {
        onActive -= Disactive;
    }

    public void Initiate()
    {
        gachaIm.sprite = data.gacha;
    }

    public void Disactive()
    {
        frameIm.sprite = normalFrame;
    }

    public void Active()
    {
        onActive?.Invoke();
        GameController.instance.au.PlayOneShot(GameController.instance.au_UIClick);

        frameIm.sprite = chosenFrame;
        startButton.interactable = true;
        tooltip.data = data;
        GameController.chosenChar = data;
        tooltip.RefreshTooltip();
    }
}
