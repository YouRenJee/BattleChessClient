using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : UnitySingleton<AudioManager>
{
    private AudioSource BGM = null;
    private AudioSource effect =null;


    public override void Awake()
    {
        base.Awake();

        BGM = gameObject.AddComponent<AudioSource>();
        effect = gameObject.AddComponent<AudioSource>(); ;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {

        BGM.clip = clip;
        BGM.loop = loop;
        BGM.volume = 1f;
        BGM.Play();
    }


    public void StopMusic()
    {
        BGM.Stop();
    }



    public void PlayEffect(AudioClip clip)
    {
        effect.PlayOneShot(clip,1f);
    }

    public void EnableMusic(bool enabled)
    {
        BGM.enabled = enabled;
    }

    public void EnableEffect(bool enabled)
    {

        effect.enabled = enabled;
    }


}
