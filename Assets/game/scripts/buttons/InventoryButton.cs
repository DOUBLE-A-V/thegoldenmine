using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : ElasticButton
{
    public bool selected = false;
    public Sprite selectedSprite;
    public Sprite deselectedSprite;
    public Image image;

    public void Select()
    {
        image.sprite = selectedSprite;
        selected = true;
        M.upgrades.toggleButton.Deselect();
        M.inventory.Show();
    }
    public void Deselect()
    {
        image.sprite = deselectedSprite;
        selected = false;
        M.inventory.Hide();
    }

    public override void OnButtonPressed()
    {
        if (selected) Deselect();
        else Select();
    }
}
