using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableComponent : MonoBehaviour
{
    public Transform SpriteBody;
    public MoveableLocationsComponent MoveableLocations;
    public StorageComponent Storage;
    public MusicConfigScriptableObject MusicData;
    private bool _dragging;
    private float _timer;
    public bool InStore;
    private Vector3 _spriteInitialScale;

    private void Awake()
    {
        _spriteInitialScale = new Vector3 (
            SpriteBody.localScale.x,
            SpriteBody.localScale.y,
            SpriteBody.localScale.z);
    }

    private void OnEnable()
    {
        _dragging = false;
    }

    private void Update()
    {
        if (_dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            transform.position = mousePosition;
        }
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        _dragging = true;
        _timer = Time.timeSinceLevelLoad + MusicData.ClickDuration;
        SpriteBody.localScale = 3f * _spriteInitialScale;
        SpriteBody.GetComponent<SpriteRenderer>().DOFade(0.5f, 0f);
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        _dragging = false;
        SpriteBody.localScale = _spriteInitialScale;
        SpriteBody.GetComponent<SpriteRenderer>().DOFade (1f, 0f);
        if (_timer < Time.timeSinceLevelLoad)
        {
            if(InStore)
            {
                Storage.OnTakeOut (gameObject);
                InStore = false;
            }
            else
                MoveableLocations.OnDrop (transform);
        }
        else
        {
            if (InStore)
            {
                Storage.OnTakeOut (gameObject);
                InStore = false;
            }else
            {
                MoveableLocations.CurrentOccupants[MoveableLocations.GetCurrentIndex (gameObject)] = null;
                Storage.OnStore (gameObject);
                InStore = true;
            }
        }
    }

    private void OnDisable()
    {
        _dragging = false;
    }
}
