using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public InventoryButton toggleButton;
    public List<Item> items;
    public Item selectedItem;
    public Transform itemsStart;
    public float itemsSpacing;
    public Image itemHighlight;
    public Image bg;
    public bool shown = false;
    public TMP_Text itemsCountText;

    public SelectItemButton selectItemButton;
    public SellItemButton sellItemButton;
    public int maxItems;


    public void SellItem()
    {
        selectedItem.active = false;
        M.soundSystem.PlaySound("soldItem");

        if (M.game.player.handItem == selectedItem)
        {
            M.game.player.pickaxe.sprite = null;
            M.game.player.pickaxeHitDelay = int.MaxValue;
            M.game.player.pickaxeHitTimer = int.MaxValue;
            M.game.handItemDurabilityText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
            M.game.handItemImage.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
            M.game.player.handItem = null;
        }
        M.game.buttons.Remove(selectedItem);
        M.inventory.items.Remove(selectedItem);
        Destroy(selectedItem.gameObject);
        M.game.gold += selectedItem.GetCost();
        M.game.UpdateGoldText();
        M.inventory.UpdateItems();
    }
    public void Show()
    {
        shown = true;
        bg.DOKill();
        bg.DOFade(0.5f, 0.5f);
        UpdateItems();
        itemsCountText.transform.DOKill();
        itemsCountText.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        UpdateItemsCountText();
    }
    public void UpdateItemsCountText()
    {
        itemsCountText.text = items.Count + "/" + maxItems;
    }
    public void UpdateItems()
    {
        if (!shown) return;
        int count = 0;
        if (selectedItem) selectedItem.Deselect();
        foreach (Item item in items)
        {
            item.active = true;
            item.transform.DOKill();
            item.transform.localPosition = itemsStart.localPosition;
            item.index = count;
            item.UpdatePosition();
            item.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            
            count++;
        }
    }

    public void OnSelectedItem()
    {
        selectItemButton.transform.DOKill();
        selectItemButton.transform.localScale = Vector3.zero;
        selectItemButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        selectItemButton.active = true;

        if (selectedItem.itemName != "basic item")
        {
            sellItemButton.transform.DOKill();
            sellItemButton.transform.localScale = Vector3.zero;
            sellItemButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            sellItemButton.active = true;
            sellItemButton.SetCost(selectedItem.GetCost());
        }

        itemHighlight.transform.DOKill();
        itemHighlight.transform.localScale = Vector3.zero;
        itemHighlight.transform.localPosition = selectedItem.transform.localPosition;
        itemHighlight.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }
    public void OnDeselectedItem()
    {
        itemHighlight.transform.DOKill();
        itemHighlight.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);

        selectItemButton.transform.DOKill();
        selectItemButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        selectItemButton.active = false;

        sellItemButton.transform.DOKill();
        sellItemButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        sellItemButton.active = false;
    }
    
    public void Hide()
    {
        if (toggleButton.selected)
        {
            toggleButton.Deselect();
            return;
        }
        shown = false;
        if (selectedItem)selectedItem.Deselect();
        foreach (Item item in items)
        {
            item.active = false;
            item.transform.DOKill();
            item.transform.DOScale(0, 1.2f).SetEase(Ease.OutExpo);
            item.transform.DOLocalMove(items[items.Count-1].transform.localPosition, 1f).SetEase(Ease.OutExpo);
        }
        bg.DOKill();
        bg.DOFade(0, 0.5f);
        M.descriptionManager.HideDescription();
        itemsCountText.transform.DOKill();
        itemsCountText.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
    }
}
