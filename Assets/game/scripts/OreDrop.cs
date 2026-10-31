using DG.Tweening;
using UnityEngine;
using System.Collections;

public class OreDrop : MonoBehaviour
{
    public float colSize;
    public int oreValue;
    public string oreDropName;

    public IEnumerator Collect()
    {
        transform.DOMove(M.game.goldText.transform.position - new Vector3(0, 0.6f, 0), 1).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(0.7f);
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        M.game.gold += oreValue;
        M.game.UpdateGoldText();
        Destroy(gameObject, 0.5f);
    }

    public void Start()
    {
        transform.DOJump(transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0), 1, 1, 0.5f).SetEase(Ease.Linear);
        //M.game.oreDrops.Add(this);
    }
}
