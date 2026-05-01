using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour, IDestroyableByPosition, IExplodable, IForceDestroyable
{
    [SerializeField] private float _maxLifeTimeWithoutHit = 10f;
    [SerializeField] private int _minTimeToDestroy = 2;
    [SerializeField] private int _maxTimeToDestroy = 5;

    private Rigidbody _rigidbody;
    private ColorChanger _colorChanger;
    private bool _isOnDestroy;

    public event Action<Vector3> Destroyed;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _isOnDestroy = false;
        _colorChanger.SetDefaultColor();
        StartCoroutine(CheckOutOfBounds());
    }

    private void OnDisable()
    {
        Destroyed = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isOnDestroy)
            return;

        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _isOnDestroy = true;
            StartCoroutine(DestroyAfterDelay(Random.Range(_minTimeToDestroy, _maxTimeToDestroy)));
            _colorChanger.SetRandomColor();
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

    public void ForceDestroy() =>
        Destroyed?.Invoke(transform.position);

    public void ApplyExplosion(Vector3 origin, float force, float radius)
    {
        if (_rigidbody == null)
            return;

        _rigidbody.AddExplosionForce(force, origin, radius);
    }
}