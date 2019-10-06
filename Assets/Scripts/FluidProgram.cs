using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FluidProgram : MonoBehaviour
{
    // Start is called before the first frame update
    public bool debug = false;
    private float radius = 5f;
    private Material material;
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/Fluid"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        var cam = this.GetComponent<Camera>();
        // Create texture from position of fluid objects
        // Pass that into the material
        var coords = new List<float>();
        var fluidParts = GameObject.FindGameObjectsWithTag("fluid");
        foreach (var f in fluidParts)
        {
            var pos = cam.WorldToScreenPoint(f.transform.position);
            // var pos = f.transform.position;
            // coords.Add(pos.x / cam.pixelWidth);
            // coords.Add(pos.y / cam.pixelHeight);
            coords.Add(pos.x);
            coords.Add(pos.y);
            Debug.DrawLine(f.transform.position, f.transform.position + new Vector3(0.1f, 0, 0), Color.green);
        }

        material.SetFloat("_Radius", radius / cam.pixelWidth);
        material.SetInt("_CircleCount", coords.Count / 2);
        material.SetFloatArray("_Circles", coords);
        if (debug) {
            Graphics.Blit(src, dest);
        } else {
            Graphics.Blit(src, dest, material);
        }
    }
}
