using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class RunningComponent : MonoBehaviour
{
    public MusicConfigScriptableObject MusicData;
    public Animator Animator;
    public MoveableLocationsComponent MoveableLocations;
    public AnimalType Type;
    public SpriteRenderer SpriteRenderer;

    private FSM<RunningComponent> _runningFSM;
    private Transform _nextLocation;

    private void Awake()
    {
        _runningFSM = new FSM<RunningComponent> (this);
    }

    public void OnEncounterObstacles(ObstacleComponent obstacle)
    {
        print (name + " Encountered: " + obstacle.Type);
        Animator.SetTrigger (obstacle.Type.ToString ());
        RuntimeManager.PlayOneShot ("event:/" + Type.ToString () + "/" + obstacle.Type.ToString () + obstacle.Version);
    }

    public void OnUpdateSegment(Transform nextLocation, bool teleport = false)
    {
        _nextLocation = nextLocation;
        if(teleport)
        {
            Vector3 curPos = transform.position;
            curPos.x = MoveableLocations.LocationTransforms[MoveableLocations.GetCurrentIndex (gameObject)].position.x;
            transform.position = curPos;
        }
    }

    private void Update()
    {
        _runningFSM.Update ();
    }

    private void LateUpdate()
    {
        SpriteRenderer.sortingOrder = (int)(transform.position.y * -10);
    }

    private void OnEnable()
    {
        _runningFSM.TransitionTo<NormalRunState> ();
    }

    private void OnDisable()
    {
        _runningFSM.TransitionTo<QuietState> ();
        if(!GetComponent<MoveableComponent>().InStore)
            transform.position = MoveableLocations.LocationTransforms[MoveableLocations.GetCurrentIndex (gameObject)].position;
    }

    private abstract class RunningState : FSM<RunningComponent>.State
    { }

    private class QuietState : RunningState
    { }

    private class NormalRunState : RunningState
    {
        public override void Update ()
        {
            base.Update ();
            if (Context._nextLocation == null) return;
            float step = Context.MusicData.TrackLength / (Context.MusicData.OneBeatDuration * Context.MusicData.BeatCount) * Time.deltaTime;
            Context.transform.position = Vector3.MoveTowards (Context.transform.position, Context._nextLocation.position, step);
        }
    }

    private class ObstacleState : RunningState
    {
        public override void OnExit ()
        {
            base.OnExit ();
            Context.transform.position = Context._nextLocation.position;
            Context.GetComponent<MoveableComponent> ().SpriteBody.localPosition = Vector3.zero;
        }
    }


}
