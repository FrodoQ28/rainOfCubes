using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnAreaPlane : MonoBehaviour
{
    [SerializeField] private float _spawnHeightOffset = 15f;
    [SerializeField] private float _edgeMargin = 10f;

    private Collider _collider;
    private Bounds _bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    public Vector3 GetRandomPositionInside()
    {
        float minX = _bounds.min.x + _edgeMargin;
        float maxX = _bounds.max.x - _edgeMargin;
        float minZ = _bounds.min.z + _edgeMargin;
        float maxZ = _bounds.max.z - _edgeMargin;

        if (minX > maxX)
        {
            float centerX = _bounds.center.x;
            minX = maxX = centerX;
        }

        if (minZ > maxZ)
        {
            float centerZ = _bounds.center.z;
            minZ = maxZ = centerZ;
        }

        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        float y = _bounds.max.y + _spawnHeightOffset;

        return new Vector3(x, y, z);
    }

    public bool IsPointInside(Vector3 point)
    {
        float minX = _bounds.min.x + _edgeMargin;
        float maxX = _bounds.max.x - _edgeMargin;
        float minZ = _bounds.min.z + _edgeMargin;
        float maxZ = _bounds.max.z - _edgeMargin;

        return point.x >= minX && point.x <= maxX &&
               point.z >= minZ && point.z <= maxZ;
    }
}