using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageComponent : MonoBehaviour
{
    public GameObject[] CurrentStorage;
    private Transform[] StorageLocations;

    public MoveableLocationsComponent MoveableLocations;

    private void Awake()
    {
        StorageLocations = new Transform[transform.childCount];
        if(CurrentStorage.Length == 0)
            CurrentStorage = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            StorageLocations[i] = transform.GetChild (i);
            if (CurrentStorage[i] != null)
                CurrentStorage[i].transform.position = StorageLocations[i].position;
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

    public void OnTakeOut(GameObject target, bool nearby = false)
    {
        for(int i = 0; i < CurrentStorage.Length; i++)
        {
            if(CurrentStorage[i] == target)
            {
                CurrentStorage[i] = null;
            }
        }
        // Move to a random empty place in the moveable locations
        MoveableLocations.DropToEmpty (target.transform, nearby);
    }
}
