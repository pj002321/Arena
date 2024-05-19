using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Arena.UI
{
    public class NPCBattleUI : MonoBehaviour
    {
        #region Varialbes
        private Slider hpSlider;

        [SerializeField]
        private GameObject damageTextPrefab;

        #endregion Varialbes

        #region Properties
        public float MinimumValue
        {
            get => hpSlider.minValue;
            set
            {
                hpSlider.minValue = value;
            }
        }

        public float MaximumValue
        {
            get => hpSlider.maxValue;
            set
            {
                hpSlider.maxValue = value;
            }
        }


        public float Value
        {
            get => hpSlider.value;
            set
            {
                hpSlider.value = value;
            }
        }
        #endregion Properties
        private void Awake()
        {
            hpSlider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            GetComponent<Canvas>().enabled = true;
        }

        private void OnDisable()
        {
            GetComponent<Canvas>().enabled = false;
        }

        public void TakeDamage(int damage)
        {

            if (damageTextPrefab != null)
            {
                GameObject damageTextGO = Instantiate(damageTextPrefab, transform);
                DamageText damageText = damageTextGO.GetComponent<DamageText>();
                if (damageText == null)
                {
                    Destroy(damageTextGO, 2f);
                }

                damageText.Damage = damage;
            }
        }
    }

}