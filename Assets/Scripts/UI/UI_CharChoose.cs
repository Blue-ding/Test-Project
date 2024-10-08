using UnityEngine;
using UnityEngine.UI;

public class UI_CharChoose : MonoBehaviour
{
    [SerializeField] private Transform charChooseButtons;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Button startButton;
    public UI_Tooltip tooltip;
    private void OnEnable()
    {
        for (int i = 0; i < GameController.instance.charDatas.Count; i++)
        {
            UI_CharChooseButton button = Instantiate(buttonPrefab, charChooseButtons).GetComponent<UI_CharChooseButton>();
            button.data = GameController.instance.charDatas[i];
            button.tooltip = tooltip;
            button.startButton = startButton;
            button.Initiate();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < charChooseButtons.childCount; i++)
        {
            Destroy(charChooseButtons.GetChild(i).gameObject);
        }
    }
}
