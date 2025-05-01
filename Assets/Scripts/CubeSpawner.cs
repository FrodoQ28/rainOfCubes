using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _platform;

    private ObjectPool<GameObject> _pool;
    private ColorChanger _colorChanger;
    private float _repeatRate = 0.5f;
    private int _poolCapacity = 20;
    private int _poolMaxSize = 20;
    private int _minTimeToDestroy = 2;
    private int _maxTimeToDestroy = 5;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _colorChanger = new ColorChanger();
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    private void GetCube() =>
        _pool.Get();

    private void ActionOnGet(GameObject cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.SetActive(true);

        _colorChanger.SetDefaultColor(cube);

        if (cube.TryGetComponent(out Cube cubeComponent))
            cubeComponent.OnDestroy += CubeDestroyAndPaint;
    }

    private Vector3 GetSpawnPosition()
    {
        float spawnHeight = 15;

        float minX = -15f;
        float maxX = 15f;

        float minZ = -15f;
        float maxZ = 15f;

        Vector3 position = new Vector3(Random.Range(minX, maxX), spawnHeight, Random.Range(minZ, maxZ));

        return position;
    }

    private void CubeDestroyAndPaint(GameObject cube)
    {
        _colorChanger.ChangeColor(cube);

        StartCoroutine(ReleaseToPool(cube));
    }

    private IEnumerator ReleaseToPool(GameObject cube)
    {
        WaitForSeconds waitTime = new WaitForSeconds(Random.Range(_minTimeToDestroy, _maxTimeToDestroy));

        yield return waitTime;

        _pool.Release(cube);

        if (cube.TryGetComponent(out Cube cubeComponent))
            cubeComponent.OnDestroy -= CubeDestroyAndPaint;
    }
}
