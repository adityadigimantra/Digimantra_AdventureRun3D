using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CurvyPath
{
    public class PlayerController : MonoBehaviour
    {
        public int count = 0;
        public static event System.Action PlayerDied;
        bool greaterdistance=false;
        bool isCreated = false;
        public GameObject PlanePrefab;
        public GameObject junglePrefab;
        public GameObject jungleSpawnPoint1;
        public GameObject jungleSpawnPoint2;
        public bool isPlay;
        public GameObject originalPoint1;
        public GameObject originalPoint2;
        public int planeCount;
        public int jungleTerrainCount;
        float offsetShaderX;
        float offsetShaderY;
        bool first;
        bool isCreateBall;
        Vector4 oldOffsetShader;
        int timeCount;
        GameObject playerBall = null;
        public GameObject addScore;
        bool color;
        public GameObject[] ChangeTag;
        public int score;
        string newTag;
        string oldTag;
        int[] array;
        int randomScore=0;
        int maxScore;
        public GameObject coin;
        public GameObject addCoin;
        bool spawnMoveBall;
        int ballNumber;
        public ParticleSystem scoreEffect;
        public ParticleSystem collisionEffect;
        GameObject trail;
        float timePast;
        float height;
        float heightforJungleTerrain;
        float heightForVeniceCity;
        public float heightforMedivalCity;
        public float yaxisForVeniceCity;
        float yaxisForMedievalCity;
        bool isdone = false;
        bool isdone1 = false;
        bool isdone2=false;

        [Header("Transforms")]
        public Vector3 playerOffset;

        public float distance;
        public GameObject MainCamera;
        [Header("Jungle Run")]
        public GameObject[] JungleFlats;
        [Header("Venice City")]
        public GameObject[] VeniceFlats;
        [Header("Medival City")]
        public GameObject[] MedievalFlats;
        [Header("Environment Temp Objs-Caching")]
        public GameObject JunglefirstTerrain;
        public GameObject JunglesecondTerrain;
        public GameObject VenicefirstTerrain;
        public GameObject VenicesecondTerrain;
        public GameObject medievalFirstTerrain;
        public GameObject medievalSecondTerrain;
        public GameObject point1;
        public GameObject point2;
        public float _distancebetween2points;
        public GameObject playerClone;
        public GameObject BackGroundSelectionScreen;
        public GameObject distanceCube;
        public Text distanceText;
        public Animator playerAnimator;
        public AudioClip[] musics;
        public Sound background;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";

        private void Awake()
        {
            height = GameManager.Instance.lenght;
            heightforJungleTerrain = GameManager.Instance.jungleTerrainLenght;
            heightForVeniceCity = GameManager.Instance.VeniceCityLenght;
            heightforMedivalCity = GameManager.Instance.medievalCityLenght;
            offsetShaderX =0;
            offsetShaderY = 0;
            newTag = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex].tag;
            randomScore =0;
            maxScore = 11;
            array = new int[3];
            array[0] = 0;
            array[1] = 1;
            array[2] = 2;
            oldTag = "ChangeToColor3";
            color = true;
            first = true;
            
            // Setup
            float distance = Mathf.Abs(originalPoint1.transform.position.x - originalPoint2.transform.position.x);
            if (distance > GameManager.Instance.width)
            {
                originalPoint1.transform.position += new Vector3(Mathf.Abs(distance - GameManager.Instance.width) / 2, 0, 0);
                originalPoint2.transform.position -= new Vector3(Mathf.Abs(distance - GameManager.Instance.width) / 2, 0, 0);
            }
            else
            {
                originalPoint1.transform.position -= new Vector3(Mathf.Abs(distance - GameManager.Instance.width) / 2, 0, 0);
                originalPoint2.transform.position += new Vector3(Mathf.Abs(distance - GameManager.Instance.width) / 2, 0, 0);
            }
            for (int i = 0; i <= GameManager.Instance.limitPlane; i++)
            {
                CreatePlane();

                planeCount += 1;
            }
            PlayerPrefs.SetInt("SelectTrack", 1); //Working
          

            if (PlayerPrefs.GetInt("WhichMapPlaying")==1)
            {
                PlayerPrefs.SetInt("SelectTrack", 1);
              
            }
            if(PlayerPrefs.GetInt("WhichMapPlaying") == 2)
            {
                PlayerPrefs.SetInt("SelectTrack", 2);
              
            }
            if(PlayerPrefs.GetInt("WhichMapPlaying")==3)
            {
                PlayerPrefs.SetInt("SelectTrack", 3);
           
            }

         

            if(distanceCube==null)
            {
               
                distanceCube = GameObject.FindWithTag("distancecube");
            }
          

            if(distanceText==null)
            {
               
                distanceText = GameObject.Find("distanceText").GetComponent<Text>(); 
            }
         

        }
        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        public void ReUsePlane(GameObject _planePrefab)
        {
            if (originalPoint1 != null && originalPoint2 != null)
            {
                Vector3 point3;
                Vector3 point4;

                point3 = originalPoint1.transform.position + new Vector3(0, 0, height);
                point4 = originalPoint2.transform.position + new Vector3(0, 0, height);


                _planePrefab.transform.position = (originalPoint1.transform.position - originalPoint2.transform.position) / 2 + originalPoint2.transform.position;
               

                originalPoint1.transform.position = point3;
                originalPoint2.transform.position = point4;

                if (isCreateBall)
                {
                    if (score >= randomScore)
                    {
                        CreateSlope(_planePrefab.transform.position);
                    }
                    else
                   {
                        int spawnCoinRate = 1;//Random.Range(0, 3);
                        CreateBall(_planePrefab.transform.position);
                        if (spawnCoinRate == 1)
                        {
                           CreateCoin(_planePrefab.transform.position);
                        }
                        isCreateBall = false;
                    }
                //isCreateBall = false;
                }

               // _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_QOffsetOld", oldOffsetShader);
               // _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_QOffset", new Vector4(offsetShaderX, offsetShaderY, 0, 1));

                if (color)
                {
                    _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_Color", GameManager.Instance.planeFirstColor);
                    color = !color;
                }
                else
                {
                    _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_Color", GameManager.Instance.planeSecondColor);
                    color = !color;
                }

                oldOffsetShader = new Vector4(offsetShaderX, offsetShaderY, 0, 1);

            }
        }
        // Update is called once per frame
        void Update()
        {
            
            playerClone = playerBall;

            if (playerClone == null)
            {
                Debug.Log("No PlayerClone Found");
            }
            else
            {
                playerClone.transform.GetChild(3).gameObject.SetActive(false); //closing character selection UI Character
               // playerAnimator = playerClone.transform.GetChild(2).GetComponent<Animator>();
                Debug.Log("Got the Animator" + playerAnimator);
            }
            //Selecting Track 1

            if (PlayerPrefs.GetInt("SelectTrack") == 1)
            {
                if (!isdone)
                {
                    JunglefirstTerrain = (GameObject)(Instantiate(JungleFlats[0], transform.position, Quaternion.Euler(0, 0, 0)));
                    JunglesecondTerrain = (GameObject)(Instantiate(JungleFlats[1], transform.position, Quaternion.Euler(0, 0, 0)));
                    isdone = true;
                }

                JunglefirstTerrain.transform.position = point1.transform.position;
                JunglesecondTerrain.transform.position = point2.transform.position;

                PlayerPrefs.SetInt("WhichMapPlaying", 1);
               // Debug.Log("Which Map Playing:Update Method:" + PlayerPrefs.GetInt("WhichMapPlaying"));

                //Checking if this track is open or not1

                if(!JunglefirstTerrain.activeSelf && !JunglesecondTerrain.activeSelf)
                {
                    JunglefirstTerrain.SetActive(true);
                    JunglesecondTerrain.SetActive(true);
                   // Debug.Log("Jungle Terrain is Active");
                }


                //Checking for other Tracks if they are intantiated and they are active then switch them off.

                if(VenicefirstTerrain==null && VenicesecondTerrain==null)
                {
                   // Debug.Log("Venice Track is Null for now");
                }
                else if(VenicefirstTerrain.activeSelf && VenicesecondTerrain.activeSelf)
                {
                    VenicefirstTerrain.SetActive(false);
                    VenicesecondTerrain.SetActive(false);
                   // Debug.Log("Venice Track  Found and Closed");
                }
                if(medievalFirstTerrain==null && medievalSecondTerrain==null)
                {
                   // Debug.Log("Medieval Track is Null for now");
                }
                else if(medievalFirstTerrain.activeSelf && medievalSecondTerrain.activeSelf)
                {
                    medievalFirstTerrain.SetActive(false);
                    medievalSecondTerrain.SetActive(false);
                  //  Debug.Log("Medeival  Track  Found and Closed");
                }

                //Updating Positions of Terrain
                if(playerClone!=null)
                {
                    if (playerClone.transform.position.z > point1.transform.position.z + 500)
                    {
                        Vector3 point3;
                        point3 = JunglefirstTerrain.transform.position + new Vector3(0, 0, heightforJungleTerrain * 2);
                        JunglefirstTerrain.transform.position = point3;
                        point1.transform.position = JunglefirstTerrain.transform.position;
                    }
                    if (playerClone.transform.position.z > point2.transform.position.z + 500)
                    {
                        Vector3 point4;
                        point4 = JunglesecondTerrain.transform.position + new Vector3(0, 0, heightforJungleTerrain * 2);
                        JunglesecondTerrain.transform.position = point4;
                        point2.transform.position = JunglesecondTerrain.transform.position;
                    }
                }
                
              //  Debug.Log("Full Condition working till last point- Track 1");

            }


            //Selecting Track 2

            if (PlayerPrefs.GetInt("SelectTrack") == 2)
            {
                if (!isdone1)
                {
                    VenicefirstTerrain = (GameObject)(Instantiate(VeniceFlats[0], transform.position, Quaternion.Euler(0, 0, 0)));
                    VenicesecondTerrain = (GameObject)(Instantiate(VeniceFlats[1], transform.position, Quaternion.Euler(0, 0, 0)));
                    isdone1 = true;
                }
                VenicefirstTerrain.transform.position = point1.transform.position;
                VenicesecondTerrain.transform.position = point2.transform.position;


                PlayerPrefs.SetInt("WhichMapPlaying", 2);
              //  Debug.Log("Which Map Playing:Update Method:" + PlayerPrefs.GetInt("WhichMapPlaying"));

                //Checking if this track is open or not

                if (!VenicefirstTerrain.activeSelf && !VenicesecondTerrain.activeSelf)
                {
                    VenicefirstTerrain.SetActive(true);
                    VenicesecondTerrain.SetActive(true);
                    //Debug.Log("Venice Terrain is Active");
                }

                //Checking for other Tracks if they are intantiated and they are active then switch them off.

                if (JunglefirstTerrain == null && JunglesecondTerrain == null)
                {
                   // Debug.Log("Jungle Track is Null for now");
                }
                else if (JunglefirstTerrain.activeSelf && JunglesecondTerrain.activeSelf)
                {
                    JunglefirstTerrain.SetActive(false);
                    JunglesecondTerrain.SetActive(false);
                   // Debug.Log("Jungle Track  Found and Closed");
                }
                if (medievalFirstTerrain == null && medievalSecondTerrain == null)
                {
                  //  Debug.Log("Medieval Track is Null for now");
                }
                else if (medievalFirstTerrain.activeSelf && medievalSecondTerrain.activeSelf)
                {
                    medievalFirstTerrain.SetActive(false);
                    medievalSecondTerrain.SetActive(false);
                   // Debug.Log("Medeival  Track  Found and Closed");
                }



                //Updating Positions of Terrain
                if(playerClone!=null)
                {
                    if (playerClone.transform.position.z > point1.transform.position.z + 500)
                    {
                        Vector3 point3;
                        point3 = VenicefirstTerrain.transform.position + new Vector3(0, 0, heightForVeniceCity * 2);
                        VenicefirstTerrain.transform.position = point3;
                        point1.transform.position = VenicefirstTerrain.transform.position;
                    }

                    if (playerClone.transform.position.z > point2.transform.position.z + 500)
                    {
                        Vector3 point4;
                        point4 = VenicesecondTerrain.transform.position + new Vector3(0, 0, heightForVeniceCity * 2);
                        VenicesecondTerrain.transform.position = point4;
                        point2.transform.position = VenicesecondTerrain.transform.position;
                    }
                }


               // Debug.Log("Full Condition working till last point- Track 2");

            }


            //Selecting Track 3


            if (PlayerPrefs.GetInt("SelectTrack") == 3)
            {
                if (!isdone2)
                {
                    medievalFirstTerrain = (GameObject)(Instantiate(MedievalFlats[0], transform.position, Quaternion.Euler(0, 0, 0)));
                    medievalSecondTerrain = (GameObject)(Instantiate(MedievalFlats[1], transform.position, Quaternion.Euler(0, 0, 0)));
                    isdone2 = true;
                }

                medievalFirstTerrain.transform.position = point1.transform.position;
                medievalSecondTerrain.transform.position = point2.transform.position; 


                PlayerPrefs.SetInt("WhichMapPlaying", 3);
              //  Debug.Log("Which Map Playing:Update Method:" + PlayerPrefs.GetInt("WhichMapPlaying"));


                //Checking if this track is open or not

                if (!medievalFirstTerrain.activeSelf && !medievalSecondTerrain.activeSelf)
                {
                    medievalFirstTerrain.SetActive(true);
                    medievalSecondTerrain.SetActive(true);
                  //  Debug.Log("Medeival  Terrain is Active");
                }

                //Checking for other Tracks if they are intantiated and they are active then switch them off.

                if (JunglefirstTerrain == null && JunglesecondTerrain == null)
                {
                  //  Debug.Log("Jungle Track is Null for now");
                }
                else if (JunglefirstTerrain.activeSelf && JunglesecondTerrain.activeSelf)
                {
                    JunglefirstTerrain.SetActive(false);
                    JunglesecondTerrain.SetActive(false);
                  //  Debug.Log("Jungle Track  Found and Closed");
                }
                if (VenicefirstTerrain == null && VenicesecondTerrain == null)
                {
                  //  Debug.Log("Venice Track is Null for now");
                }
                else if (VenicefirstTerrain.activeSelf && VenicesecondTerrain.activeSelf)
                {
                    VenicefirstTerrain.SetActive(false);
                    VenicesecondTerrain.SetActive(false);
                 //   Debug.Log("Venice  Track  Found and Closed");
                }



                //Updating Positions of Terrain
                if(playerClone!=null)
                {
                    if (playerClone.transform.position.z > point1.transform.position.z + 500)
                    {
                        Vector3 point3;

                        point3 = medievalFirstTerrain.transform.position + new Vector3(0, 0, heightforMedivalCity * 2);
                        medievalFirstTerrain.transform.position = point3;
                        point1.transform.position = medievalFirstTerrain.transform.position;
                    }

                    if (playerClone.transform.position.z > point2.transform.position.z + 500)
                    {
                        Vector3 point4;
                        point4 = medievalSecondTerrain.transform.position + new Vector3(0, 0, heightforMedivalCity * 2);
                        medievalSecondTerrain.transform.position = point4;
                        point2.transform.position = medievalSecondTerrain.transform.position;
                    }
                }
             

              //  Debug.Log("Full Condition working till last point- Track 3");

             if (isPlay)
             {
                 timePast += Time.deltaTime;
             }

              //  Debug.Log("Track Selection working fine in Main Menu");
            }

            //Calculating Distance of Player from distance Cube.
            if(playerClone!=null)
            {
                float var = Vector3.Distance(playerClone.transform.position, distanceCube.transform.position);
                int varInt = (int)var;
                // Debug.Log("Distance Between 2 Points" + varInt);
                distanceText.text = (varInt / 10).ToString() + " " + "m";
            }


            

                   
        }


        public void CreatePlane()
        {
            Vector3 point3 = originalPoint1.transform.position + new Vector3(0, 0, height);
            
            Vector3 point4 = originalPoint2.transform.position + new Vector3(0, 0, height);

            GameObject _planePrefab = (GameObject)Instantiate(PlanePrefab, (originalPoint1.transform.position - originalPoint2.transform.position) / 2 + originalPoint2.transform.position, Quaternion.Euler(0, 0, 0));

            _planePrefab.GetComponent<CreateMesh>().createMesh(originalPoint1.transform.position, originalPoint2.transform.position, point3, point4);
            originalPoint1.transform.position = point3;
            originalPoint2.transform.position = point4;

            if (isCreateBall)
            {
                if (score >= randomScore)
                {
                  CreateSlope(_planePrefab.transform.position);
                }
                else
                {
                        int spawnCoinRate = Random.Range(0, 3);
                        CreateBall(_planePrefab.transform.position);
                        if (spawnCoinRate == 1)
                        {
                            CreateCoin(_planePrefab.transform.position);
                        }
                   
                }
                isCreateBall = false;
            }

            if (color)
            {
                _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_Color", GameManager.Instance.planeFirstColor);
                color = !color;
            }
            else
            {
                _planePrefab.GetComponent<MeshRenderer>().material.SetVector("_Color", GameManager.Instance.planeSecondColor);
                color = !color;
            }

            oldOffsetShader = new Vector4(offsetShaderX, offsetShaderY, 0, 1);

            if (first)
            {
                StartCoroutine(instantiate());
                first = false;
            }
        }


        void CreateSlope(Vector3 position)
        {
            int randomTag = Random.Range(0, 2);

            if (ChangeTag[randomTag].tag == oldTag)
            {
                if (randomTag >= 2)
                    randomTag -= 1;
                else
                    randomTag += 1;
            }

            switch (ChangeTag[randomTag].tag)
            {
                case "ChangeToColor1":
                    newTag = "Color1";
                    break;
                case "ChangeToColor2":
                    newTag = "Color2";
                    break;
                case "ChangeToColor3":
                    newTag = "Color3";
                    break;
            }

            GameObject change = (GameObject)Instantiate(ChangeTag[randomTag], position + new Vector3(0,1.5f, 0), Quaternion.Euler(-10f, 0, 0));
            newScale(change, GameManager.Instance.width);
            oldTag = change.tag;
            score = 0;
            randomScore = Random.Range(-1, 15);
            spawnMoveBall = false;
        }

        void CreateBall(Vector3 position)
        {
            float xRandom = 0;
            string ballTag = "Color3";
            Shuffle(array);
            score += 1;
            for (int i = 0; i<3; i++)
            {

                switch (i)
                {
                    case 0:
                        xRandom = 0;
                        break;
                    case 1:
                        xRandom = GameManager.Instance.spacing;
                        break;
                    case 2:
                        xRandom = -GameManager.Instance.spacing;
                        break;
                }

               

                switch (array[i])
                {
                    case 0:
                        ballTag = "Color3";
                        break;
                    case 1:
                       ballTag = "Color2";
                        break;
                    case 2:
                        ballTag = "Color1";
                        break;
                }

                GameObject _ball = ObjectPooling.SharedInstance.GetPooledObjectByTag(ballTag);
                if (_ball != null)
                {
                    _ball.transform.position = position + new Vector3(xRandom, 2.5f, 20);
                    _ball.transform.rotation = Quaternion.Euler(0, 0, 0);
                    _ball.SetActive(true);
                }
            }
        }

        void CreateMoveBall(Vector3 position, int ballNumber)
        {
            int i = Random.Range(1, 3);
            float xRandom = 0;
            string ballTag = "Color3";

            switch (i)
            {
                case 1:
                    xRandom = GameManager.Instance.spacing;
                    break;
                case 2:
                    xRandom = -GameManager.Instance.spacing;
                    break;
            }

            score += 1;
            switch (ballNumber)
            {
                case 0:
                    ballTag = "Color3";
                    break;
                case 1:
                    ballTag = "Color2";
                    break;
                case 2:
                    ballTag = "Color1";
                    break;
            }

            GameObject _moveBall = ObjectPooling.SharedInstance.GetPooledObjectByTag(ballTag);
            if (_moveBall != null)
            {
                _moveBall.transform.position = position + new Vector3(xRandom, 2.5f, 10);
                _moveBall.transform.rotation = Quaternion.Euler(0f, 0, 0);
                _moveBall.SetActive(true);
            }
            _moveBall.GetComponent<Rigidbody>().isKinematic = false;

            switch (i)
            {
                case 1:
                    _moveBall.GetComponent<BallController>().moveRight = false;
                    _moveBall.GetComponent<BallController>().velocity = -0.33f * GameManager.Instance.width;
                    ;
                    break;
                case 2:
                    _moveBall.GetComponent<BallController>().moveRight = true;
                    _moveBall.GetComponent<BallController>().velocity = 0.33f * GameManager.Instance.width;
                    break;
            }

            _moveBall.GetComponent<BallController>().move = true;

            if (_moveBall.tag == newTag)
            {
                _moveBall.GetComponent<Collider>().isTrigger = true;
            }
        }
        void CreateCoin(Vector3 position)
        {
            int i = Random.Range(0, 2);
            float xRandom = 0;

            switch (i)
            {
                case 0:
                    xRandom = 0;
                    break;
                case 1:
                    xRandom = GameManager.Instance.spacing;
                    break;
                case 2:
                    xRandom = -GameManager.Instance.spacing;
                    break;
            }

            GameObject _coin = (GameObject)Instantiate(coin, position + new Vector3(xRandom, 2.5f,100), Quaternion.Euler(0, 0, 0));
            GameObject _coin2 = (GameObject)Instantiate(coin, position + new Vector3(xRandom, 2.5f,250), Quaternion.Euler(0, 0, 0));
            _coin.GetComponent<MeshRenderer>().material.SetVector("_QOffsetOld", oldOffsetShader);
            _coin2.GetComponent<MeshRenderer>().material.SetVector("_QOffsetOld", oldOffsetShader);
            _coin.GetComponent<MeshRenderer>().material.SetVector("_QOffset", new Vector4(offsetShaderX, offsetShaderY, 0, 1));
            _coin2.GetComponent<MeshRenderer>().material.SetVector("_QOffset", new Vector4(offsetShaderX, offsetShaderY, 0, 1));
            _coin2.GetComponent<MeshRenderer>().material.SetFloat("_CamColorDistModifier", 100); _coin.GetComponent<MeshRenderer>().material.SetFloat("_CamColorDistModifier", 100);
        }

        public void newScale(GameObject theGameObject, float newSize)
        {

            float size = theGameObject.GetComponent<MeshRenderer>().bounds.size.x;

            Vector3 rescale = theGameObject.transform.localScale;

            rescale.x = newSize * rescale.x / size;

            theGameObject.transform.localScale = rescale;

        }

        public void ReScale(GameObject theGameObject, float witdh, float height)
        {

            float sizeX = theGameObject.GetComponent<MeshRenderer>().bounds.size.x;

            Vector3 rescale = theGameObject.transform.localScale;

            rescale.x = witdh * rescale.x / sizeX;

            float sizeZ = theGameObject.GetComponent<MeshRenderer>().bounds.size.z;

            rescale.z = height * rescale.z / sizeZ;

            theGameObject.transform.localScale = rescale;

        }

        IEnumerator instantiate()
        {
            //if(finishLoad)
            yield return new WaitForSeconds(GameManager.Instance.timeCreateBall);
            //else
            //yield return new WaitForSeconds(GameManager.Instance.timeCreateBall*0.1f);

            isCreateBall = true;
            StartCoroutine(instantiate());

        }

 

        IEnumerator LerpXY(float randomX, float RandomY)
        {
            var startTime = Time.time;
            float runTime = 20f;
            float timePast = 0;
            float oriX = offsetShaderX;
            float oriY = offsetShaderY;

            while (Time.time < startTime + runTime)
            {
                timePast += Time.deltaTime;
                float factor = timePast / runTime;
                offsetShaderX = Mathf.Lerp(oriX, randomX, factor);
                offsetShaderY = Mathf.Lerp(oriY, RandomY, factor);
                yield return null;
            }
           // StartCoroutine(Curved());
        }

        void Shuffle(int[] a)
        {
            // Loops through array
            for (int i = a.Length - 1; i > 0; i--)
            {
                // Randomize a number between 0 and i (so that the range decreases each time)
                int rnd = Random.Range(0, i);

                // Save the value of the current i, otherwise it'll overright when we swap the values
                int temp = a[i];

                // Swap the new and old values
                a[i] = a[rnd];
                a[rnd] = temp;
            }
        }

        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.Playing)
            {
                //Player Instantiation 
                isPlay = true;
                playerBall = (GameObject)Instantiate(CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex],
                gameObject.transform.position, //Position
                Quaternion.Euler(0, CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex].transform.eulerAngles.y, 0)); //Rotation
                //playerBall.transform.parent = GameObject.FindGameObjectWithTag("playerTransform").transform;
                playerBall.GetComponent<PlayerBallController>().scoreEffect = scoreEffect;
                playerBall.GetComponent<PlayerBallController>().collisionEffect = collisionEffect;
                playerAnimator.SetBool("isRunning", true);
                playerBall.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
                isPlay = false;
        }

        public void AddScore(Vector3 position, GameObject parent, int score)
        {
            GameObject addScoreCanvas = (GameObject)Instantiate(addScore, position, Quaternion.Euler(Camera.main.transform.eulerAngles.x, 0, 0));
            addScoreCanvas.transform.GetComponentInChildren<Text>().text = "+" + score.ToString();
            addScoreCanvas.transform.SetParent(parent.transform);
            StartCoroutine(MoveAndFade(addScoreCanvas));

        }

        public void AddCoin(Vector3 position, GameObject parent)
        {
            GameObject addCoinCanvas = (GameObject)Instantiate(addCoin, position, Quaternion.Euler(Camera.main.transform.eulerAngles.x, 0, 0));
            addCoinCanvas.transform.SetParent(parent.transform);
            StartCoroutine(MoveAndFade(addCoinCanvas));

        }

        IEnumerator MoveAndFade(GameObject canvas)
        {
            var startTime = Time.time;
            float runTime = 1.5f;
            float timePast = 0;
            var oriPos = canvas.transform.localPosition;
            while (Time.time < startTime + runTime)
            {
                timePast += Time.deltaTime;
                float factor = timePast / runTime;
                if (canvas != null)
                {
                    canvas.transform.localPosition = oriPos + new Vector3(0, factor * 1.0f, 0);
                    canvas.GetComponent<CanvasGroup>().alpha = 1 - factor;
                }
                yield return null;
            }
            if (canvas != null)
            {
                canvas.transform.SetParent(null);
                Destroy(canvas);
            }

        }

        // Calls this when the player dies and game over
        public void Die()
        {
            StopAllCoroutines();

            if (PlayerDied != null)
                PlayerDied();
        }
        public void OpenBgScreen()
        {
            BackGroundSelectionScreen.SetActive(true);
        }
        public void SelectJungleTrack()
        {
            PlayerPrefs.SetInt("SelectTrack", 1);
            BackGroundSelectionScreen.SetActive(false);

        }
        public void SelectVeniceTrack()
        {
            PlayerPrefs.SetInt("SelectTrack", 2);
            BackGroundSelectionScreen.SetActive(false);
        }
        public void SelectMedievalTrack()
        {
            PlayerPrefs.SetInt("SelectTrack", 3);
            BackGroundSelectionScreen.SetActive(false);
        }

    }
}