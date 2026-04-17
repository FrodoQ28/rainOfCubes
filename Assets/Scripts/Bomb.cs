using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bomb : MonoBehaviour, ICubeDestroyable
{
    public event Action<Vector3> Destroyed;

    [SerializeField] private float _explosionRadius = 25f;
    [SerializeField] private float _explosionForce = 1000f;

    private Renderer _renderer;
    private Material _material;
    private Rigidbody _rigidbody;
    private float _lifeTime;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _lifeTime = Random.Range(2f, 5f);
        SetupTransparentMaterial();
        StartCoroutine(FadeAndExplode());
    }

    private void OnDisable()
    {
        Destroyed = null;
    }

    private void SetupTransparentMaterial()
    {
        if (_material.HasProperty("_Color") == false)
            return;

        _material.SetFloat("_Mode", 3f);
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt("_ZWrite", 0);
        _material.DisableKeyword("_ALPHATEST_ON");
        _material.EnableKeyword("_ALPHABLEND_ON");
        _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _material.renderQueue = 3000;

        Color color = _material.color;
        color.a = 1f;
        _material.color = color;
    }

    private IEnumerator FadeAndExplode()
    {
        float elapsed = 0f;
        bool hasColor = _material.HasProperty("_Color");

        while (elapsed < _lifeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / _lifeTime);

            if (hasColor)
            {
                Color color = _material.color;
                color.a = alpha;
                _material.color = color;
            }

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;

            if (rb == null || rb == _rigidbody)
                continue;

            rb.AddExplosionForce(
                _explosionForce,
                transform.position,
                _explosionRadius);
        }

        Destroyed?.Invoke(transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}