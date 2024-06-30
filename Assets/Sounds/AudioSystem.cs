using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.Audiosystem
{
    public class AudioSystem : MonoBehaviour
    {
        #region Variables
        public AudioSource playeraudioSource;
        public AudioSource enemyaudioSource;
        #endregion Variables
        // Start is called before the first frame update
        void Start()
        {
            playeraudioSource= GetComponent<AudioSource>();
            enemyaudioSource = GetComponent<AudioSource>();
        }

        #region StateSoundMethods
        public void IdleSound(AudioSource audioSources, AudioClip audioClip)
        {
            audioSources.clip = audioClip;
            audioSources.loop = true;
            audioSources.Play();
        }
        public void AttackSound(AudioSource audioSources, AudioClip audioClip)
        {
            audioSources.clip = audioClip;
            audioSources.loop = false;
            audioSources.Play();
        }
        public void HitSound(AudioSource audioSources, AudioClip audioClip)
        {
            audioSources.clip = audioClip;
            audioSources.loop = false;
            audioSources.Play();
        }
        public void DeadSound(AudioSource audioSources, AudioClip audioClip)
        {
            audioSources.clip = audioClip;
            audioSources.loop = false;
            audioSources.Play();
        }
        public void WalkSound(AudioSource audioSources, AudioClip audioClip)
        {
            audioSources.clip = audioClip;
            audioSources.loop = false;
            audioSources.Play();
        }
        #endregion StateSoundMethods
    }
}
