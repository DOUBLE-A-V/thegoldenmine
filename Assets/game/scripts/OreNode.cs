using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OreNode : Breakable
{

    public SpriteRenderer sprite;

    public List<string> drop;
    public string oreName;
    public int quality;

    public void SpawnOreDrop(string oreDropName)
    {
        OreDrop drop = Instantiate(M.prefabs.oreDrops.Find(x => x.oreDropName == oreDropName));
        drop.transform.position = transform.position;
        M.game.oreDrops.Add(drop);
    }

    public override void OnDeath()
    {
        foreach (string oreDropName in drop)
        {
            SpawnOreDrop(oreDropName);
        }
    }
}