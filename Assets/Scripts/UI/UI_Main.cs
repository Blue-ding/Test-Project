using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Main : MonoBehaviour
{
    [SerializeField] private Transform mainUI;
    [SerializeField] private Transform chooseGameUI;
    public void StartCharChoose()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_UIClick);
        mainUI.gameObject.SetActive(false);
        chooseGameUI.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void Back()
    {
        mainUI.gameObject.SetActive(true);
        chooseGameUI.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_UIClick);
        StopCoroutine(GameController.instance.UISpanEnemy());
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
