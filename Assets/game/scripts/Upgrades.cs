using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Upgrades : MonoBehaviour
{
    public UpgradesButton toggleButton;
    public RandomUpgradeButton randomUpgradeButton;
    public TMP_Text goldText;

    public TMP_Text statsText;
    public List<string> upgradeNames;

    public int goldRequired = 3;
    public bool choosing = false;
    public Image bg;

    public bool shown = false;

    public List<UpgradeSelectButton> upgradeButtons;

    public void Start()
    {
        UpdateGoldText();
        UpdateStats();
    }

    public void UpdateStats()
    {
        statsText.text = 
        "speed: " + (int)(M.game.player.speedRatio * 100) + "%\n" + 
        "collecting radius: " + (int)(M.game.player.collectingRadiusRatio * 100) + "%\n" + 
        "mining radius: " + (int)(M.game.player.miningRadiusRatio * 100) + "%\n" + 
        "items damage: " + (int)(M.game.itemDamageRatio * 100) + "%\n" + 
        "item barrels quality: " + (int)(M.game.itemBarrelsQualityRatio * 100) + "%\n" + 
        "ores quality: " + (int)(M.game.oresQualityRatio * 100) + "%\n" + 
        "items speed: " + (int)(M.game.itemDelayRatio * 100) + "%\n";
    }

    public void UpdateGoldText()
    {
        if (choosing || !shown) return;
        float savea = goldText.alpha;
        goldText.text = M.game.gold + "/" + goldRequired;
        if (M.game.gold >= goldRequired) goldText.color = Color.green;
        else goldText.color = Color.white;
        goldText.alpha = savea;

        if (M.game.gold >= goldRequired)
        {
            randomUpgradeButton.active = true;
            randomUpgradeButton.image.color = Color.white;
        } else
        {
            randomUpgradeButton.active = false;
            randomUpgradeButton.image.color = new Color(0.6f, 0.6f, 0.6f, 1);
        }
    }

    public void Show()
    {
        bg.DOKill();
        bg.DOFade(0.5f, 0.5f).SetEase(Ease.OutExpo);

        shown = true;
        statsText.DOKill();
        statsText.DOFade(1, 0.5f).SetEase(Ease.OutExpo);

        randomUpgradeButton.transform.DOKill();
        randomUpgradeButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);

        goldText.DOKill();
        goldText.DOFade(1, 0.5f).SetEase(Ease.OutExpo);

        if (choosing)
        {
            foreach (UpgradeSelectButton btn in upgradeButtons)
            {
                btn.transform.DOKill();
                btn.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
                btn.transform.DOLocalMove(btn.defaultPos, 0.5f).SetEase(Ease.OutExpo);
                btn.active = true;
            }
        } else
        {
            UpdateGoldText();
        }
    }

    public void Hide()
    {
        shown = false;
        statsText.DOKill();
        statsText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);

        randomUpgradeButton.transform.DOKill();
        randomUpgradeButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        randomUpgradeButton.active = false;

        goldText.DOKill();
        goldText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);

        bg.DOKill();
        bg.DOFade(0, 0.5f);

        foreach (UpgradeSelectButton btn in upgradeButtons)
        {
            btn.transform.DOKill();
            btn.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
            btn.transform.DOLocalMove(upgradeButtons[1].transform.localPosition, 0.5f).SetEase(Ease.OutExpo);
            btn.active = false;
        }
    }

    public void ApplyUpgrade(string upgrade, int change)
    {
        switch (upgrade)
        {
            case "speed":
                M.game.player.speedRatio += change/100f;
                break;
            case "collecting radius":
                M.game.player.collectingRadiusRatio += change/100f;
                break;
            case "mining radius":
                M.game.player.miningRadiusRatio += change/100f;
                break;
            case "items damage":
                M.game.itemDamageRatio += change/100f;
                break;
            case "item barrels quality":
                M.game.itemBarrelsQualityRatio += change/100f;
                break;
            case "ores quality":
                M.game.oresQualityRatio += change/100f;
                break;
            case "items speed":
                M.game.itemDelayRatio += change/100f;
                break;
        }
    }

    public IEnumerator GiveRandomUpgrade()
    {
        yield return null;
        M.game.gold -= goldRequired;
        M.game.UpdateGoldText();
        randomUpgradeButton.active = false;
        randomUpgradeButton.transform.DOKill();
        randomUpgradeButton.transform.localScale = Vector3.one * 0.8f;
        randomUpgradeButton.transform.DOScale(1, 1f).SetEase(Ease.OutElastic, 0.5f);
        choosing = true;

        goldText.text = "choose one";

        foreach (UpgradeSelectButton btn in upgradeButtons)
        {
            btn.transform.DOKill();
            btn.transform.localPosition = upgradeButtons[1].transform.localPosition;
            btn.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            btn.transform.DOLocalMove(btn.defaultPos, 0.5f).SetEase(Ease.OutExpo);

            btn.active = true;
        }

        string upgradeName = "";
        int upgradeValue = 0;

        for (int i = 0; i < 512; i++)
        {
            foreach (UpgradeSelectButton btn in upgradeButtons)
            {
                upgradeName = upgradeNames[Random.Range(0, upgradeNames.Count)];
                upgradeValue = Random.Range(20, 35);
                btn.upgrade = upgradeName;
                btn.upgradeValue = upgradeValue;
                btn.text.text = upgradeName + " +" + upgradeValue + "%";
                btn.icon.sprite = M.prefabs.upgradeIcons.Find(x => x.iconName == upgradeName).sprite;
            }
            bool repeats = false;
            for (int j = 0; j < 3; j++)
            {
                if (upgradeButtons.FindAll(x => x.upgrade == upgradeButtons[j].upgrade).Count > 1) repeats = true;
            }
            if (!repeats) break;
        }

        ApplyUpgrade(upgradeName, upgradeValue);
        UpdateStats();
        M.soundSystem.PlaySound("upgrade");

        goldRequired = Mathf.CeilToInt(goldRequired * 1.25f);
    }
}