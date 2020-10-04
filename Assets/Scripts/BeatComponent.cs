using System.Collections;
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

    private void Awake()
    {
        ObstaclesPerTrack = new GameObject[MusicData.TrackCount][];
        for (int i = 0; i < MusicData.TrackCount; i++)
        {
            ObstaclesPerTrack[i] = new GameObject[MusicData.OneTrackSegmentCount];
        }
    }

    private void _initObstacles()
    {
        // Initialize ObstaclesPerTrack
        int j = 0;
        for (int i = 0; i < ObstacleLocations.CurrentOccupants.Length; i++)
        {
            if (i / MusicData.OneTrackSegmentCount == j)
            {
                ObstaclesPerTrack[j][i % MusicData.OneTrackSegmentCount] = ObstacleLocations.CurrentOccupants[i];
            }
            if (i % MusicData.OneTrackSegmentCount == 0 && i != 0)
            {
                j++;
            }
        }
    }

    private void OnEnable()
    {
        _initObstacles ();
        _updateSegment (0);
        _timer = Time.timeSinceLevelLoad + MusicData.GetSegmentDuration;
    }

    private void Update()
    {
        if(_timer < Time.timeSinceLevelLoad)
        {
            _timer = Time.timeSinceLevelLoad + MusicData.GetSegmentDuration;
            _updateSegment ((CurrentSegmentIndex + 1) % MusicData.OneTrackSegmentCount, (CurrentSegmentIndex + 1) % MusicData.OneTrackSegmentCount == 0);
        }
    }

    private void _updateSegment(int newSegment, bool teleport = false)
    {
        CurrentSegmentIndex = newSegment;
        // Loop through track and see what each animal should perform
        for(int i = 0; i < MusicData.TrackCount; i++)
        {
            GameObject animal = AnimalLocations.CurrentOccupants[i];
            if (animal == null) continue;
            animal.GetComponent<RunningComponent> ().OnUpdateSegment (ObstacleLocations.LocationTransforms[i * MusicData.OneTrackSegmentCount + CurrentSegmentIndex], teleport);
            GameObject obstacle = ObstaclesPerTrack[i][CurrentSegmentIndex];
            if (obstacle != null)
                animal.GetComponent<RunningComponent> ().OnEncounterObstacles (obstacle.GetComponent<ObstacleComponent> ().Type);
        }
    }

    private void OnDisable()
    {
        CurrentSegmentIndex = 0;
    }
}
