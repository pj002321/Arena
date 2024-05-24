using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Arena.InvenSystem.item
{
    public enum ItemType : int
    {
        Helmet=0,
        Chest=1,
        Pants=2,
        Boots=3,
        Pauldrons=4,
        Gloves=5,
        LeftWeapon=6,
        RightWeapon=7,
        Food,
        Default
    }

    [CreateAssetMenu(fileName ="New Item",menuName ="Inventory System/Items/New Item")]
    public class ItemObject : ScriptableObject
    {
        #region Variables
        public ItemType type;

        // Multi index item
        public bool stackable;

        public Sprite icon;
        public GameObject modelPrefab;

        public Item data = new Item();
        
        public List<string> boneNames = new List<string>();

        [TextArea(15, 20)]
        public string description;
        #endregion Variables

        // Function called when data changes
        private void OnValidate()
        {
            boneNames.Clear();
            if(modelPrefab==null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            {
                return; 
            }

            SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            var bones = renderer.bones;

            foreach(var t in bones)
            {
                boneNames.Add(t.name);  
            }
        }

        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }
}
