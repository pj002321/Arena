using Arena.InvenSystem;
using Arena.InvenSystem.item;
using Arena.InvenSystem.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Arena.InvenSystem.Equipment
{
    public class PlayerEquiipment : MonoBehaviour
    {
        public InventoryObject equipment;
        private EquipmentCombiner combiner;
        private ItemInstances[] itemInstances = new ItemInstances[14];
        public ItemObject[] defaultItemObjects = new ItemObject[14];

        private void Awake()
        {
            combiner = new EquipmentCombiner(gameObject);
            for (int i = 0; i < equipment.Slots.Length; i++)
            {
                equipment.Slots[i].OnPreUpdate += OnRemoveItem;
                equipment.Slots[i].OnPostUpdate += OnEquipItem;
            }
        }
        private void Start()
        {
            foreach (Inventory_Slot slot in equipment.Slots)
            {
                OnEquipItem(slot);
            }
        }
        private void OnRemoveItem(Inventory_Slot slot)
        {
            ItemObject itemObject = slot.itemObject;
            if(itemObject == null)
            {
                RemoveItemBy(slot.allowedItems[0]);
                return;
            }

            if(slot.itemObject.modelPrefab != null)
            {
                RemoveItemBy(slot.allowedItems[0]);
            }
        }
        private void RemoveItemBy(ItemType type)
        {
            int index = (int)type;
            if (itemInstances[index] != null)
            {
                Destroy(itemInstances[index].gameObject);
                itemInstances[index]=null;
            }
        }
        private void OnEquipItem(Inventory_Slot slot)
        {
            ItemObject itemObject = slot.itemObject;
            if (itemObject == null)
            {
                EquipDefaultItemBy(slot.allowedItems[0]);
                return;
            }

            int index = (int)slot.allowedItems[0];
            switch (slot.allowedItems[0])
            {
                case ItemType.Helmet:
                case ItemType.Boots:
                case ItemType.Pants:
                case ItemType.Pauldrons:
                case ItemType.Chest:
                case ItemType.Gloves:
                    itemInstances[index] = EquipSkinnedItem(itemObject);
                    break;

                case ItemType.LeftWeapon:
                case ItemType.RightWeapon:
                case ItemType.Ring:
                case ItemType.Sheild:
                    itemInstances[index] = EquipMeshItem(itemObject);
                    break;

            }
            if (itemInstances[index] != null)
            {
                itemInstances[index].name = slot.allowedItems[0].ToString();
            }

        }
        private void EquipDefaultItemBy(ItemType type)
        {
            int index = (int)type;
            ItemObject itemObject = defaultItemObjects[index];
            switch (type)
            {
                case ItemType.Helmet:
                case ItemType.Boots:
                case ItemType.Pants:
                case ItemType.Pauldrons:
                case ItemType.Chest:
                case ItemType.Gloves:
                    itemInstances[index] = EquipSkinnedItem(itemObject);
                    break;

                case ItemType.LeftWeapon:
                case ItemType.RightWeapon:
                case ItemType.Ring:
                case ItemType.Sheild:
                    itemInstances[index] = EquipMeshItem(itemObject);
                    break;

            }
        }
        private ItemInstances EquipSkinnedItem(ItemObject itemObject)
        {
            if (itemObject == null)
            {
                return null;
            }

            Transform itemTransform = combiner.AddLimb(itemObject.modelPrefab, itemObject.boneNames);
            ItemInstances instance = new ItemInstances();
            if (itemTransform != null)
            {
                instance.itemTransforms.Add(itemTransform);
                return instance;
            }
            return null;
        }
        private ItemInstances EquipMeshItem(ItemObject itemObject)
        {
            if (itemObject == null)
            {
                return null;
            }
            Transform[] itemTransform = combiner.AddMesh(itemObject.modelPrefab);
            if (itemTransform.Length > 0)
            {
                ItemInstances instances = new ItemInstances();
                instances.itemTransforms.AddRange(itemTransform.ToList<Transform>());

                return instances;
            }
            return null;
        }
      
    }
}
