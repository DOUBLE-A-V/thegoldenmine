using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
}

public class SoundSystem : MonoBehaviour
{
    public AudioSource audioSourcePrefab;
    public List<AudioSource> audioSourcesPool;
    public List<Sound> sounds;

    public void PlaySound(string soundName)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == soundName)
            {
                bool played = false;
                foreach (AudioSource audioSource in audioSourcesPool)
                {
                    if (audioSource.isPlaying) continue;
                    played = true;
                    audioSource.clip = sound.audioClip;
                    audioSource.Play();
                    break;
                }
                if (!played)
                {
                    AudioSource newSource = Instantiate(audioSourcePrefab, M.cam.transform);
                    newSource.clip = sound.audioClip;
                    newSource.Play();
                    audioSourcesPool.Add(newSource);
                }
            }
        }
    }
}
