using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BWProgram : MonoBehaviour
{
    public float intensity;
    private Material material;
    // Start is called before the first frame update
    void Awake()
    {
       material = new Material(Shader.Find("Custom/BW"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (intensity == 0) {
            Graphics.Blit(src, dest);
            return;
        }

        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(src, dest, material);
    }
}
