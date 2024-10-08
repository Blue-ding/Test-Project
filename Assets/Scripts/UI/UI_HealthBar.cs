using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : UI_Game
{
    [SerializeField] private GameObject heartPrefab;
    private List<GameObject> hearts = new();
    private int currentHP;
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.instance.player.onHPChange += RefreshUI;
        currentHP = GameController.instance.player.currentHP;
    }

    protected override void OnDisable()
    {
        GameController.instance.player.onHPChange -= RefreshUI;
    }

    private void RefreshUI()
    {
        if (hearts == null || hearts.Count != GameController.instance.player.maxHP)
        {
            int count = hearts.Count;
            for (int i = 0; i < GameController.instance.player.maxHP; i++)
            {
                count--;
                if (count < 0)
                {
                    CreateHeart();
                }
            }
            for (int i = 0;i < count; i++)
            {
                DestoryHeart();
            }
        }

        int currentCount = currentHP - GameController.instance.player.currentHP;
        currentHP = GameController.instance.player.currentHP;

        for (int i = 0; i < GameController.instance.player.currentHP; i++)
        {
            currentCount --;
            hearts[i].GetComponent<Animator>().SetBool("isHealthy", true);
            hearts[i].GetComponent<Animator>().Play("Health",0,0);
        }
        for (int i = GameController.instance.player.currentHP;  i < GameController.instance.player.maxHP;  i++)
        {
            hearts[i].GetComponent<Animator>().SetBool("isHealthy", false);
            hearts[i].GetComponent<Animator>().Play("Health_Damaged", 0, 0);
            if (currentCount > 0)
            {
                currentCount--;
                StartCoroutine(GameController.instance.EasyFlash(hearts[i].GetComponent<Image>()));
            }
        }
    }

    private void CreateHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform);
        hearts.Add(newHeart);
    }
    private void DestoryHeart()
    {
        Destroy(hearts[^1]);
        hearts.RemoveAt(hearts.Count - 1);
    }
}
