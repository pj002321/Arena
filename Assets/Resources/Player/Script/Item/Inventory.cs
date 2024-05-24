using Arena.InvenSystem.item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Arena.InvenSystem
{

    [Serializable]
    public class Inventory
    {
        #region Variables
        public Inventory_Slot[] slots = new Inventory_Slot[30];
        #endregion Variables

        #region Methods
        public void Clear()
        {
            foreach(Inventory_Slot slot in slots)
            {
                //slot.UpdateSlot(new Item(), 0);
                slot.RemoveItem();
            }
        }

        public bool IsContain(ItemObject itemObject)
        {
            return IsContain(itemObject.data.id);
        }

        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id==id) != null;
        }
        #endregion Methods
    }

}