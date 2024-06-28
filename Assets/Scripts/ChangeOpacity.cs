using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOpacity : MonoBehaviour
{
    public float targetOpacity = 0.5f; // Set your desired opacity (0 is fully transparent, 1 is fully opaque)

    void Start()
    {
        // Get the Renderer component from the object
        Renderer renderer = GetComponent<Renderer>();

        // Ensure the object has a Renderer and a material
        if (renderer != null && renderer.material != null)
        {
            // Set the material's rendering mode to Transparent
            renderer.material.SetFloat("_Mode", 3);
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;

            // Set the material color with the desired opacity
            Color color = renderer.material.color;
            color.a = targetOpacity;
            renderer.material.color = color;
        }
        else
        {
            Debug.LogError("Renderer or Material not found on the object.");
        }
    }
}
