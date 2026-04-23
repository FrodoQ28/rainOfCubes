using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlatformArea : MonoBehaviour
{
    [SerializeField] private float SpawnHeightOffset = 15f;
    [SerializeField] private float EdgeMargin = 1f;

    private Collider _collider;
    private Bounds _bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    public Vector3 GetRandomPositionInside()
    {
        float minX = _bounds.min.x + EdgeMargin;
        float maxX = _bounds.max.x - EdgeMargin;
        float minZ = _bounds.min.z + EdgeMargin;
        float maxZ = _bounds.max.z - EdgeMargin;

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
        float y = _bounds.max.y + SpawnHeightOffset;

        return new Vector3(x, y, z);
    }

    public bool IsPointInside(Vector3 point)
    {
        float minX = _bounds.min.x + EdgeMargin;
        float maxX = _bounds.max.x - EdgeMargin;
        float minZ = _bounds.min.z + EdgeMargin;
        float maxZ = _bounds.max.z - EdgeMargin;

        return point.x >= minX && point.x <= maxX &&
               point.z >= minZ && point.z <= maxZ;
    }
}