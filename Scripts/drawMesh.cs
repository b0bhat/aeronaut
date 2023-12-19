using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawMesh : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    public Vector3[] vertexArray = {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)};
    Mesh mesh;

  void Start() {
       // meshFilter.GetComponent<MeshFilter>();
       // meshRenderer.GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void FixedUpdate() {
        mesh.vertices = vertexArray;
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles =  new int[] {0, 1, 2};
    }

    public void SetVertices(Vector3 p1, Vector3 p2, Vector3 p3) {
            vertexArray = new Vector3[] {p1, p2, p3};
    }
}