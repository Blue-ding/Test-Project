using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TooltipType
{
    none,skill,weapon
}
public class UI_TinyToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UI_TinyToolTip tinyTooltipPrefab;
    [HideInInspector] public CharData data;
    public TooltipType type;
    private UI_TinyToolTip tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(type == TooltipType.none)
        {
            return;
        }

        tooltip = Instantiate(tinyTooltipPrefab,transform.parent.parent);

        if(type == TooltipType.skill)
        {
            tooltip.icon.sprite = data.skill.skillIcon;
            tooltip.tmpTitle.text = data.skill.skillName;
            tooltip.tmpBasicInfo.text = "CD:" + data.skill.CD + "s, Duration:" + data.skill.duration + "s.";
            tooltip.tmpDescription.text = data.skill.skillDescription;
        }
        if(type == TooltipType.weapon)
        {
            tooltip.icon.sprite = data.defWeapon.weaponIcon;
            tooltip.tmpTitle.text = data .defWeapon.weaponName;
            tooltip.tmpBasicInfo.text = "FireCD:" + data.defWeapon.atkCD + "s, DamageRate: " + data.defWeapon.damageRate * 100 + "%.";
            tooltip.tmpDescription.text = data.defWeapon.weaponDescription;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tooltip != null)
        {
            Destroy(tooltip.gameObject);
        }
    }
}
