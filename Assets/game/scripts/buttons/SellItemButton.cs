using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellItemButton : ElasticButton
{
    public TMP_Text text;
    public void SetCost(int cost)
    {
        text.text = "sell for " + cost;
    }

    public override void OnButtonPressed()
    {
        if (!M.inventory.selectedItem)return;
        M.inventory.SellItem();
    }
}