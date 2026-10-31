using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable]
public class TransformFloatTarget
{
    public bool active = false;
    public float floatScale = 1;
    public float floatSpeed = 1;

    public float sincounter = 0;
    
    public float transformChangeDuration = 0;

    public float originValue = 0;
    public float targetValue = 0;
}

public class FloatingObject : MonoBehaviour
{
    public bool active = true;

    public TransformFloatTarget scaleX;
    public TransformFloatTarget scaleY;
    public TransformFloatTarget posX;
    public TransformFloatTarget posY;

    public TransformFloatTarget rotation;

    public void DisableAll()
    {
        scaleX.active = false;
        scaleY.active = false;

        posX.active = false;
        posY.active = false;
        
        rotation.active = false;
    }
    public void EnableAll()
    {
        scaleX.active = true;
        scaleY.active = true;

        posX.active = true;
        posY.active = true;
        
        rotation.active = true;
    }

    public void UpdateSincounter(TransformFloatTarget target)
    {
        if (target.active) target.sincounter += Time.deltaTime * target.floatSpeed;
        if (target.sincounter > 360) target.sincounter = 0;
    }

    public void UpdateSincounters()
    {
        UpdateSincounter(scaleX);
        UpdateSincounter(scaleY);

        UpdateSincounter(posX);
        UpdateSincounter(posY);

        UpdateSincounter(rotation);
    }

    public void CalcValue(TransformFloatTarget target)
    {
        target.targetValue = target.originValue + Mathf.Sin(Mathf.Deg2Rad * target.sincounter) * target.floatScale;
    }

    public void UpdateTransform()
    {
        CalcValue(scaleX);
        CalcValue(scaleY);

        CalcValue(posX);
        CalcValue(posY);

        CalcValue(rotation);


        if (scaleX.active) transform.DOScaleX(scaleX.targetValue, scaleX.transformChangeDuration);
        if (scaleY.active) transform.DOScaleY(scaleY.targetValue, scaleY.transformChangeDuration);

        if (posX.active) transform.DOLocalMoveX(posX.targetValue, posX.transformChangeDuration);
        if (posY.active) transform.DOLocalMoveY(posY.targetValue, posY.transformChangeDuration);

        if (rotation.active) transform.DOLocalRotate(new Vector3(0, 0, rotation.targetValue), rotation.transformChangeDuration);
    }

    public void Update()
    {
        if (!active) return;
        UpdateSincounters();
        UpdateTransform();
    }
}
