using UnityEngine;
using UnityEngine.UIElements;

public class Button : MonoBehaviour
{
    public BoxCollider2D col;
    public bool touching = false;
    public bool active = false;
    public bool pressed = false;

    public virtual void OnHover()
    {
        
    }
    public virtual void OnNoHover()
    {
        
    }
    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnPressed()
    {
        
    }
    public virtual void OnReleased()
    {
        
    }

    public void UpdateButton()
    {
        if (!active) return;
        if (col.OverlapPoint(M.cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (!touching) OnHover();
            touching = true;
            if (Input.GetMouseButtonDown(0))
            {
                OnPressed();
                pressed = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnReleased();
                pressed = false;
            }
        } else
        {
            if (touching) 
            {
                touching = false;
                OnNoHover();
                OnReleased();
            }
        }
    }
}
