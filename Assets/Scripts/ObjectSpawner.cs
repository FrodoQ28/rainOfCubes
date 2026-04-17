using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] protected T _prefab;
    [SerializeField] protected SpawnAreaPlane _spawnArea;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _spawnedText;
    [SerializeField] private TextMeshProUGUI _createdText;
    [SerializeField] private TextMeshProUGUI _activeText;

    [Header("Pool Settings")]
    [SerializeField] private float _repeatRate = 0.5f;
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 20;

    protected ObjectPool<T> _pool;
    protected int _spawnedCount;

    public event Action<Vector3> DestroyedAt;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: obj => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    protected virtual void Start()
    {
        StartCoroutine(SpawnRoutine());
        InvokeRepeating(nameof(UpdateStats), 0f, 0.5f);
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (true)
        {
            Spawn();

            yield return wait;
        }
    }

    public virtual void Spawn()
    {
        T obj = _pool.Get();
        obj.transform.position = _spawnArea.GetRandomPositionInside();
        _spawnedCount++;
    }

    public virtual void SpawnAt(Vector3 position)
    {
        Vector3 flatPosition = new Vector3(position.x, _spawnArea.transform.position.y + 15f, position.z);

        if (_spawnArea.IsPointInside(flatPosition) == false)
            return;

        T obj = _pool.Get();
        obj.transform.position = flatPosition;
        _spawnedCount++;
    }

    protected virtual void OnGet(T obj)
    {
        obj.gameObject.SetActive(true);

        if (obj is ICubeDestroyable destroyable)
            destroyable.Destroyed += position => HandleDestroyed(obj, position);
    }

    protected virtual void OnRelease(T obj) =>
        obj.gameObject.SetActive(false);

    private void HandleDestroyed(T obj, Vector3 position)
    {
        DestroyedAt?.Invoke(position);

        _pool.Release(obj);
    }

    protected virtual void UpdateStats()
    {
        if (_spawnedText != null)
            _spawnedText.text = $"Всего: {_spawnedCount}";

        if (_createdText != null)
            _createdText.text = $"Создано: {_pool.CountAll}";

        if (_activeText != null)
            _activeText.text = $"На сцене: {_pool.CountActive}";
    }

    public void ReleaseToPool(T obj)
    {
        _pool.Release(obj);
    }
}