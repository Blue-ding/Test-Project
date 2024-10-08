using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_End : MonoBehaviour
{
    public Image mask;
    public Transform EndPanel;
    public TextMeshProUGUI tmpTitle;
    public TextMeshProUGUI tmpScore;

    [HideInInspector] public bool WinOrLose;
    private bool hasEnd;
    private void OnEnable()
    {
        hasEnd = false;
        if (WinOrLose)
        {
            tmpTitle.text = "Your Win!";
            tmpTitle.color = Color.white;
            mask.color = new Color(1, 1, 1, 0);
        }
        else
        {
            tmpTitle.text = "Your are Dead!";
            tmpTitle.color = Color.red;
            mask.color = new Color(1, 0, 0, 0);
        }
        tmpScore.text = "Score: " + GameController.instance.FinalScore();
        EndPanel.gameObject.SetActive(false);
        StartCoroutine(GotoEnd());

    }

    private void Update()
    {
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, mask.color.a + Time.deltaTime * 2);
        if(mask.color.a > .99f &&!hasEnd)
        {
            hasEnd = true;
            if (WinOrLose)
            {
                GameController.instance.Win();
            }
            else
            {
                GameController.instance.Lose();
            }
        }
    }

    private IEnumerator GotoEnd()
    {
        yield return new WaitForSecondsRealtime(2);
        EndPanel.gameObject.SetActive(true);
    }
    public void Restart()
    {
        GameController.instance.player.Frozen = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
        SceneManager.LoadScene("MainScene");
    }

}
