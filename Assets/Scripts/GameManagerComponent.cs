using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : MonoBehaviour
{
    public RunningComponent[] AllAnimals;
    public BeatComponent BeatController;

    public void OnStart () 
    {
        for (int i = 0; i < AllAnimals.Length; i++)
        {
            AllAnimals[i].enabled = true;
        }
        BeatController.enabled = true;
    }

    public void OnReset()
    {
        for (int i = 0; i < AllAnimals.Length; i++)
        {
            AllAnimals[i].enabled = false;
        }
        BeatController.enabled = false;
    }
}
