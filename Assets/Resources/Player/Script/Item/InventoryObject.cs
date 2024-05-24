using Arena.InvenSystem.item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
namespace Arena.InvenSystem
{

    public enum InterfaceType
    {
        Inventory,
        Equipment,
        Chest

    }

    [CreateAssetMenu(fileName ="New Inventory",menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemObjectDB database;
        public InterfaceType type;

        [SerializeField]
        private Inventory container = new Inventory();

        public Inventory_Slot[] Slots => container.slots;

        public int EmptySlotCount
        {
            get
            {
                int counter = 0;
                foreach(Inventory_Slot slot in Slots)
                {
                    if(slot.item.id<=-1)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }

        public bool AddItem(Item item, int amount)
        {
            Inventory_Slot slot = FindItemInInventory(item);
            if (!database.itemObjects[item.id].stackable || slot == null)
            {
                if (EmptySlotCount <= 0)
                {
                    return false;
                }

                GetEmptySlot().UpdateSlot(item, amount);
            }
            else
            {
                slot.AddAmount(amount);
            }

            return true;
        }
     
        public Inventory_Slot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        public Inventory_Slot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id <=-1);
        }

        public bool IsContainItem(ItemObject itemObject)
        {
            return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
        }

        public void SwapItems(Inventory_Slot itemSlotA, Inventory_Slot itemSlotB)
        {
            if (itemSlotA == itemSlotB)
            {
                return;
            }

            if (itemSlotB.CanPlaceInSlot(itemSlotA.itemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.itemObject))
            {
                Inventory_Slot tempSlot = new Inventory_Slot(itemSlotB.item,itemSlotB.amount);
                itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
                itemSlotA.UpdateSlot(tempSlot.item,tempSlot.amount);
            }
        }
    }

}