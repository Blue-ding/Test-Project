using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent : MonoBehaviour
{
    public Sprite icon;
    public string talentName;
    [Multiline(5)]
    public string description;
    public int availableDifficulty;
    public int maxAmount;

    private void OnValidate()
    {
        gameObject.name = talentName;
    }
    public virtual void Effect()
    {
        GameController.instance.au.PlayOneShot(GameController.instance.au_getTalent);
    }
}
