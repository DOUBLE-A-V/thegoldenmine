using DG.Tweening;
using UnityEngine;

public class ElasticButton : Button
{
    public Tween scaleTween;

    public virtual void OnButtonPressed()
    {
        
    }

    public override void OnHover()
    {
        if (scaleTween != null) scaleTween.Kill();
        scaleTween = transform.DOScale(1.2f, 1f);
        scaleTween.SetEase(Ease.OutElastic, 0.5f);
    }
    public override void OnNoHover()
    {
        if (scaleTween != null) scaleTween.Kill();
        scaleTween = transform.DOScale(1f, 0.5f);
        scaleTween.SetEase(Ease.OutExpo);
    }

    public override void OnPressed()
    {
        if (scaleTween != null) scaleTween.Kill();
        transform.localScale = Vector3.one * 0.9f;
        scaleTween = transform.DOScale(1.2f, 1f);
        scaleTween.SetEase(Ease.OutElastic, 0.5f);
        OnButtonPressed();
    }
}
