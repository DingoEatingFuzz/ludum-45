using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPath : MonoBehaviour
{
    public List<Vector3> Path = new List<Vector3>();
    public float Z = -1.0f;

    private LineRenderer line;
    private EdgeCollider2D edge;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting now: (" + this.Path.Count + "): " + this.Path.ToArray());
        this.line = this.GetComponent<LineRenderer>();
        this.edge = this.GetComponent<EdgeCollider2D>();
        this.UpdatePath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdatePath() {
        Debug.Log("Updating the path: " + this.Path);
        // Set the EdgeCollider (2D)
        var path = this.Path.ToArray();
        this.line.positionCount = path.Length;
        this.line.SetPositions(path);

        // Set the LineRenderer (3D)
        var path2 = new List<Vector2>();
        foreach (var p in path)
        {
            path2.Add(new Vector2(p.x, p.y));
        }
        this.edge.points = path2.ToArray();
    }
}
