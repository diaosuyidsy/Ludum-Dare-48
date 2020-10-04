﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatComponent : MonoBehaviour
{
    public MoveableLocationsComponent ObstacleLocations;
    public MusicConfigScriptableObject MusicData;
    public GameObject[][] ObstaclesPerTrack;
    public MoveableLocationsComponent AnimalLocations;
    public int CurrentSegmentIndex;

    private float _timer;
    private void Awake ()
    {
        // Initialize TrackLocations
        ObstaclesPerTrack = new GameObject[MusicData.TrackCount][];
        for(int i = 0; i < MusicData.TrackCount; i++)
        {
            ObstaclesPerTrack[i] = new GameObject[MusicData.OneTrackSegmentCount];
        }
        int j = 0;
        for (int i = 0; i < ObstacleLocations.CurrentOccupants.Length; i++)
        {
            if(i / MusicData.OneTrackSegmentCount == j)
            {
                ObstaclesPerTrack[j][i % MusicData.OneTrackSegmentCount] = ObstacleLocations.CurrentOccupants[i].gameObject;
            }
            if(i % MusicData.OneTrackSegmentCount == 0 && i != 0)
            {
                j++;
            }
        }
    }

    private void OnEnable()
    {
        _updateSegment (0);
        _timer = Time.timeSinceLevelLoad + MusicData.GetSegmentDuration;
    }

    private void Update()
    {
        if(_timer < Time.timeSinceLevelLoad)
        {
            _timer = Time.timeSinceLevelLoad + MusicData.GetSegmentDuration;
            _updateSegment ((CurrentSegmentIndex + 1) % MusicData.OneTrackSegmentCount);
        }
    }

    private void _updateSegment(int newSegment)
    {
        CurrentSegmentIndex = newSegment;
        // Loop through track and see what each animal should perform
        for(int i = 0; i < MusicData.TrackCount; i++)
        {
            GameObject animal = AnimalLocations.CurrentOccupants[i];
            if (animal == null) continue;
            animal.GetComponent<RunningComponent> ().OnUpdateSegment (ObstacleLocations.LocationTransforms[(i + 1) * CurrentSegmentIndex]);
            GameObject obstacle = ObstaclesPerTrack[i][CurrentSegmentIndex];
            if (obstacle == null)
                animal.GetComponent<RunningComponent> ().OnNormalRun ();
            else
                animal.GetComponent<RunningComponent> ().OnEncounterObstacles (obstacle.GetComponent<ObstacleComponent> ().Type);
        }
    }

    private void OnDisable()
    {

    }
}