using System;
using System.Collections;
using System.Collections.Generic;
using Arena.InvenSystem.item;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Arena.InvenSystem
{
    [Serializable]
    public class Inventory_Slot 
    {
        #region Variables
        public ItemType[] allowedItems = new ItemType[0];

        [NonSerialized]
        public InventoryObject parent;
        [NonSerialized]
        public GameObject slotUI;

        // An action that calls an event when the item in the slot is updated
        [NonSerialized]
        public Action<Inventory_Slot> OnPreUpdate;
        [NonSerialized]
        public Action<Inventory_Slot> OnPostUpdate;

        public Item item;
        public int amount;
        #endregion Variables
        #region Properties
        public ItemObject itemObject
        {
            get
            {
                return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
            }
        }
        #endregion Properties
        public Inventory_Slot() => UpdateSlot(new Item(), 0);
        public Inventory_Slot(Item item,int amount)=>UpdateSlot(item, amount);

        public void RemoveItem() => UpdateSlot(new Item(), 0);
        public void AddItem(Item item,int amount) => UpdateSlot(item, amount);  

        public void AddAmount(int value) => UpdateSlot(item, amount += value);
       
        public void UpdateSlot(Item item,int amount)
        {
            if (amount <= 0)
            {
                item = new Item();
            }
            OnPreUpdate?.Invoke(this);
            this.item = item;
            this.amount = amount;

            OnPostUpdate?.Invoke(this);

        }

        public bool CanPlaceInSlot(ItemObject itemObject)
        {
            if(allowedItems.Length <=0 || itemObject==null || itemObject.data.id < 0)
            {
                return true;
            }
            foreach(ItemType type in allowedItems)
            {
                if(itemObject.type==type)
                {
                    return true;
                }
            }
            return false;
        }
    }

}