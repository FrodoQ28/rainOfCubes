using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectSpawner<T> : ObjectSpawnerBase where T : MonoBehaviour, IDestroyableByPosition
{
    [Header("Spawn Settings")]
    [SerializeField] protected T Prefab;
    [SerializeField] protected PlatformArea SpawnArea;
    [SerializeField] protected float RepeatRate = 0.5f;
    [SerializeField] protected int PoolCapacity = 20;
    [SerializeField] protected int PoolMaxSize = 20;

    protected ObjectPool<T> Pool;
    protected int SpawnedCount;

    public event Action<Vector3> ObjectDestroyedAt;
    public override event Action StatsChanged;

    public override int TotalSpawned => SpawnedCount;
    public override int TotalCreated => Pool?.CountAll ?? 0;
    public override int TotalActive => Pool?.CountActive ?? 0;

    protected virtual void Awake()
    {
        if (Prefab == null)
        {
            Debug.LogError($"{name}: не назначен Prefab для {typeof(T).Name}");
            enabled = false;
            return;
        }

        if (SpawnArea == null)
        {
            Debug.LogError($"{name}: не назначен SpawnAreaPlane");
            enabled = false;
            return;
        }

        Pool = new ObjectPool<T>(
            createFunc: () => Instantiate(Prefab),
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: obj => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize);
    }

    private void HandleDestroyed(T obj, Vector3 position)
    {
        ObjectDestroyedAt?.Invoke(position);
        Pool.Release(obj);
        StatsChanged?.Invoke();
    }

    protected virtual void OnGetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.Destroyed += pos => HandleDestroyed(obj, pos);
        StatsChanged?.Invoke();
    }

    protected virtual void OnReleaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        StatsChanged?.Invoke();
    }

    protected IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(RepeatRate);

        while (true)
        {
            Spawn();
            yield return wait;
        }
    }

    public virtual void Spawn()
    {
        T obj = Pool.Get();
        obj.transform.position = SpawnArea.GetRandomPositionInside();
        SpawnedCount++;
        StatsChanged?.Invoke();
    }

    public virtual void SpawnAt(Vector3 position)
    {
        if (SpawnArea.IsPointInside(position) == false)
            return;

        T obj = Pool.Get();
        obj.transform.position = position;
        SpawnedCount++;
        StatsChanged?.Invoke();
    }

    public void ReturnToPool(T obj)
    {
        Pool.Release(obj);
        StatsChanged?.Invoke();
    }
}