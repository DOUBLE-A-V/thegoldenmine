using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class ItemModifier
{
    public string name;
    public Item parent;
    public float costRatio;


    public virtual void OnCollectThisItem()
    {
        
    }
    public virtual void OnCollectGold()
    {
        
    }
    public virtual void OnHitBreakable(Breakable breakable)
    {
        
    }
    public virtual void OnMinedOre(OreNode ore)
    {
        
    }
    public virtual void OnEquip()
    {
        
    }
}

namespace ItemModifiers
{
    public class Golden : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "golden";
            costRatio = 1.5f;
            parent.durability.maxHealth /= 2;
        }
        public override void OnMinedOre(OreNode ore)
        {
            foreach (string s in new List<string>(ore.drop))
            {
                ore.drop.Add(s);
                ore.drop.Add(s);
            }
        }
    }
    public class Sapphire : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "sapphire";
            parent.durability.maxHealth *= 0.3f;
            costRatio = 2f;
        }

        public override void OnMinedOre(OreNode ore)
        {
            foreach (string s in new List<string>(ore.drop))
            {
                ore.drop.Add(s);
                ore.drop.Add(s);
                ore.drop.Add(s);
            }
        }
    }
    public class Sharpening : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "sharpening";
            parent.damage += 4;
            costRatio = 1.5f;
        }
    }

    public class LongHandle : ItemModifier
    {
        public override void OnEquip()
        {
            M.game.player.miningRadiusRatioModifier = 6;
        }
        public override void OnCollectThisItem()
        {
            name = "long handle";
            costRatio = 1.5f;
        }
    }

    public class HugeWeight : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "huge weight";
            parent.delay *= 1.5f;
            costRatio = 2.5f;
        }

        public override void OnHitBreakable(Breakable breakable)
        {
            foreach(Breakable b in new List<Breakable>(M.game.breakables))
            {
                if (b != breakable &&
                Vector2.Distance(b.transform.position, breakable.transform.position) < 2f)
                {
                    b.Damage(parent.damage);
                }
            }
            M.effects.SpawnCracks(breakable.transform.position, 14);
        }
    }
    
    public class LightWeight : ItemModifier
    {
        public override void OnEquip()
        {
            M.game.player.speedRatioModifier = 1.5f;
        }
        public override void OnCollectThisItem()
        {
            name = "lightweight";
            parent.delay *= 0.75f;
            parent.durability.maxHealth *= 0.75f;

            costRatio = 1.5f;
        }
    }

    public class Melting : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "melting";
            parent.durability.maxHealth *= 0.5f;

            costRatio = 3f;
        }

        public override void OnMinedOre(OreNode ore)
        {
            M.effects.SpawnPuddle(ore.transform.position);
        }
    }

    public class Elastic : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "elastic";
            parent.damage *= 0.8f;
            parent.durability.maxHealth *= 1.5f;

            costRatio = 2f;
        }
    }

    public class GoodAim : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "good aim";
            costRatio = 2f;
        }

        public override void OnHitBreakable(Breakable b)
        {
            if (Random.Range(0, 100) < 30 && b.health.GetHealth() - parent.damage * M.game.itemDamageRatio > 0)
            {
                b.Damage(parent.damage * 3);
                M.effects.SpawnText("critical!", Color.aquamarine, b.transform.position + new Vector3(0, 1, 0), 5);
                if (b.breakableName == "ore")
                {
                    foreach (ItemModifier mod in parent.modifiers)
                    {
                        mod.OnMinedOre((OreNode)b);
                    }
                }
            }
        }
    }

    public class WarrantyCard : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "warranty card";
            parent.durability.maxHealth *= 2;
            costRatio = 1.5f;
        }
    }

    public class Fragile : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "fragile";
            parent.durability.maxHealth *= 0.5f;
            costRatio = 0.5f;
        }
    }
    public class Expensive : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "expensive";
            costRatio = 0.5f;
        }
        public override void OnHitBreakable(Breakable b)
        {
            M.game.gold--;
            M.game.UpdateGoldText();
        }
    }
    public class BadCounter : ItemModifier
    {
        public override void OnCollectThisItem()
        {
            name = "bad counter";
            costRatio = 0.5f;
        }
        public override void OnHitBreakable(Breakable b)
        {
            int n = Random.Range(0, 100);
            if (n < 5)
            {
                parent.durability.damage += (int)(parent.durability.maxHealth/3);
            } else if (n < 20)
            {
                parent.durability.damage += 5;
            } else if (n < 50)
            {
                parent.durability.damage += 3;
            }

            M.game.UpdateHandItemDurability();
        }
    }
}