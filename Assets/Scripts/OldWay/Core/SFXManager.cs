using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip[] audios;

    private void Awake()
    {
        source = GetComponent<AudioSource>();    
    }

    public void PlayAndInterrupt(SFX sound)
    {
        source.Stop();
        source.clip = audios[(int)sound];
        source.pitch = Random.Range(.8f, 1.2f);
        source.Play();
    }

    public void PlayNoInterrupt(SFX sound)
    {
        source.clip = audios[(int)sound];
        source.Play();
    }

}

public enum SFX
{
    DragonDeath = 0,
    DragonRoar = 1,
    EmblemExplosion = 2,
    PlayerHurt = 3,
}
