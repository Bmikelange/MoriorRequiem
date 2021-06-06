using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    List<int> vertices;
}

public class TestDiceValue : MonoBehaviour
{
    new Rigidbody rigidbody;
    Mesh mesh;
    Vector3 pos;
    Quaternion rot;
    List<Face> faces = new List<Face>();

    void BuildFaces()
    {
        for (int i = 0; i < mesh.triangles.Length / 3; ++i)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(new Vector3(0, 100, 100));
        //rigidbody.AddTorque(new Vector3(100, 0, 300));
        mesh = GetComponent<MeshFilter>().mesh;
        BuildFaces();
    }

    void CheckTransform()
    {
        if(pos == transform.position && rot == transform.rotation)
        {
            transform.hasChanged = false;
        } else
        {
            transform.hasChanged = true;
        }
        pos = transform.position;
        rot = transform.rotation;
    }

    int GetUpwardsFace()
    {
        int upFace = 0;
        float yValue = Mathf.NegativeInfinity;
        //Debug.Log(mesh.triangles.Length);
        for(int i = 0; i < mesh.triangles.Length / 3; ++i)
        {
            float meanY = 0f;
            for(int j = i * 3; j < i * 3 + 3; ++j)
            {
                meanY += transform.TransformPoint(mesh.vertices[mesh.triangles[j]]).y;
            }
            meanY /= 3f;
            if(meanY > yValue)
            {
                yValue = meanY;
                upFace = i;
            }
             
        }
        return upFace;
    }

   int GetUpwardsVertex()
    {
        int maxY = 0;
        float yValue = Mathf.NegativeInfinity;
        
        HashSet<Vector3> vert = new HashSet<Vector3>(mesh.vertices);
        List<Vector3> vertices = new List<Vector3>(vert);
        //Debug.Log(vertices.Count);
        for(int i = 0; i < vertices.Count; ++i)
        {
            float y = transform.TransformPoint(vertices[i]).y;
            if (y > yValue)
            {
                yValue = y;
                maxY = i;
            }

        }
        return maxY + 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTransform();
        if(!transform.hasChanged)
        {
            Debug.Log(GetUpwardsFace());
            //Debug.Log(GetUpwardsVertex());
        }
    }
}
