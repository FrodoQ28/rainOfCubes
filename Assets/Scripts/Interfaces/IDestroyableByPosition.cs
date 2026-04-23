using System;
using UnityEngine;

public interface IDestroyableByPosition
{
    public event Action<Vector3> Destroyed;
}