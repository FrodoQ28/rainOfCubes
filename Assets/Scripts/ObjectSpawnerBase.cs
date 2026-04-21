using System;
using UnityEngine;

public abstract class ObjectSpawnerBase : MonoBehaviour
{
    public abstract int TotalSpawned { get; }
    public abstract int TotalCreated { get; }
    public abstract int TotalActive { get; }

    public abstract event Action StatsChanged;
}