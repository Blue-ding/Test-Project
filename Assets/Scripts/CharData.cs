using UnityEngine;

[CreateAssetMenu(fileName = "CharData", menuName = "Data/CharData", order = 0)]
public class CharData : ScriptableObject
{
    public int id;
    public Sprite gacha;
    public string charName;
    [Multiline(5)]
    public string charDescription;
    public int atk;
    public int maxHP;
    public int speed;
    public Skill skill;
    public Weapon defWeapon;
}
