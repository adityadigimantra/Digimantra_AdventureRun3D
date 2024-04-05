using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CurvyPath
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CreateMesh : MonoBehaviour
    {
        public GameObject point1;
        public GameObject point2;
        public GameObject point3;
        public GameObject point4;

        public Vector3 up;
        public Vector3 right;
        public Vector3 forward;
        public Vector3 rotation;
        public Quaternion quat;
        public Material Curved;

        public void createMesh(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            var MeshFilter = GetComponent<MeshFilter>();
            var mesh = new Mesh();
            MeshFilter.mesh = mesh;

            Vector3[] vertices = new Vector3[8];

            //vertices[0] = new Vector3(0, 0, 0);
            //vertices[1] = new Vector3(width, 0, 0);
            //vertices[2] = new Vector3(0, height, 0);
            //vertices[3] = new Vector3(width, height, 0);
            Vector3 p5 = p1 - new Vector3(0, 1, 0);
            Vector3 p6 = p2 - new Vector3(0, 1, 0);
            Vector3 p7 = p3 - new Vector3(0, 1, 0);
            Vector3 p8 = p4 - new Vector3(0, 1, 0);
            vertices[0] = transform.InverseTransformPoint(p1);
            vertices[1] = transform.InverseTransformPoint(p2);
            vertices[2] = transform.InverseTransformPoint(p3);
            vertices[3] = transform.InverseTransformPoint(p4);
            vertices[4] = transform.InverseTransformPoint(p5);
            vertices[5] = transform.InverseTransformPoint(p6);
            vertices[6] = transform.InverseTransformPoint(p7);
            vertices[7] = transform.InverseTransformPoint(p8);

            mesh.vertices = vertices;

            //int[] tri = new int[6];

            //tri[0] = 0;
            //tri[1] = 2;
            //tri[2] = 1;

            //tri[3] = 2;
            //tri[4] = 3;
            //tri[5] = 1;
            int[] tri = {
    0, 2, 1, //face front
	2, 3, 1,
    2, 6, 3, //face top
	6, 7, 3,
    5, 1, 7, //face right
	1, 3, 7,
    4, 6, 0, //face left
	6, 2, 0,
    5, 7, 4, //face back
	7, 6, 4,
    4, 0, 5, //face bottom
	0, 1, 5
};

            mesh.triangles = tri;

            Vector3[] normals = new Vector3[8];

            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;
            normals[4] = -Vector3.forward;
            normals[5] = -Vector3.forward;
            normals[6] = -Vector3.forward;
            normals[7] = -Vector3.forward;

            mesh.normals = normals;


            //Vector2[] uv = new Vector2[4];

            //uv[0] = new Vector2(0, 0);
            //uv[1] = new Vector2(1, 0);
            //uv[2] = new Vector2(0, 1);
            //uv[3] = new Vector2(1, 1);

            //mesh.uv = uv;
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            gameObject.GetComponent<MeshCollider>().convex = true;
        }

        private void OnBecameInvisible()
        {
            if (GameManager.Instance != null &&  GameManager.Instance.GameState != GameState.GameOver && GameManager.Instance.playerController != null)
                if (gameObject.transform.position.z < GameManager.Instance.playerController.transform.position.z)
                {
                    GameManager.Instance.playerController.ReUsePlane(gameObject);
                    
                }
        }
    }
}
