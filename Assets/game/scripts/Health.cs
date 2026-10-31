using System;
using UnityEngine;

[Serializable]
public class Health
{
    public float damage;
    public float maxHealth;

    public float GetHealth()
    {
        return maxHealth - damage;
    }
}
