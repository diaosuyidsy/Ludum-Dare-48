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
    public MusicConfigScriptableObject MusicData;

    public int GetCurrentIndex(GameObject target)
    {
        for(int i = 0; i < CurrentOccupants.Length; i++)
        {
            if(CurrentOccupants[i] == target)
            {
                return i;
            }
        }

        //Debug.Assert (false, "Did not find gameobject: " + target.name + " in: " + gameObject.name);
        return -1;
    }

    public int GetNearestPreviousOccupants(int thisIndex)
    {
        thisIndex--;
        while(thisIndex >= 0 && CurrentOccupants[thisIndex] == null)
        {
            thisIndex--;
        }
        return thisIndex;
    }

    public void DropToEmpty(Transform target, bool nearby = false)
    {
        int minIndex = -1;
        float minDistance = Mathf.Infinity;
        for(int i = 0; i < CurrentOccupants.Length; i++)
        {
            if(CurrentOccupants[i] == null)
            {
                if(!nearby)
                {
                    CurrentOccupants[i] = target.gameObject;
                    target.position = LocationTransforms[i].position;
                    return;
                }
                if(Vector2.Distance(LocationTransforms[i].position, target.position) < minDistance)
                {
                    minIndex = i;
                    minDistance = Vector2.Distance (LocationTransforms[i].position, target.position);
                }
            }
        }
        if (minIndex == -1) return;
        CurrentOccupants[minIndex] = target.gameObject;
        target.position = LocationTransforms[minIndex].position;
    }

    public void OnDrop(Transform target)
    {
        int index = GetCurrentIndex (target.gameObject);
        if(index != -1)
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
        // Before putting it in place, check to see if it's obstacle and being blocked by other obstacles
        ObstacleComponent obs = target.GetComponent<ObstacleComponent> ();
        if(obs != null)
        {
            if(minIndex != index)
            {
                // Check Previous Obstacles to see blocking
                int lastObstacle = GetNearestPreviousOccupants (minIndex);
                if(lastObstacle != -1)
                {
                    if(CurrentOccupants[lastObstacle].GetComponent<ObstacleComponent>().ObstacleLength > Mathf.Abs(minIndex - lastObstacle))
                    {
                        minIndex = index;
                    }
                }
                // Check Behind Obstacles to see blocking
                for(int i = 0; i < obs.ObstacleLength; i++)
                {
                    if(minIndex + i >= CurrentOccupants.Length || CurrentOccupants[minIndex + i] != null)
                    {
                        minIndex = index;
                        break;
                    }
                }
            }
        }
        if (minIndex == -1)
            minIndex = index;
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
