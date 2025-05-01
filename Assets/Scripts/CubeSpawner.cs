using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    private ObjectPool<Cube> _pool;
    private float _repeatRate = 0.5f;
    private int _poolCapacity = 20;
    private int _poolMaxSize = 20;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetCube(_repeatRate));
    }

    private IEnumerator GetCube(float delay)
    {
        bool isRunning = true;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (isRunning)
        {
            _pool.Get();

            yield return wait;
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);

        cube.DestroingCube += ReleaseToPool;
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

    private void ReleaseToPool(Cube cube)
    {
        _pool.Release(cube);

        cube.DestroingCube -= ReleaseToPool;
    }
}
