using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableComponent : MonoBehaviour
{
    public Transform SpriteBody;
    public MoveableLocationsComponent MoveableLocations;
    private bool _dragging;

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
        _dragging = true;
        SpriteBody.localScale = 3f * Vector3.one;
        SpriteBody.GetComponent<SpriteRenderer>().DOFade(0.5f, 0f);
    }

    private void OnMouseUp()
    {
        _dragging = false;
        SpriteBody.localScale = Vector3.one;
        SpriteBody.GetComponent<SpriteRenderer>().DOFade (1f, 0f);
        MoveableLocations.OnDrop (transform);
    }

    private void OnDisable()
    {
        _dragging = false;
    }
}
