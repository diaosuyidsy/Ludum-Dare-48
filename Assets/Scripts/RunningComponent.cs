using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningComponent : MonoBehaviour
{
    public MusicConfigScriptableObject MusicData;
    public Animator Animator;
    public MoveableLocationsComponent MoveableLocations;

    private FSM<RunningComponent> _runningFSM;
    private Transform _nextLocation;

    private void Awake()
    {
        _runningFSM = new FSM<RunningComponent> (this);
    }

    public void OnEncounterObstacles(ObstacleType type)
    {
        print (name + " Encountered: " + type);
        Animator.SetTrigger (type.ToString ());
        _runningFSM.TransitionTo<ObstacleState> ();
    }

    public void OnNormalRun()
    {
        if(!_runningFSM.CurrentState.GetType().Equals(typeof(NormalRunState)))
            _runningFSM.TransitionTo<NormalRunState> ();
    }

    public void OnUpdateSegment(Transform nextLocation)
    {
        _nextLocation = nextLocation;
    }

    private void Update()
    {
        _runningFSM.Update ();
    }

    private void OnEnable()
    {
        _runningFSM.TransitionTo<NormalRunState> ();
    }

    private void OnDisable()
    {
        _runningFSM.TransitionTo<QuietState> ();
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
            float step = 2f * Time.deltaTime;
            Context.transform.position = Vector3.MoveTowards (Context.transform.position, Context._nextLocation.position, step);
        }
    }

    private class ObstacleState : RunningState
    {

    }


}
