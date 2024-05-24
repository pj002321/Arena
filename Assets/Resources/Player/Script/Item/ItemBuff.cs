using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
namespace Arena.InvenSystem.item
{

    public enum ChracterAttribute
    {
        Agillity,
        Intellect,
        Stamina,
        Strength
    }
    [Serializable]
    public class ItemBuff : MonoBehaviour
    {
        #region Variables
        public ChracterAttribute stat;
        public int value;

        [SerializeField]
        private int min;

        [SerializeField]
        private int max;
        #endregion Variables

        #region Properties
        public int Min => min;
        public int Max => max;
        #endregion Properties

        #region Methods
        public ItemBuff(int min, int max)
        {
            this.min = min;
            this.max = max;

            GenerateValue();
        }

        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }

        public void AddValue(ref int v)
        {
            v += value;
        }
        #endregion Methods

    }
}