using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvyPath
{
    public class DestroyObject : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        private void OnBecameInvisible()
        {
            if (GameManager.Instance != null && GameManager.Instance.playerController != null)
            {
                if (GameManager.Instance.playerController.isPlay && gameObject.transform.position.z < GameManager.Instance.playerController.transform.position.z)
                    Destroy(gameObject);
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
