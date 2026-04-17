using UnityEngine;

public class BombSpawner : ObjectSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    protected override void Start()
    {
        InvokeRepeating(nameof(UpdateStats), 0f, 0.5f);

        if (_cubeSpawner != null)
            _cubeSpawner.DestroyedAt += SpawnAt;
    }

    public override void SpawnAt(Vector3 position)
    {
        if (_spawnArea.IsPointInside(position) == false)
            return;

        Bomb obj = _pool.Get();
        obj.transform.position = position;
        _spawnedCount++;
    }

    private void OnDestroy()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.DestroyedAt -= SpawnAt;
    }
}