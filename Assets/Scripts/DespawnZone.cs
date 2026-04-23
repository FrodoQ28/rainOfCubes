using UnityEngine;

public class DespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IForceDestroyable>(out IForceDestroyable destroyable))
            destroyable.ForceDestroy();
    }
}