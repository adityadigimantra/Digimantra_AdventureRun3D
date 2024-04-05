using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frustrumCulling : MonoBehaviour
{
    public Camera mainCamera;
    public MeshRenderer meshRenderer;

    private void Start()
    {
        mainCamera = Camera.main;
        if(mainCamera!=null)
        {
            Debug.Log("Main Camera Found");
        }    
        else
        {
            Debug.Log("Main Camera Not Found");
        }
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        Plane[] plane = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Bounds bounds = meshRenderer.bounds;

        bool isVisible = GeometryUtility.TestPlanesAABB(plane, bounds);

        if(isVisible)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            meshRenderer.enabled = true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("InvisibleLayer");
            meshRenderer.enabled = false;
        }

    }
    //void OnBecameInvisible()
    //{
    //    gameObject.layer = LayerMask.NameToLayer("InvisibleLayer");
    //}
    //
    //void OnBecameVisible()
    //{
    //    gameObject.layer = LayerMask.NameToLayer("Default");
    //}
    //
}
