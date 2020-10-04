using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableLocationsComponent : MonoBehaviour
{
    /// <summary>
    /// Location Transforms and CurrentOccupants share the same index
    /// </summary>
    public Transform[] LocationTransforms;
    public GameObject[] CurrentOccupants;

    public int GetCurrentIndex(GameObject target)
    {
        for(int i = 0; i < CurrentOccupants.Length; i++)
        {
            if(CurrentOccupants[i] == target)
            {
                return i;
            }
        }

        Debug.Assert (false, "Did not find gameobject: " + target.name + " in: " + gameObject.name);
        return 0;
    }

    public void OnDrop(Transform target)
    {
        int index = GetCurrentIndex (target.gameObject);
        CurrentOccupants[index] = null;
        int minIndex = index;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < LocationTransforms.Length; i++)
        {
            Vector3 locationPos = LocationTransforms[i].position;
            float dist = Vector2.Distance (locationPos, target.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                minIndex = i;
            }
        }
        target.position = LocationTransforms[minIndex].position;
        GameObject swapCache = null;
        // Swap if current position has occupants
        if(CurrentOccupants[minIndex] != null)
        {
            swapCache = CurrentOccupants[minIndex];
        }
        CurrentOccupants[minIndex] = target.gameObject;
        if (swapCache != null)
        {
            CurrentOccupants[index] = swapCache;
            swapCache.transform.position = LocationTransforms[index].position;
        }
    }
}
