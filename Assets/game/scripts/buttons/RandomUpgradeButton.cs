using UnityEngine;
using UnityEngine.UI;

public class RandomUpgradeButton : ElasticButton
{
    public Image image;
    public override void OnButtonPressed()
    {
        StartCoroutine(M.upgrades.GiveRandomUpgrade());
    }
}
