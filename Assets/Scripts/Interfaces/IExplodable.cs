using UnityEngine;

public interface IExplodable
{
    public void ApplyExplosion(Vector3 origin, float force, float radius);
}