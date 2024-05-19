using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent_Enemy : MonoBehaviour
{
    #region Variables

    [Header("Sound Info")]
    [Tooltip("Input Attack Sound")]
    public AudioClip[] attackClips;
    [Tooltip("Input Hit Sound")]
    public AudioClip hitClip;
    [Tooltip("Input Die Sound")]
    public AudioClip dieClip;
    public AudioSource audioSources;
    [Header("Effect Info")]
    public ParticleSystem TrailEffect;
    public Transform Trail;
    ParticleSystem effectInstance;

    #endregion Variables

    private void Start()
    {
        audioSources = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (effectInstance != null)
        {
            effectInstance.transform.position = Trail.position;
            effectInstance.transform.rotation = Trail.rotation;
        }
    }

    // This method is called to initialize the trail effect
    void Attack001()
    {
        if (TrailEffect != null)
        {
            effectInstance = Instantiate(TrailEffect, Trail.position, Trail.rotation);
            effectInstance.Play();
            AttackSound(audioSources, attackClips[0]);
            Destroy(effectInstance.gameObject, 1f);
        }
    }
    void Attack002()
    {
        if (TrailEffect != null)
        {
            effectInstance = Instantiate(TrailEffect, Trail.position, Trail.rotation);
            effectInstance.Play();
            AttackSound(audioSources, attackClips[1]);
            Destroy(effectInstance.gameObject, 1f);
        }
    }
    void HitEffect()
    {
        HitSound(audioSources, hitClip);
    }
    void DieEffect()
    {
        DieSound(audioSources, dieClip);
    }
    #region Sound Methods
    void AttackSound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.Play();
    }
    void HitSound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.Play();
    }
    void DieSound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.Play();
    }
    #endregion Sound Methods
}
