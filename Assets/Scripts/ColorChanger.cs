using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetDefaultColor()
    {
        if (_renderer != null)
            _renderer.material.color = Color.white;
    }

    public void SetRandomColor()
    {
        if (_renderer == null)
            return;

        Color color = new Color(Random.value, Random.value, Random.value, 1f);
        _renderer.material.color = color;
    }
}