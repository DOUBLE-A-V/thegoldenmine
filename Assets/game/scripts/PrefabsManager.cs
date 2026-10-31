using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class ModifierDescription
{
    public string modifier;
    [TextArea(3, 9999)]
    public string description;
}

[Serializable]
public class UpgradeIcon
{
    public Sprite sprite;
    public string iconName;
}

[Serializable]
public class ModifierVisual
{
    public string modifierName;
    public Sprite sprite;
    public Vector2 pos = Vector2.zero;
}

public class PrefabsManager : MonoBehaviour
{
    public List<OreDrop> oreDrops;
    public List<OreNode> oreNodes;
    public List<Item> items;
    public List<ItemModifier> itemModifiers;
    public List<ModifierDescription> modifierDescriptions;
    public List<UpgradeIcon> upgradeIcons;
    public List<ModifierVisual> modifierVisuals;
    public SpriteRenderer cracksPrefab;
    public Image modifierVisual;
    public Puddle puddle;

    private void Awake()
    {
        itemModifiers = new List<ItemModifier>()
        {
            new ItemModifiers.Golden(){name="golden"},
            new ItemModifiers.Sapphire(){name="sapphire"},
            new ItemModifiers.Sharpening(){name="sharpening"},
            new ItemModifiers.LongHandle(){name="long handle"},
            new ItemModifiers.HugeWeight(){name="huge weight"},
            new ItemModifiers.LightWeight(){name="lightweight"},
            new ItemModifiers.Melting(){name="melting"},
            new ItemModifiers.Elastic(){name="elastic"},
            new ItemModifiers.GoodAim(){name="good aim"},
            new ItemModifiers.WarrantyCard(){name="warranty card"},
            new ItemModifiers.Fragile(){name="fragile"},
            new ItemModifiers.Expensive(){name="expensive"},
            new ItemModifiers.BadCounter(){name="bad counter"},
        };
    }
}
