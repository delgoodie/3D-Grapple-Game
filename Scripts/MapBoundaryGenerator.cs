using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MapBoundaryGenerator : MonoBehaviour
{
    Mesh mesh;
    public int uvSlider;
    public bool mapGeneratorEnabled;
    public Vector2[] uv;
    public Material mat;
    Vector3[] vertices;
    int[] triangles;
    int[] special = new int[10];
    void Start()
    {
        if (mapGeneratorEnabled)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
            CreateShape();
            UpdateMesh();
            GetComponent<MeshRenderer>().material = mat;
        }
    }

    void CreateShape()
    {
        int midSections = 1;
        int sides = 4;
        int midSectionDistance = 500;
        int minWidth = 200;
        int maxWidth = 500;


        //CREATES VERTICES
        vertices = new Vector3[2 + sides * (midSections + 1)];
        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 0; i < midSections + 1; i++)
        {
            float width = Random.Range(minWidth, maxWidth);
            for (int j = 0; j < sides; j++)
            {
                float theta = ((2 * Mathf.PI) / sides) * j;
                vertices[i * sides + j + 1] = new Vector3(Mathf.Cos(theta) * maxWidth, Mathf.Sin(theta) * maxWidth, i * midSectionDistance); //new Vector3(Mathf.Cos(theta) * width, Mathf.Sin(theta) * width, i * midSectionDistance);
            }
        }
        vertices[vertices.Length - 1] = new Vector3(0, 0, midSections * midSectionDistance);
        float zOff = (vertices[vertices.Length - 1].z - vertices[0].z) / 2;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].z -= zOff;
        }


        //CREATES TRIANGLES
        triangles = new int[(sides * midSections * 2 + sides * 2) * 3];
        int startT = 0;
        int startV = 1;
        for (int i = 0; i < sides; i++)
        {
            triangles[startT + i * 3] = 0;
            triangles[startT + i * 3 + 1] = startV + i;
            triangles[startT + i * 3 + 2] = (i != sides - 1) ? startV + i + 1 : startV;
        }

        startT = sides * 3;
        startV = 1;

        for (int i = 0; i < midSections; i++)
        {
            for (int j = 0; j < sides; j++)
            {
                if (j == sides - 1)
                {
                    triangles[startT + j * 6] = startV + j + sides;
                    triangles[startT + j * 6 + 1] = startV + j + 1;
                    triangles[startT + j * 6 + 2] = startV + sides - 1;

                    triangles[startT + j * 6 + 3] = startV + j;
                    triangles[startT + j * 6 + 4] = startV + sides;
                    triangles[startT + j * 6 + 5] = startV;
                }
                else
                {
                    triangles[startT + j * 6] = startV + j + sides;
                    triangles[startT + j * 6 + 1] = startV + j + sides + 1;
                    triangles[startT + j * 6 + 2] = startV + j + 1;

                    triangles[startT + j * 6 + 3] = startV + j;
                    triangles[startT + j * 6 + 4] = startV + j + sides;
                    triangles[startT + j * 6 + 5] = startV + j + 1;
                }
            }
            startT += sides * 6;
            startV += sides;
        }

        startT = triangles.Length - sides * 3;
        startV = vertices.Length - sides - 1;
        for (int i = 0; i < sides; i++)
        {
            triangles[startT + i * 3] = vertices.Length - 1;
            triangles[startT + i * 3 + 1] = (i != sides - 1) ? startV + i + 1 : startV;
            triangles[startT + i * 3 + 2] = startV + i;
        }

        //CREATE UVS
        uv = new Vector2[vertices.Length];
        startV = 1;
        for (int i = 0; i < midSections; i++)
        {
            for (int j = 0; j < sides; j++)
            {
                int vertex = startV + i * sides + j;
                float dist = Vector3.Distance(vertices[startV + i * sides + 1], vertices[startV + i * sides + 2]);
                uv[vertex] = new Vector2((Mathf.Sqrt(2) * maxWidth * (sides / 2 - j)) / 200, i * midSectionDistance / 200);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (mapGeneratorEnabled)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], (i % 5 == 0 && i > 6 && i < vertices.Length - 6) ? 0.1f : 0.1f);
            }

            for (int i = 0; i < special.Length; i++)
            {
                if (special[i] != 0)
                    Gizmos.DrawSphere(vertices[special[i]], (i + 1) * .5f);
            }
        }
    }


    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
}
