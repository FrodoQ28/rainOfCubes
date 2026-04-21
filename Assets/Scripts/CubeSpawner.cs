public class CubeSpawner : ObjectSpawner<Cube>
{
    private void Start() =>
        StartCoroutine(SpawnRoutine());
}