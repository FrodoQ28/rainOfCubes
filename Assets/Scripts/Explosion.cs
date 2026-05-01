using UnityEngine;

public class Explosion
{
    public void Apply(Vector3 origin, float force, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(origin, radius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent<IExplodable>(out var explodable))
            {
                explodable.ApplyExplosion(origin, force, radius);
            }
        }
    }
}