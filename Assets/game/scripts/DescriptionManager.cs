using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[Serializable]
public class CustomTag
{
    public string name;
    public string text;
}

public class DescriptionManager : MonoBehaviour
{
    public List<CustomTag> tags;
    public TMP_Text descriptionText;
    public Image descriptionBG;

    public string CalcText(string rawText, List<CustomTag> additionalTags = null)
    {
        string result = rawText;
        foreach (CustomTag tag in tags)
        {
            result = result.Replace(tag.name, tag.text);
        }
        if (additionalTags != null)
        {
            foreach (CustomTag tag in additionalTags)
            {
                result = result.Replace(tag.name, tag.text);
            }
        }
        return result;
    }

    public void ShowDescription(string doneText)
    {
        descriptionText.text = doneText;
        descriptionText.DOKill();
        descriptionText.DOFade(1f, 0.5f).SetEase(Ease.OutExpo);

        descriptionBG.transform.DOKill();
        descriptionBG.transform.localScale = Vector3.zero;
        descriptionBG.transform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
    }
    public void HideDescription()
    {
        descriptionText.DOKill();
        descriptionText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);

        descriptionBG.transform.DOKill();
        descriptionBG.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
    }
}
