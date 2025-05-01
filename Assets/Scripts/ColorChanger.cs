using UnityEngine;

public class ColorChanger
{
    public void ChangeColor(GameObject cube)
    {
        Color color = new Color(Random.value, Random.value, Random.value);

        if (cube.TryGetComponent(out Renderer renderer))
            renderer.material.color = color;
    }

    public void SetDefaultColor(GameObject cube)
    {
        Color color = Color.white;

        if (cube.TryGetComponent(out Renderer renderer))
            renderer.material.color = color;
    }
}
