using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Pause : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }
    }
    public void Restart()
    {
        GameController.instance.player.Frozen = false;
        StopAllCoroutines();
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("MainScene");
    }

    public void Continue()
    {
        GameController.instance.ContinueGame();
    }
}
