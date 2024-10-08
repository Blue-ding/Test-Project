using UnityEngine;

public class Talent_Regeneration : Talent
{
    [SerializeField] private float healCD;
    private float timer;

    public override void Effect()
    {
        base.Effect();
        if (GameController.instance.player.GetComponentInChildren<Talent_Regeneration>() != null)
        {
            GameController.instance.player.GetComponentInChildren<Talent_Regeneration>().healCD /= 2f;
        }
        else
        {
            Instantiate(this, GameController.instance.player.transform);
        }

    }

    private void Awake()
    {
        timer = healCD;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = healCD;
            GameController.instance.player.BeHeal(1);
        }
    }

}
