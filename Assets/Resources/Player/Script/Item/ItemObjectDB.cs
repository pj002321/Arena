using Arena.InvenSystem.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.InvenSystem.item
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
    public class ItemObjectDB : ScriptableObject
    {
        public ItemObject[] itemObjects;

        public void OnValidate()
        {
            for (int i = 0; i < itemObjects.Length; ++i)
            {
                itemObjects[i].data.id = i;
            }
        }
    }
}