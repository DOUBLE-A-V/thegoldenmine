using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FrameAnimation : MonoBehaviour
{
    public float fps = 12;
    public List<Sprite> frames;
    public bool playing = false;
    public float timer = 0;
    public int currentFrame = 0;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;

    public void Play(bool fromZero)
    {
        if (fromZero)
        {
            timer = (1 / Time.deltaTime / 12) * Time.deltaTime;
            playing = true;
            spriteRenderer.sprite = frames[0];
            currentFrame = 0;   
        } else
        {
            playing = true;
        }
    }

    public void Stop()
    {
        playing = false;
    }

    public void NextFrame()
    {
        currentFrame++;
        timer = (1 / Time.deltaTime / 12) * Time.deltaTime;
        if (currentFrame == frames.Count)
        {
            if (loop)
            {
                currentFrame = 0;
            } else
            {
                playing = false;
            }
        }

        spriteRenderer.sprite = frames[currentFrame];
    }

    public void Update()
    {
        if (!playing) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            NextFrame();
        }
    }
}
