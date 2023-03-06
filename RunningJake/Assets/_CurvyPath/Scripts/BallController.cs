using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvyPath
{
    public class BallController : MonoBehaviour {

        public float counter;
        public bool move;
        public float velocity;
        public bool moveRight;
        float speed;
        public AnimationCurve curve;

        // Use this for initialization
        void Start()
        {
            speed = 5f;
        }

        // Update is called once per frame
        void Update() {

            if (move)
            {
                counter += Time.deltaTime*speed;
                gameObject.GetComponent<Rigidbody>().velocity = Mathf.Sin(counter) * Vector3.right * velocity*speed;
            }
        }

        public void ResetToDefault()
        {
            velocity = 0;
            move = false;
            counter = 0;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(move && other.gameObject.tag==gameObject.tag)
            {
                ScoreManager.Instance.AddScore(+4);
            }
            if (!move && other.gameObject.tag == gameObject.tag)
            {
                ScoreManager.Instance.AddScore(+1);
            }
        }

        private void OnBecameInvisible()
        {
            if (GameManager.Instance != null && GameManager.Instance.playerController != null)
            {
                if (GameManager.Instance.playerController.isPlay && gameObject.transform.position.z < GameManager.Instance.playerController.transform.position.z)
                {
                    gameObject.SetActive(false);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    ResetToDefault();
                }
            }
        }
    }
}
