using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : ElasticButton
{
    public Health durability;
    public Image image;
    public Vector3 targetPos;

    public List<ItemModifier> modifiers = new List<ItemModifier>();
    public float quality = 100;
    public string itemName;
    public float damage;
    public float delay;
    public int index = 0;
    public int baseCost;
    public bool selected = false;
    [TextArea(3, 9999999)]
    public string description;

    public void UpdatePosition()
    {
        targetPos = M.inventory.itemsStart.localPosition + new Vector3((index % 3) * M.inventory.itemsSpacing,
             Mathf.Floor(index / 3) * -M.inventory.itemsSpacing, 0);
        transform.DOLocalMove(targetPos, 1).SetEase(Ease.OutExpo);
    }

    public void Select()
    {
        if (M.inventory.selectedItem)
        {
            M.inventory.selectedItem.Deselect();
        }
        selected = true;
        M.inventory.selectedItem = this;
        string modifiersDescription = "";
        foreach (ItemModifier mod in modifiers)
        {
            modifiersDescription += M.prefabs.modifierDescriptions.Find(x => x.modifier == mod.name).description + "\n";
        }
        M.descriptionManager.ShowDescription(M.descriptionManager.CalcText((M.game.player.handItem == this ? "<size=50>- SELECTED -</size>\n" : "") + description + "\n" + modifiersDescription, new List<CustomTag>()
        {
            new CustomTag{name="{durability}", text="" + (int)durability.GetHealth()},
            new CustomTag{name="{maxDurability}", text="" + (int)durability.maxHealth},
            new CustomTag{name="{damage}", text="" + damage},
            new CustomTag{name="{delay}", text="" + delay}
        }));
        M.inventory.OnSelectedItem();
    }
    public void Deselect()
    {
        selected = false;
        M.inventory.selectedItem = null;
        M.descriptionManager.HideDescription();
        M.inventory.OnDeselectedItem();
    }

    public int GetCost()
    {
        float cost = baseCost;
        foreach (ItemModifier mod in modifiers)
        {
            cost *= mod.costRatio;
        }
        return (int)(cost * (durability.GetHealth() / durability.maxHealth));
    }

    public override void OnButtonPressed()
    {
        if (selected)
        {
            Deselect();
        } else
        {
            Select();
        }
    }
}
