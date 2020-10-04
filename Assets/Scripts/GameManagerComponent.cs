using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : MonoBehaviour
{
    public RunningComponent[] AllAnimals;
    public BeatComponent BeatController;
    private MoveableComponent[] AllMoveables;

    private void Awake()
    {
        AllMoveables = Resources.FindObjectsOfTypeAll<MoveableComponent> ();
    }

    public void OnStart () 
    {
        for (int i = 0; i < AllAnimals.Length; i++)
        {
            AllAnimals[i].enabled = true;
        }
        for(int i = 0; i < AllMoveables.Length; i++)
        {
            AllMoveables[i].enabled = false;
        }
        BeatController.enabled = true;
    }

    public void OnReset()
    {
        for (int i = 0; i < AllAnimals.Length; i++)
        {
            AllAnimals[i].enabled = false;
        }
        for (int i = 0; i < AllMoveables.Length; i++)
        {
            AllMoveables[i].enabled = true;
        }
        BeatController.enabled = false;
    }
}
