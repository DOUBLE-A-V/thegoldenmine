using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectButton : ElasticButton
{
    public TMP_Text text;
    public string upgrade;
    public int upgradeValue;
    public Vector3 defaultPos;
    public Image icon;

    public void Awake()
    {
        defaultPos = transform.localPosition;
    }
    public override void OnButtonPressed()
    {
        M.upgrades.ApplyUpgrade(upgrade, upgradeValue);
        foreach (UpgradeSelectButton upgradeButton in M.upgrades.upgradeButtons)
        {
            upgradeButton.active = false;
            upgradeButton.transform.DOKill();
            upgradeButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        }
        M.upgrades.choosing = false;
        M.upgrades.UpdateGoldText();
    }
}
