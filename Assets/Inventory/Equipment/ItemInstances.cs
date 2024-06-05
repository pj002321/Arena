using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances : MonoBehaviour
{
    public List<Transform> itemTransforms = new List<Transform>();
    
    public void OnDestory()
    {
        foreach(Transform item in itemTransforms)
        {
           Destroy(item.gameObject);
        }
    }
}
