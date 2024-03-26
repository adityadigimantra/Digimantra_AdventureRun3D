using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CurvyPath
{
    public class PlayerBallController : MonoBehaviour
    {

        // Use this for initialization
        Vector3 gravity;
        bool move;
        GameObject ground;
        Vector3 upDir;
        private Vector2 prevPoint;
        private Vector2 newPoint;
        private Vector2 screenTravel;
        bool hasMove=true;
        bool start;
        Vector3 velocity;
        Vector3 newveolocity;
        Vector3 right;
        Vector3 forward;
        //public Mesh mesh;
        bool change;
        bool leftTurn=true;
        bool rightTurn=true;
        bool isCollision;
        bool die;
        float limitDistance;
        public bool EnableRotating = true;
        public ParticleSystem scoreEffect;
        public ParticleSystem collisionEffect;
        Color scoreColor;
        AnimationCurve curve;
        float speed;
        bool collision;
        public float coordinatesScore;
        bool first = true;
        Rigidbody rigiBody;
        TrailRenderer trailRender;
        Renderer rendererObject;
        GameObject body;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == gameObject.tag)
            {
                createCollisionEffect(gameObject.transform.position);
                CreateScoreEffect(other.gameObject.transform.position);
                if(other.gameObject.GetComponent<BallController>().move)
                    GameManager.Instance.playerController.AddScore(new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-0.75f,gameObject.transform.position.z),gameObject,4);
                else
                    GameManager.Instance.playerController.AddScore(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.75f, gameObject.transform.position.z), gameObject, 1);
                SoundManager.Instance.PlaySound(SoundManager.Instance.score);
                coordinatesScore = other.gameObject.transform.position.z;
                other.gameObject.GetComponent<BallController>().ResetToDefault();
                other.gameObject.SetActive(false);
                //GameManager.Instance.playerController.score += 1;
            }
            else
            if(other.gameObject.tag=="Gold")
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
                GameManager.Instance.playerController.AddCoin(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.75f, gameObject.transform.position.z), gameObject);
            }

            if ((other.gameObject.tag == "ChangeToColor1" || other.gameObject.tag == "ChangeToColor2" || other.gameObject.tag == "ChangeToColor3") && collision)
            {
                collision = false;
                change = true;
                switch (other.gameObject.tag)
                {
                    case "ChangeToColor1":
                        rendererObject.material.SetColor("_Color", GameManager.Instance.color4);
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 0)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 1)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 2)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        break;
                    case "ChangeToColor2":
                        rendererObject.material.SetColor("_Color", GameManager.Instance.color4);
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 0)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 1)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 2)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }

                        break;
                    case "ChangeToColor3":
                        rendererObject.material.SetColor("_Color", GameManager.Instance.color4);
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 0)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 1)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 2)
                        {
                            body.gameObject.tag = "Color1";
                            gameObject.tag = "Color1";
                        }
                        break;
                }

                if (speed < GameManager.Instance.limitSpeed)
                    speed += GameManager.Instance.increaseSpeedRatio * speed;
                else
                    speed = GameManager.Instance.limitSpeed;

                //SoundManager.Instance.PlaySound(SoundManager.Instance.jump);
                createCollisionEffect(gameObject.transform.position);
                StartCoroutine(FixPosition());
            }

            if (other.gameObject.tag != gameObject.tag && other.gameObject.tag != "Plane" && other.gameObject.tag != "ChangeToColor1" && other.gameObject.tag != "ChangeToColor2" && other.gameObject.tag != "ChangeToColor3" && other.gameObject.tag != "Gold" && other.gameObject.transform.position.z != coordinatesScore )
            {
                other.gameObject.GetComponent<Collider>().isTrigger = false;
                die = true;
                //SoundManager.Instance.PlaySound(SoundManager.Instance.wrong);
                Camera.main.GetComponent<CameraController>().ShakeCamera();
                gameObject.transform.localScale = Vector3.zero;
                if (gameObject.transform.childCount > 0)
                    gameObject.transform.GetChild(0).SetParent(null);
                CreateScoreEffect(gameObject.transform.position);
                GameManager.Instance.playerController.Die();
                rigiBody.useGravity = false;
                velocity = Vector3.zero;
            }
        }

        void CreateScoreEffect( Vector3 position )
        {
            scoreEffect.transform.position = position;
            scoreColor = rendererObject.material.GetColor("_Color");
            scoreEffect.GetComponent<Renderer>().material.SetColor("_Color", scoreColor);
            scoreEffect.Play();
        }

        void createCollisionEffect( Vector3 position)
        {
            var characterMesh = body.GetComponent<MeshFilter>().sharedMesh;
            var main = collisionEffect.main;
            collisionEffect.transform.position = position;
            main.startColor = rendererObject.material.GetColor("_Color");
            collisionEffect.GetComponent<ParticleSystemRenderer>().mesh = characterMesh;
            main.startSize = gameObject.transform.localScale.x*2;
            collisionEffect.transform.parent = gameObject.transform;
            collisionEffect.Play();


        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Plane" && !die && first)
            {
                limitDistance = GameManager.Instance.width/2;
                isCollision = true;
                Physics.gravity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.transform.rotation = collision.transform.rotation;
                move = true;
                ground = collision.gameObject;
                Physics.gravity = -ground.transform.up * 100;
                right = gameObject.transform.right;
                forward = gameObject.transform.forward;
                velocity = forward * speed;
                first = false;
                
            }
            
        }

        IEnumerator FixPosition()
        {
            /*
            var startTime = Time.time;
            float runTime = 0.25f*(200/speed);
            float timePast = 0;
            Vector3 pos = gameObject.transform.position;
            //curve = new AnimationCurve(new Keyframe(0, pos.y), new Keyframe(0.5f, pos.y+8), new Keyframe(1, pos.y));
            //curve.preWrapMode = WrapMode.Clamp;
            //curve.postWrapMode = WrapMode.Clamp;

            while (Time.time < startTime + runTime)
            {
                timePast += Time.deltaTime;
                float factor = timePast / runTime;
                Vector3 posi = gameObject.transform.position;//Vector3.Lerp(pos, pos+new Vector3(0,0,20), factor);
                //posi.y = curve.Evaluate(factor);
                transform.position = pos;
                gameObject.transform.position = posi;
               
            }
            */
            collision = true;
            velocity = transform.forward * speed;
            yield return null;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag== "ChangeToColor1" && change)
            {
                isCollision = false;
               
            }
        }

        void Awake()
        {
           rigiBody = gameObject.GetComponent<Rigidbody>();
           body = gameObject.transform.GetChild(1).gameObject;
           rendererObject = new Renderer();
           rendererObject = gameObject.transform.GetChild(1).GetComponent<Renderer>();
           //trailRender = gameObject.transform.GetChild(0).GetComponent<TrailRenderer>();
        }

        void Start()
        {
            collision = true;
            speed = GameManager.Instance.speed;
        }

        private void processSwipe()
        {
            float force = Mathf.Clamp(Mathf.Abs(screenTravel.x), 0, 150);
            prevPoint = newPoint;
            // your code here
            if (screenTravel.x > 0 && rightTurn && isCollision)
            {
                rigiBody.AddForce(right * force * GameManager.Instance.swipeForce, ForceMode.Force);
            }
            if (screenTravel.x < 0 && leftTurn && isCollision)
            {
                rigiBody.AddForce(right * -force * GameManager.Instance.swipeForce, ForceMode.Force);
            }

        }
        void FixedUpdate()
        {
            if (GameManager.Instance.playerController.isPlay)
            {
                if (EnableRotating)
                    body.transform.Rotate(Vector3.right * GameManager.Instance.speed * Time.deltaTime * 3);
                if (isCollision)
                    rigiBody.velocity = velocity;
                if (start)
                {
                    // Simulate touch events from mouse events
                    if (Input.touchCount == 0)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            HandleTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
                        }
                        if (Input.GetMouseButton(0))
                        {
                            HandleTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            HandleTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
                        }
                    }
                    else
                    {
                        if (Input.touchCount < 2)
                        {
                            Touch touch = Input.GetTouch(0);
                            HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase);
                        }
                        else
                        {
                            newPoint = prevPoint = Input.GetTouch(1).position;
                        }
                    }
                }
            }
            else
                rigiBody.velocity = Vector3.zero;

            Vector3 pos = gameObject.transform.position;
            pos.x = Mathf.Clamp(pos.x, -limitDistance, limitDistance-5f);//Mathf.Clamp(pos.x, -limitDistance, limitDistance-5);
            if (pos.x <= (-limitDistance))
            {
                leftTurn = false;
                rigiBody.MovePosition(pos);
            }
            else if (pos.x >= (limitDistance - 5f))
            {
                rightTurn = false;
                rigiBody.MovePosition(pos);
            }
            if (pos.x > (-limitDistance))
            {
                leftTurn = true;
            }
            if (pos.x < (limitDistance - 5f))
            {
                rightTurn = true;
            }

        }

        private void Update()
        {
            GameManager.Instance.speed = speed;
            GameManager.Instance.playerController.transform.position = gameObject.transform.position;
            if (move && hasMove)
            {
                hasMove = false;
                StartCoroutine(Wait());
            }
        }

        private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
        {
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    newPoint = prevPoint = Input.mousePosition;
                    break;
                case TouchPhase.Moved:
                    newPoint = Input.mousePosition;
                    screenTravel = newPoint - prevPoint;
                    processSwipe();
                    break;
                case TouchPhase.Ended:
                    break;
            }
        }

        private void HandleTouch(Vector3 touchPosition, TouchPhase touchPhase)
        {
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    prevPoint = Input.mousePosition;
                    break;
                case TouchPhase.Moved:
                    newPoint = Input.mousePosition;
                    screenTravel = newPoint - prevPoint;
                    processSwipe();
                    break;
                case TouchPhase.Ended:
                    break;
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            start = true;
        }

    }
}