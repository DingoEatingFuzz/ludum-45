using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// INCOMPLETE
// 1. For whatever reason, the projection from texture space to world pixel space just doesn't work at all
// 2. Doing this with textures is a useless task because it's way way way way way too slow. If only shaders made a single sense at all

public class WaterTexture : MonoBehaviour
{
    public Camera GameCamera;

    Texture2D composite;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the texture using the camera dimensions
        var bbox = GameCamera.pixelRect;
        Debug.Log("Camera Box: " + bbox);
        Debug.Log("Screen Box: " + new Rect(0, 0, Screen.width, Screen.height));
        // composite = new Texture2D((int)bbox.width, (int)bbox.height);
        composite = new Texture2D((int)Screen.width, (int)Screen.height);

        this.GetComponent<SpriteRenderer>().sprite = Sprite.Create(composite, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        // Clear the texture
        // TODO: turn into a loop if this is too slow
        composite.SetPixels(Enumerable.Repeat(Color.clear, composite.width * composite.height).ToArray());
        var bbox = new Rect(0, 0, composite.width, composite.height);

        // Reset texture based on current positions of objects
        var fluidParts = GameObject.FindGameObjectsWithTag("fluid");
        foreach (var f in fluidParts)
        {
            var position = GameCamera.WorldToScreenPoint(f.transform.position);
            var texture = f.GetComponent<SpriteRenderer>().sprite.texture;
            var newColors = texture.GetPixels(0, 0, texture.width, texture.height);
            var texturePos = new Vector2(position.x, position.y);
            // var texturePos = new Vector2(position.x + composite.width / 2, position.y + composite.height / 2);

            // TODO: Deal with partially occluded objects
            if (bbox.Contains(texturePos)) {
                // Debug.Log("Yep: " + texturePos);
                composite.SetPixels(
                    (int)texturePos.x,
                    (int)texturePos.y,
                    texture.width,
                    texture.height,
                    newColors
                );
            }

            composite.SetPixels(0, 0, 5, 5, Enumerable.Repeat(Color.green, 25).ToArray());
            composite.SetPixels(composite.width - 5, 0, 5, 5, Enumerable.Repeat(Color.green, 25).ToArray());
            composite.SetPixels(composite.width - 5, composite.height - 5, 5, 5, Enumerable.Repeat(Color.green, 25).ToArray());
            composite.SetPixels(0, composite.height - 5, 5, 5, Enumerable.Repeat(Color.green, 25).ToArray());
        }

        var thisTexture = this.GetComponent<SpriteRenderer>().sprite.texture;
        thisTexture.SetPixels(composite.GetPixels());
        thisTexture.Apply();
    }
}
