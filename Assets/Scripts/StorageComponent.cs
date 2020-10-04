using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageComponent : MonoBehaviour
{
    public GameObject[] CurrentStorage;
    private Transform[] StorageLocations;

    private void Awake()
    {
        StorageLocations = new Transform[transform.childCount];
        CurrentStorage = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            StorageLocations[i] = transform.GetChild (i);
        }
    }

    public void OnStore(GameObject target)
    {
        for(int i = 0; i < CurrentStorage.Length; i++)
        {
            if (CurrentStorage[i] == target)
                return;
        }

        for(int i = 0; i < CurrentStorage.Length; i++)
        {
            if(CurrentStorage[i] == null)
            {
                CurrentStorage[i] = target;
                target.transform.position = StorageLocations[i].position;
                return;
            }
        }
    }

    public void OnTakeOut(GameObject target)
    {
        for(int i = 0; i < CurrentStorage.Length; i++)
        {
            if(CurrentStorage[i] == target)
            {
                CurrentStorage[i] = null;
            }
        }
    }
}
