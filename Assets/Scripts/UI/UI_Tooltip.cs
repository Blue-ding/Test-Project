using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private Image gacha;
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpDescription;
    [SerializeField] private Image skillIm;
    [SerializeField] private TextMeshProUGUI tmpSkillName;
    [SerializeField] private UI_TinyToolTipTrigger skillTP;
    [SerializeField] private Image weaponIm;
    [SerializeField] private TextMeshProUGUI tmpWeaponName;
    [SerializeField] private UI_TinyToolTipTrigger weaponTP;

    [HideInInspector] public CharData data;

    private void OnEnable()
    {
        ClearTooltip();
    }

    public void RefreshTooltip()
    {
        tmpName.text = data.charName;
        gacha.sprite = data.gacha;
        gacha.color = Color.white;
        tmpDescription.text = data.charDescription;

        skillIm.sprite = data.skill.skillIcon;
        skillIm.color = Color.white;
        tmpSkillName.text = data.skill.skillName;
        skillTP.data = data;
        skillTP.type = TooltipType.skill;

        weaponIm.sprite = data.defWeapon.weaponIcon;
        weaponIm.color = Color.white;
        tmpWeaponName.text = data.defWeapon.weaponName;
        weaponTP.data = data;
        weaponTP.type = TooltipType.weapon;
    }

    public void ClearTooltip()
    {
        tmpName.text = "Who?";
        gacha.color = Color.clear;
        tmpDescription.text = "Choose a character to see more";

        skillIm.color = Color.clear;
        tmpSkillName.text = "";
        skillTP.type = TooltipType.none;

        weaponIm.color = Color.clear;
        tmpWeaponName.text = "";
        weaponTP.type = TooltipType.none;
    }
}
