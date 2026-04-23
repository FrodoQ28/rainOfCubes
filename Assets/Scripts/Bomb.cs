using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bomb : MonoBehaviour, IDestroyableByPosition, IExplodable, IForceDestroyable
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 1000f;

    private Renderer _renderer;
    private Material _material;
    private Rigidbody _rigidbody;
    private float _lifeTime;

    public event Action<Vector3> Destroyed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _material = _renderer.material;
    }

    private void OnEnable()
    {
        _lifeTime = Random.Range(2f, 5f);
        ResetAlpha();
        StartCoroutine(FadeAndExplode());
    }

    private void OnDisable() =>
        Destroyed = null;

    private void ResetAlpha()
    {
        Color color = _material.color;
        color.a = 1f;
        _material.color = color;
    }

    private IEnumerator FadeAndExplode()
    {
        float elapsed = 0f;

        while (elapsed < _lifeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / _lifeTime);

            Color color = _material.color;
            color.a = alpha;
            _material.color = color;

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody == _rigidbody)
                continue;

            if (hit.TryGetComponent<IExplodable>(out var explodable))
                explodable.ApplyExplosion(transform.position, _explosionForce, _explosionRadius);
        }

        Destroyed?.Invoke(transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    public void ApplyExplosion(Vector3 origin, float force, float radius)
    {
        if (_rigidbody == null)
            return;

        _rigidbody.AddExplosionForce(force, origin, radius);
    }

    public void ForceDestroy() =>
        Destroyed?.Invoke(transform.position);
}