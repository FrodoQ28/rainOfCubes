using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    public event Action<GameObject> OnDestroy;

    private bool _isOnDestroy;

    private void OnEnable()
    {
        _isOnDestroy = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Plane plane))
        {
            if (_isOnDestroy == false)
            {
                _isOnDestroy = true;

                OnDestroy?.Invoke(this.gameObject);
            }
        }
    }
}
