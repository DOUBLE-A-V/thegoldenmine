using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public float timer;
    public float delay;
    public float delayTimer;

    public void UpdatePuddle()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
            M.game.puddles.Remove(this);
            Destroy(gameObject);
        }
        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0)
        {
            delayTimer = delay;
            foreach (Breakable b in new List<Breakable>(M.game.breakables))
            {
                if (Vector2.Distance(b.transform.position, transform.position) < 2)
                {
                    b.Damage(0.5f);
                }
            }
        }
    }
}
