using UnityEngine;
using UnityEngine.UI;

public class SelectItemButton : ElasticButton
{
    public override void OnButtonPressed()
    {
        if (!M.inventory.selectedItem)return;
        M.game.player.SetHandItem(M.inventory.selectedItem);
    }
}