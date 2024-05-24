using Arena.InvenSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arena.InvenSystem
{
    public class StaticInventoryUI : InventoryUI
    {
        public GameObject[] staticSlots = null;

        public override void CreateSlotUIs()
        {
            slotUIs = new Dictionary<GameObject, Inventory_Slot> ();
            for(int i=0;i<inventoryObject.Slots.Length;i++) {

                GameObject go = staticSlots[i];

                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });

                inventoryObject.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObject.Slots[i]);

            }
        }
    }

}