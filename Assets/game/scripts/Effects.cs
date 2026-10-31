using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class Effects : MonoBehaviour
{
    public TMP_Text textPrefab;
    public List<GameObject> effectsPrefabs;

    
    public void SpawnEffect(string effectName, Vector3 at)
    {
        GameObject prefab = effectsPrefabs.Find(x => x.name == effectName);
        if (!prefab)
        {
            Debug.Log("not found effect: " + effectName);
            return;
        }

        prefab = Instantiate(prefab);
        prefab.transform.localPosition = at;
    }

    public TMP_Text SpawnText(string text, Color color, Vector3 pos, int size = 4)
    {
        TMP_Text obj = Instantiate(textPrefab);

        obj.text = text;
        obj.fontSize = size;
        obj.transform.position = pos;
        obj.transform.DOMove(new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(0.5f, 1.5f), 0) + pos, 0.5f).SetEase(Ease.OutExpo);
        obj.color = color;
        obj.DOFade(0, 2f).SetEase(Ease.InExpo);
        Destroy(obj.gameObject, 2f);
        return obj;
    }

    public void SpawnCracks(Vector3 pos, float scale)
    {
        SpriteRenderer s = Instantiate(M.prefabs.cracksPrefab);
        s.transform.position = pos;
        s.transform.localScale = Vector3.one * scale;
        s.transform.DOScale(0, 0.7f).SetEase(Ease.InExpo);
        Destroy(s.gameObject, 0.7f);
    }

    public void SpawnPuddle(Vector3 pos)
    {
        Puddle s = Instantiate(M.prefabs.puddle);
        s.transform.position = pos;
        M.game.puddles.Add(s);
    }
}
