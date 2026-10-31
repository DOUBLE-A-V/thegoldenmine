using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Game : MonoBehaviour
{
    public TMP_Text goldText;
    public List<OreDrop> oreDrops;
    public Player player;

    public float gold;
    public float spawnOreDelay;
    public float spawnOreTimer;
    public List<Breakable> breakables = new List<Breakable>();
    public List<Button> buttons;
    public ItemBarrel barrelPrefab;

    public float collectTimer = 0;
    public float collectDelay;

    public float itemDamageRatio = 0.2f;
    public float itemBarrelsQualityRatio = 0.2f;
    public float oresQualityRatio = 0.2f;
    public float itemDelayRatio = 0.2f;
    public float spawnBarrelDelay;
    public float spawnBarrelTimer = 0;
    public TMP_Text handItemDurabilityText;
    public Image handItemImage;
    public List<Puddle> puddles;
    public Breakable blockage;

    public TMP_Text mainGoalText;

    public bool paused = false;
    public Image blackscreen;
    public Image endScreen;
    public TMP_Text spaceToReplayText;

    public TMP_Text tutorialText;


    public IEnumerator Win()
    {
        blackscreen.transform.SetAsLastSibling();
        endScreen.transform.SetAsLastSibling();
        spaceToReplayText.transform.SetAsLastSibling();
        
        paused = true;
        yield return new WaitForSeconds(2);
        blackscreen.DOFade(1, 2).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2);
        endScreen.DOFade(1, 2).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        spaceToReplayText.DOFade(1, 1);
        while (true)
        {
            yield return null;
            if (Input.GetKey(KeyCode.Space))
            {
                break;
            }
        }
        endScreen.DOFade(0, 1f);
        spaceToReplayText.DOFade(0, 1f);
        yield return new WaitForSeconds(1);
        paused = false;
        StartNewGame();
    }

    public void UpdateMainGoalText()
    {
        mainGoalText.text = "blockage\n" + blockage.health.GetHealth() + "/" + blockage.health.maxHealth;
    }

    public void StartNewGame()
    {
        blackscreen.DOKill();
        blackscreen.DOFade(0, 1f);

        tutorialText.DOKill();
        tutorialText.DOFade(1, 1f);

        itemDamageRatio = Random.Range(80, 120) / 100f;
        itemBarrelsQualityRatio = Random.Range(80, 120) / 100f;

        oresQualityRatio = Random.Range(80, 120) / 100f;
        itemDelayRatio = Random.Range(80, 120) / 100f;

        player.speedRatio = Random.Range(80, 120) / 100f;
        player.miningRadiusRatio = Random.Range(80, 120) / 100f;
        player.collectingRadiusRatio = Random.Range(80, 120) / 100f;

        gold = 0;
        UpdateGoldText();

        foreach (Item item in M.inventory.items)
        {
            buttons.Remove(item);
            Destroy(item.gameObject);
        }
        M.inventory.items.Clear();

        Item defaultItem = Instantiate(M.prefabs.items.Find(x => x.itemName == "default"), transform);
        defaultItem.transform.localScale = Vector3.zero;
        defaultItem.damage = 1f;
        defaultItem.delay = 1f;
        defaultItem.itemName = "basic item";
        defaultItem.durability.maxHealth = 999999;
        M.inventory.items.Add(defaultItem);
        M.inventory.UpdateItems();
        buttons.Add(defaultItem);

        player.SetHandItem(defaultItem);
        player.transform.position = Vector3.zero;
        player.pickaxe.transform.position = player.transform.position + new Vector3(0.8f, 0, 0);

        blockage.health.damage = 0;
        UpdateMainGoalText();

        foreach (Breakable breakable in new List<Breakable>(breakables))
        {
            if (breakable.breakableName != "blockage")
            {
                breakables.Remove(breakable);
                Destroy(breakable.gameObject);
            }
        }
        foreach (OreDrop oreDrop in oreDrops)
        {
            Destroy(oreDrop.gameObject);
        }
        oreDrops.Clear();
        spawnBarrelTimer = 3;
    }

    public void UpdateHandItemDurability()
    {
        if (!player.handItem) return;
        if (player.handItem == M.inventory.selectedItem)M.inventory.sellItemButton.SetCost(player.handItem.GetCost());
        handItemDurabilityText.text = (int)player.handItem.durability.GetHealth() + "/" + (int)player.handItem.durability.maxHealth;
        if (player.handItem.durability.GetHealth() <= 0)
        {
            //destroy item
            if (M.inventory.selectedItem == player.handItem)
            {
                M.inventory.selectedItem.Deselect();
            }

            M.soundSystem.PlaySound("brokenItem");
            player.pickaxe.sprite = null;
            player.pickaxeHitDelay = int.MaxValue;
            player.pickaxeHitTimer = int.MaxValue;
            handItemDurabilityText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
            handItemImage.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
            buttons.Remove(player.handItem);
            M.inventory.items.Remove(player.handItem);
            Destroy(player.handItem.gameObject);
            M.inventory.UpdateItems();

            player.handItem = null;
        }
    }

    public void UpdateGoldText()
    {
        goldText.transform.DOKill();
        goldText.transform.localScale = Vector3.one * 0.8f;
        goldText.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        goldText.text = gold + "";
        M.upgrades.UpdateGoldText();
    }

    public void UpdateOreDrops()
    {
        List<OreDrop> remove = new List<OreDrop>();
        foreach (OreDrop drop in oreDrops)
        {
            if (Vector2.Distance(drop.transform.position, player.transform.position) < drop.colSize + player.colSize * player.collectingRadiusRatio * player.collectingRadiusRatioModifier)
            {
                if (collectTimer <= 0 || oreDrops.Count > 100)
                {
                    M.soundSystem.PlaySound("pickupCoin");
                    remove.Add(drop);
                    StartCoroutine(drop.Collect());
                    collectTimer = collectDelay;
                } else
                {
                    drop.transform.DOMove(player.transform.position, 1).SetEase(Ease.OutExpo);
                }
            }
        }
        foreach (OreDrop drop in remove)
        {
            oreDrops.Remove(drop);
        }
    }

    public void SpawnOre(string oreName)
    {
        OreNode ore = Instantiate(M.prefabs.oreNodes.Find(x => x.oreName == oreName));
        ore.transform.position = new Vector3(Random.Range(-8.5f, 5f), Random.Range(-4.8f, 4.8f), 0);
        ore.transform.localScale = Vector3.zero;
        ore.transform.DOScale(ore.defaultScale, 0.5f).SetEase(Ease.OutExpo);
        breakables.Add(ore);
    }

    private void UpdateButtons()
    {
        foreach (Button button in new List<Button>(buttons))
        {
            if (button.active) button.UpdateButton();
        }
    }

    public void SpawnBarrel()
    {
        ItemBarrel barrel = Instantiate(barrelPrefab);
        barrel.transform.position = new Vector3(Random.Range(-8.5f, 5f), Random.Range(-4.8f, 4.8f), 0);
        barrel.transform.localScale = Vector3.zero;
        barrel.transform.DOScale(barrel.defaultScale, 0.5f).SetEase(Ease.OutExpo);
        breakables.Add(barrel);
    }
    public void UpdatePuddles()
    {
        foreach (Puddle p in new List<Puddle>(puddles))
        {
            p.UpdatePuddle();
        }
    }

    public void Update()
    {
        if (paused) return;
        UpdatePuddles();
        UpdateButtons();
        collectTimer -= Time.deltaTime;
        UpdateOreDrops();
        spawnOreTimer -= Time.deltaTime;
        if (spawnOreTimer < 0)
        {
            spawnOreTimer = spawnOreDelay / (itemDamageRatio + itemDelayRatio - 1);
            OreNode ore1 = null;
            OreNode ore2 = null;
            float ore1Delta = float.MaxValue;
            foreach (OreNode oreNode in M.prefabs.oreNodes)
            {
                if (Mathf.Abs(oreNode.quality - oresQualityRatio * 100f) < ore1Delta)
                {
                    ore1Delta = Mathf.Abs(oreNode.quality - oresQualityRatio * 100f);
                    ore1 = oreNode;
                }
            }
            float ore2Delta = float.MaxValue;
            foreach (OreNode oreNode in M.prefabs.oreNodes)
            {
                if (Mathf.Abs(oreNode.quality - oresQualityRatio * 100f) < ore2Delta && oreNode != ore1)
                {
                    ore2Delta = Mathf.Abs(oreNode.quality - oresQualityRatio * 100f);
                    ore2 = oreNode;
                }
            }

            int n = Random.Range(0, 100);
            if (n < (1 - ore1Delta / (ore1Delta + ore2Delta)) * 100f)
            {
                SpawnOre(ore1.oreName);
            } else
            {
                SpawnOre(ore2.oreName);
            }
        }

        spawnBarrelTimer -= Time.deltaTime;
        if (spawnBarrelTimer < 0)
        {
            spawnBarrelTimer = spawnBarrelDelay / (itemDamageRatio + itemDelayRatio - 1);
            SpawnBarrel();
        }

        player.UpdatePlayer();
    }
}
