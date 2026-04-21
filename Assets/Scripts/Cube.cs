using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ColorChanger))]
public class Cube : MonoBehaviour, ICubeDestroyable
{
    public event Action<Vector3> Destroyed;

    [SerializeField] private string PlatformTag = "Platform";
    [SerializeField] private float MinYToReturn = -10f;
    [SerializeField] private float MaxLifeTimeWithoutHit = 10f;
    [SerializeField] private int MinTimeToDestroy = 2;
    [SerializeField] private int MaxTimeToDestroy = 5;

    private ColorChanger _colorChanger;
    private bool _isOnDestroy;
    private bool _hasTouchedPlatform;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
    }

    private void OnEnable()
    {
        _isOnDestroy = false;
        _hasTouchedPlatform = false;
        _colorChanger.SetDefaultColor();
        StartCoroutine(CheckOutOfBounds());
    }

    private void OnDisable()
    {
        Destroyed = null;
    }

    private void Update()
    {
        if (_hasTouchedPlatform == false && _isOnDestroy == false && transform.position.y < MinYToReturn)
        {
            _isOnDestroy = true;
            Destroyed?.Invoke(transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isOnDestroy)
            return;

        if (collision.gameObject.CompareTag(PlatformTag))
        {
            _hasTouchedPlatform = true;
            _isOnDestroy = true;
            StartCoroutine(DestroyAfterDelay(Random.Range(MinTimeToDestroy, MaxTimeToDestroy)));
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
        yield return new WaitForSeconds(MaxLifeTimeWithoutHit);

        if (_isOnDestroy == false)
        {
            _isOnDestroy = true;
            Destroyed?.Invoke(transform.position);
        }
    }
}