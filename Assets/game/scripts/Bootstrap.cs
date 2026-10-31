using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        M.cam = Camera.main;
        M.game = FindFirstObjectByType<Game>();
        M.soundSystem = FindFirstObjectByType<SoundSystem>();
        M.prefabs = FindFirstObjectByType<PrefabsManager>();
        M.effects = FindFirstObjectByType<Effects>();
        M.upgrades = FindFirstObjectByType<Upgrades>();
        M.inventory = FindFirstObjectByType<Inventory>();
        M.descriptionManager = FindFirstObjectByType<DescriptionManager>();
        M.game.StartNewGame();
    }
}
