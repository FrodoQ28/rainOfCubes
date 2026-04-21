using UnityEngine;

public class BombSpawner : ObjectSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void Start()
    {
        if (_cubeSpawner == null)
        {
            Debug.LogError($"{name}: не назначен CubeSpawner в BombSpawner");
            return;
        }

        _cubeSpawner.ObjectDestroyedAt += SpawnAt;
    }

    private void OnDestroy()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.ObjectDestroyedAt -= SpawnAt;
    }
}