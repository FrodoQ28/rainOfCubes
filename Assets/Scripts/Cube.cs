using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private bool _isOnDestroy;
    private int _minTimeToDestroy = 2;
    private int _maxTimeToDestroy = 5;

    public event Action<Cube> DestroingCube;

    private void Awake()
    {
        _colorChanger = new ColorChanger();
    }

    private void OnEnable()
    {
        _isOnDestroy = false;

        _colorChanger.SetDefaultColor(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Plane plane))
        {
            if (_isOnDestroy == false)
            {
                _isOnDestroy = true;

                StartCoroutine(WaitOfDestroy(Random.Range(_minTimeToDestroy, _maxTimeToDestroy)));

                _colorChanger.ChangeColor(this);
            }
        }
    }

    private IEnumerator WaitOfDestroy(float delay)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delay);

        yield return waitTime;

        DestroingCube?.Invoke(this);
    }
}
