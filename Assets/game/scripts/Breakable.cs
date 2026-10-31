using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour
{
    public bool undying = false;
    public string breakableName;
    public float colSize;
    public Health health;
    public Vector3 defaultScale;
    public string damageEffect;
    public string deathEffect;
    public string damageSoundEffect;
    public string deathSoundEffect;

    public void Awake()
    {
        defaultScale = transform.localScale;
    }

    public virtual void OnDamage()
    {
        
    }
    public virtual void OnDeath()
    {
        
    }
    
    public void Damage(float damage)
    {
        M.soundSystem.PlaySound(damageSoundEffect);
        health.damage += damage * M.game.itemDamageRatio;
        transform.DOKill();
        transform.localScale = defaultScale * 0.8f;
        transform.DOScale(defaultScale, 0.5f).SetEase(Ease.OutExpo);
        M.effects.SpawnEffect(damageEffect, transform.position);
        OnDamage();
        if (health.GetHealth() <= 0)
        {
            Death();
        }
        M.effects.SpawnText("" + damage * M.game.itemDamageRatio, Color.white, transform.position);
    }

    public void Death()
    {
        if (!undying)
        {
            M.soundSystem.PlaySound(deathSoundEffect);
            transform.DOKill();
            transform.localScale = Vector3.zero;
            M.game.breakables.Remove(this);
            M.effects.SpawnEffect(deathEffect, transform.localPosition);
            Destroy(gameObject, 0.5f);
        }

        OnDeath();
    }
}
