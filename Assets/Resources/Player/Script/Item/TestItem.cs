using Arena.InvenSystem;
using Arena.InvenSystem.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public InventoryObject equipObject;
    public InventoryObject invenObject;
    public ItemObjectDB databaseObject;

    public void AddNewItem()
    {
        if(databaseObject.itemObjects.Length>0)
        {
            ItemObject newItemObject = databaseObject.itemObjects[Random.Range(0,databaseObject.itemObjects.Length-1)];
            Item newItem = new Item(newItemObject);

            invenObject.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        equipObject?.Clear();
        invenObject?.Clear();
    }
}
