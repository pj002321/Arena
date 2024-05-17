using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent_Enemy : MonoBehaviour
{
    #region Variables

    public ParticleSystem TrailEffect;
    public Transform Trail;
    public AudioClip[] audioclips;
    public AudioSource audioSources;
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
    void TrailInit()
    {
        if (TrailEffect != null)
        {
            effectInstance = Instantiate(TrailEffect, Trail.position, Trail.rotation);
            effectInstance.Play();
            AttackSound(audioSources, audioclips[0]);
            Destroy(effectInstance.gameObject, 1f);
        }
    }

    #region Sound Methods
    void AttackSound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.Play();
    }
    #endregion Sound Methods
}
