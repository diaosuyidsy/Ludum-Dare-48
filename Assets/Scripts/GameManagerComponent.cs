using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameManagerComponent : MonoBehaviour
{
    private RunningComponent[] AllAnimals;
    public BeatComponent BeatController;
    private MoveableComponent[] AllMoveables;
    private FMOD.Studio.EventInstance BGMEV;
    private bool _isMuted;

    private void Awake()
    {
        AllMoveables = Resources.FindObjectsOfTypeAll<MoveableComponent> ();
        AllAnimals = Resources.FindObjectsOfTypeAll<RunningComponent> ();
    }

    public void OnStart () 
    {
//        BGMEV = RuntimeManager.CreateInstance ("event:/BGM");
//        BGMEV.start ();
//        BGMEV.setVolume (_isMuted ? 1f : 0f);
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
//        BGMEV.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
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

    public void ToggleBGM(bool isOn)
    {
        _isMuted = isOn;
        BGMEV.setVolume (_isMuted ? 1f : 0f);
    }
}
