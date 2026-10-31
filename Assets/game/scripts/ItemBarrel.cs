using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarrel : Breakable
{
    public float quality = 100;

    public void Start()
    {
        quality = Random.Range(50 * M.game.itemBarrelsQualityRatio, 100 * M.game.itemBarrelsQualityRatio);
    }

    public override void OnDamage()
    {
        transform.DOKill();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        transform.DOScale(defaultScale, 0.5f).SetEase(Ease.OutExpo);
    }
    public override void OnDeath()
    {
        if (M.inventory.maxItems == M.inventory.items.Count)
        {
            M.effects.SpawnText("inventory full!", Color.red, transform.position);
            return;
        }
        List<Item> suitingItems = new List<Item>();
        foreach (Item i in M.prefabs.items)
        {
            if (Mathf.Abs(i.quality - quality) < 70 || true) // HERE CHANGE SOMETHING LATER PLEASE
            {
                suitingItems.Add(i);
            }
        }
        List<ItemModifier> modifiers = new List<ItemModifier>();
        int modifiersCount = Mathf.RoundToInt(Random.Range(0f, 1f * M.game.itemBarrelsQualityRatio));
        for (int i = 0; i < modifiersCount; i ++)
        {
            modifiers.Add(M.prefabs.itemModifiers[Random.Range(0, M.prefabs.itemModifiers.Count)]);
        }
        bool changed = false;
        for (int j = 0; j < 500; j++)
        {
            changed = false;
            for (int i = 0; i < modifiers.Count; i++)
            {
                if (modifiers.FindAll(x => x == modifiers[i]).Count > 1)
                {
                    modifiers.RemoveAt(i);
                    changed = true;
                    break;
                }
            }
            if (!changed) break;
        }
        Item item = Instantiate(suitingItems[Random.Range(0, suitingItems.Count)], M.game.transform);
        item.modifiers = modifiers;
        foreach (ItemModifier mod in item.modifiers)
        {
            Image image = Instantiate(M.prefabs.modifierVisual, item.transform);
            ModifierVisual visual = M.prefabs.modifierVisuals.Find(x => x.modifierName == mod.name);
            image.sprite = visual.sprite;
            image.transform.localPosition = visual.pos;

            mod.parent = item;
            mod.OnCollectThisItem();
        }
        M.inventory.items.Add(item);
        M.game.buttons.Add(item);

        if (M.inventory.toggleButton.selected) M.inventory.UpdateItems();
        else item.transform.localScale = Vector3.zero;
        M.inventory.UpdateItemsCountText();
    }
}
