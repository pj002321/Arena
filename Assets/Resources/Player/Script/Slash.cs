using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.EffectSystem
{
    public class Slash : MonoBehaviour
    {
        #region Variables
        public ParticleSystem TrailEffect001;
        public ParticleSystem subEffect001;
        public ParticleSystem TrailEffect002;
        public Transform Trail;
        public Transform TrailShort;
        public AudioClip[] Audioclips;
        public AudioSource AudioSources;
        public bool beingSkill = false;
        #endregion Variables
        ParticleSystem effectInstance;
        ParticleSystem subeffectInstance;
        private void Start()
        {
            AudioSources = GetComponent<AudioSource>();
           
        }
        private void Update()
        {
            if (effectInstance != null)
            {
                effectInstance.transform.position = Trail.position;
            }
            if (subeffectInstance != null)
            {
                subeffectInstance.transform.position = Trail.position;
                subeffectInstance.transform.rotation = Trail.rotation;
            }
        }
        // Update is called once per frame
        void longTrailInit()
        {
          
            if (TrailEffect001 != null)
            {
                effectInstance = Instantiate(TrailEffect001, Trail.transform.position, Trail.rotation);
                subeffectInstance = Instantiate(subEffect001, Trail.transform.position, Trail.rotation);
                effectInstance.Play();
                subeffectInstance.Play();
                AudioSources.clip = Audioclips[0];
                AudioSources.loop = false;
                AudioSources.Play();
                Destroy(effectInstance.gameObject, 1f);
                Destroy(subeffectInstance.gameObject, 1f);
            }
        }
        void ShortTrailInit()
        {
            if (TrailEffect002 != null)
            {
                effectInstance = Instantiate(TrailEffect002, TrailShort.transform.position, TrailShort.rotation);
                effectInstance.Play();
                AudioSources.clip = Audioclips[1];
                AudioSources.loop = false;
                AudioSources.Play();
                Destroy(effectInstance.gameObject, 1f);
            }
        }
        void ResetBeingSkill()
        {
            beingSkill = false;
            Debug.Log(beingSkill);
        }
        void startBeingSkill()
        {
            beingSkill = true;
            Debug.Log(beingSkill);
        }
    }
    
}