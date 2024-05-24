using Arena.InvenSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;



namespace Arena.InvenSystem
{
    public static class MouseData
    {
        public static InventoryUI interfaceMouseIsOver;
        public static GameObject slotHoveredOver;
        public static GameObject tempItemBeingDragged;
    }
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        public InventoryObject inventoryObject;
        private InventoryObject previousInventoryObject;

        public Dictionary<GameObject, Inventory_Slot> slotUIs = new Dictionary<GameObject, Inventory_Slot>();

        private void Awake()
        {
            // Slot UIs
            CreateSlotUIs();

            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].parent = inventoryObject;
                inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
            }

            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }

        protected virtual void Start()
        {
            for (int i = 0; i < inventoryObject.Slots.Length; ++i)
            {
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
            }
        }

        public abstract void CreateSlotUIs();

        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (!trigger)
            {
                Debug.LogWarning("No EventTrigger Component Found!");
                return;
            }

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void OnPostUpdate(Inventory_Slot slot)
        {
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.itemObject.icon;
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
        }

        public void OnEnterInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
        }

        public void OnExitInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = null;
        }

        public void OnEnterSlot(GameObject go)
        {
            MouseData.slotHoveredOver = go;
            MouseData.interfaceMouseIsOver = go.GetComponentInParent<InventoryUI>();
        }

        public void OnExitSlot(GameObject go)
        {
            MouseData.slotHoveredOver = null;
        }

        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemBeingDragged = CreateDragImage(go);
        }
        private GameObject CreateDragImage(GameObject go)
        {
            if (slotUIs[go].item.id<0)
            {
                return null;
            }

            GameObject dragImageGo = new GameObject();

            RectTransform rectTransform = dragImageGo.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            dragImageGo.transform.SetParent(transform.parent);
            Image image = dragImageGo.AddComponent<Image>();
            image.sprite = slotUIs[go].itemObject.icon;
            image.raycastTarget = false;

            dragImageGo.name = "Drag Image";
            return dragImageGo;
        }
        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemBeingDragged == null)
            {
                return;
            }

            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemBeingDragged);

            if(MouseData.interfaceMouseIsOver == null)
            {
                slotUIs[go].RemoveItem();
            }
            else if(MouseData.slotHoveredOver)
            {
                Inventory_Slot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
                inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
            }
        }
    }
}
