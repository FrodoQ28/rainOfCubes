using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, ICubeDestroyable
{
    public event Action<Vector3> Destroyed;

    [SerializeField] private string _platformTag = "Platform";
    [SerializeField] private float _minYToReturn = -10f;
    [SerializeField] private float _maxLifeTimeWithoutHit = 10f;

    private ColorChanger _colorChanger;
    private bool _isOnDestroy;
    private bool _hasTouchedPlatform;
    private int _minTimeToDestroy = 2;
    private int _maxTimeToDestroy = 5;

    private void Awake()
    {
        _colorChanger = new ColorChanger();
    }

    private void OnEnable()
    {
        _isOnDestroy = false;
        _hasTouchedPlatform = false;
        _colorChanger.SetDefaultColor(this);

        StartCoroutine(CheckOutOfBounds());
    }

    private void OnDisable()
    {
        Destroyed = null;
    }

    private void Update()
    {
        if (_hasTouchedPlatform == false && transform.position.y < _minYToReturn && _isOnDestroy == false)
        {
            _isOnDestroy = true;
            Destroyed?.Invoke(transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isOnDestroy)
            return;

        if (collision.gameObject.CompareTag(_platformTag))
        {
            _hasTouchedPlatform = true;
            _isOnDestroy = true;
            StartCoroutine(DestroyAfterDelay(Random.Range(_minTimeToDestroy, _maxTimeToDestroy)));
            _colorChanger.ChangeColor(this);
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroyed?.Invoke(transform.position);
    }

    private IEnumerator CheckOutOfBounds()
    {
        yield return new WaitForSeconds(_maxLifeTimeWithoutHit);

        if (_isOnDestroy == false)
        {
            _isOnDestroy = true;
            Destroyed?.Invoke(transform.position);
        }
    }
}