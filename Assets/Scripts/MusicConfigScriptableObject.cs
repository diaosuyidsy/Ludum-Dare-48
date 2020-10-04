using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MusicConfigScriptableObject : ScriptableObject
{
    public float OneBeatDuration;
    public int BeatCount = 4;
    public int SegmentCount = 8;
    public int TrackCount = 4;

    public float GetSegmentDuration => OneBeatDuration / SegmentCount;
    public int OneTrackSegmentCount => BeatCount * SegmentCount;

    public float ClickDuration = 0.1f;
    public float TrackLength = 18f;
}
