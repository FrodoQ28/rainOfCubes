using UnityEngine;

public class ColorChanger
{
    public void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            Color color = new Color(Random.value, Random.value, Random.value, 1f);
            renderer.material.color = color;
        }
    }

    public void SetDefaultColor(Cube cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Color.white;
        }
    }
}