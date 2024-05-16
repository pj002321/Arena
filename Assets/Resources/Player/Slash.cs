using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.EffectSystem
{
    public class Slash : MonoBehaviour
    {
        public ParticleSystem TrailEffect001;
        public ParticleSystem TrailEffect002;
        public Transform Trail;
        public Transform TrailShort;
        public AudioClip[] Audioclips;
        public AudioSource AudioSources;

        private void Start()
        {
            AudioSources = GetComponent<AudioSource>();
        }
        // Update is called once per frame
        void longTrailInit()
        {
            if (TrailEffect001 != null)
            {
                ParticleSystem effectInstance = Instantiate(TrailEffect001, Trail.transform.position, Trail.rotation);
                effectInstance.Play();
                AudioSources.clip = Audioclips[0];
                AudioSources.loop = false;
                AudioSources.Play();
                Destroy(effectInstance.gameObject, 1f);
            }
        }
        void ShortTrailInit()
        {
            if (TrailEffect002 != null)
            {
                ParticleSystem effectInstance = Instantiate(TrailEffect002, TrailShort.transform.position, TrailShort.rotation);
                effectInstance.Play();
                AudioSources.clip = Audioclips[1];
                AudioSources.loop = false;
                AudioSources.Play();
                Destroy(effectInstance.gameObject, 1f);
            }
        }
    }

}