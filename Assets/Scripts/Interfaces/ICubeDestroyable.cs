using System;
using UnityEngine;

public interface ICubeDestroyable
{
    event Action<Vector3> Destroyed;
}