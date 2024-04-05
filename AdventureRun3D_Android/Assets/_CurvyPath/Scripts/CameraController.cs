using UnityEngine;
using System.Collections;

namespace CurvyPath
{
    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;
        private Vector3 originalDistance;
        private Vector3 newDistance;
        public Vector3 cameraOffsetMain;
        [Header("Shaking Effect")]
        // How long the camera shaking.
        public float shakeDuration = 0.1f;
        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.2f;
        public float decreaseFactor = 0.3f;
        [HideInInspector]
        public Vector3 originalPos;

        private float currentShakeDuration;
        private float currentDistance;
        private Vector3 pos;

        public float speed = 1.0F;

        void Start()
        {
             originalDistance =transform.position - playerTransform.transform.position;
           
            //Vector3 thirdPersonPos = Vector3.Lerp(originalDistance, (transform.position - cameraOffsetMain), cameraAnimTime);
        }

        void LateUpdate()
        {
            if(GameManager.Instance.GameState==GameState.Playing)// Original-if (GameManager.Instance.GameState == GameState.Playing)
            {
                //transform.position = pos;
                pos = playerTransform.position +originalDistance;
                //transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, 0.08f / (GameManager.Instance.speed / 200));
                transform.position = pos;//new Vector3(pos.x,13, pos.z);
            }
        }
        public void FixPosition()
        {
           // transform.position = playerTransform.position + originalDistance;
        }

        public void ShakeCamera()
        {
            StartCoroutine(Shake());
        }

        IEnumerator Shake()
        {
            originalPos = transform.position;
            currentShakeDuration = shakeDuration;
            while (currentShakeDuration > 0)
            {
                transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
                currentShakeDuration -= Time.deltaTime * decreaseFactor;
                yield return null;
            }
            transform.position = originalPos;
        }
    }
}