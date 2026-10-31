using UnityEngine;
using UnityEngine.UI;

public class UpgradesButton : ElasticButton
{
    public bool selected = false;
    public Sprite selectedSprite;
    public Sprite deselectedSprite;
    public Image image;

    public void Select()
    {
        image.sprite = selectedSprite;
        selected = true;
        M.inventory.toggleButton.Deselect();
        M.upgrades.Show();
    }
    public void Deselect()
    {
        image.sprite = deselectedSprite;
        selected = false;
        M.upgrades.Hide();
    }

    public override void OnButtonPressed()
    {
        if (selected) Deselect();
        else Select();
    }
}
