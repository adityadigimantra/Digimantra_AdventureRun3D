using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CurvyPath
{
    public enum GameState
    {
        Prepare,
        Playing,
        Paused,
        PreGameOver,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
          get;
          private set;
        }

        public static event System.Action<GameState, GameState> GameStateChanged;   //event

        public bool isRestart;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        public static int GameCount
        {
            get { return _gameCount; }
            private set { _gameCount = value; }
        }

        private static int _gameCount = 0;

        [Header("Set the target frame rate for this game")]
        [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
        public int targetFrameRate = 30;

        [Header("Current game state")]
        [SerializeField]
        private GameState _gameState = GameState.Prepare;

        // List of public variable for gameplay tweaking
        [Header("Gameplay Config")]

        [Range(0f, 1f)]
        public float coinFrequency = 0.1f;

        public float width = 50;

        public float spacing = 12;

        public float speed = 200;

        public float lenght = 25;

        public float jungleTerrainLenght = 25;

        public float VeniceCityLenght = 25;

        public float medievalCityLenght = 25;

        public float timeCreateBall = 0.5f;

        public int limitPlane = 60;

        public int limitJungleTerrain = 3;

        public float swipeForce = 2000;

        public float increaseSpeedRatio = 0.05f;

        public float limitSpeed = 300;

        public Color planeFirstColor = Color.white;

        public Color planeSecondColor = Color.gray;

        public Color color1 = Color.yellow;

        public Color color2 = Color.green;

        public Color color3 = Color.red;

        public Color color4 = Color.blue;

        [Header("Starting Text")]
        public GameObject StartingTextPanel;
        public GameObject ControlTextPanel;
        public GameObject[] StartingText;
        public GameObject[] ControlsText;



        [Header("Random Texts")]
        public GameObject[] RandomTexts;
        public int randomNumber;
        public GameObject randomeTextPanel;
        public bool numberSelected = false;
        public bool isGameOver=false;
        public bool GameStartNow = false;
        public GameObject GameoverText;

        [Header("Tutorial Panel")]
        public GameObject TutorialPanel;
        public GameObject[] Tutorials;
        public bool tutorialShown = false;

        // List of public variables referencing other objects
        [Header("Object References")]
        public PlayerController playerController;

        [Header("For Characters 1 to 3")]
        [SerializeField]
        private Material materialObjectColor1;

        [SerializeField]
        private Material materialObjectColor2;

        [SerializeField]
        private Material materialObjectColor3;
        [SerializeField]
        private Material materialChangeColor1;

        [SerializeField]
        private Material materialChangeColor2;

        [SerializeField]
        private Material materialChangeColor3;

        [Header("For Characters 4 to 6")]
        [SerializeField]
        private Material materialObjectColor4;

        [SerializeField]
        private Material materialObjectColor5;

        [SerializeField]
        private Material materialObjectColor6;
        [SerializeField]
        private Material materialChangeColor4;

        [SerializeField]
        private Material materialChangeColor5;

        [SerializeField]
        private Material materialChangeColor6;




        [Header("New Balls Texture")]

        [SerializeField]
        private Material material1ObjectColor1;

        [SerializeField]
        private Material material1ObjectColor2;

        [SerializeField]
        private Material material1ObjectColor3;

        private Material material1ChangeColor1;

        [SerializeField]
        private Material material1ChangeColor2;

        [SerializeField]
        private Material material1ChangeColor3;

        void OnEnable()
        {
            PlayerController.PlayerDied += PlayerController_PlayerDied;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
            if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 0 || PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 1 || PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 2)
            {
                materialObjectColor1.SetColor("_Color", color1);
                materialObjectColor2.SetColor("_Color", color2);
                materialObjectColor3.SetColor("_Color", color3);

                materialChangeColor1.SetColor("_Color", color1);
                materialChangeColor2.SetColor("_Color", color2);
                materialChangeColor3.SetColor("_Color", color3);
            }
            if (PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 4 || PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 5 || PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY) == 6)
            {
                materialObjectColor4.SetColor("_Color", color1);
                materialObjectColor5.SetColor("_Color", color2);
                materialObjectColor6.SetColor("_Color", color3);

                materialChangeColor4.SetColor("_Color", color1);
                materialChangeColor5.SetColor("_Color", color2);
                materialChangeColor6.SetColor("_Color", color3);
            }

        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            
            if (!isRestart)
            {
                //FindObjectOfType<UIManager>().ShowCharacterSelectionScene();
            }
            Application.targetFrameRate = targetFrameRate;
            ScoreManager.Instance.Reset();
            GameStartNow = false;
            //if(PlayerPrefs.GetInt("TutorialShown") ==0)
            //{
            //    StartCoroutine(TutorialSection());
            //}
            PrepareGame();
        }


        // Listens to the event when player dies and call GameOver
        void PlayerController_PlayerDied()
        {
            GameOver();
        }

        // Make initial setup and preparations before the game can be played
        public void PrepareGame()
        {
            GameState = GameState.Prepare;

            // Automatically start the game if this is a restart.
            if (isRestart)
            {
                isRestart = false;

                StartGame();
                FindObjectOfType<ObjectPooling>().PoolingObjFunc();
            }
        }

        // A new game official starts
        public void  StartGame()
        {
            StartCoroutine(StartingTextCorout());
            int chooseitems = Random.Range(1, 5);
            switch (chooseitems)
            {
                case 1:
                    Debug.Log("Items Selected-1");
                    PlayerPrefs.SetInt("ChooseItems", 1);
                    break;
                case 2:
                    Debug.Log("Items Selected-2");
                    PlayerPrefs.SetInt("ChooseItems", 2);
                    break;
                case 3:
                    Debug.Log("Items Selected-3");
                    PlayerPrefs.SetInt("ChooseItems", 3);
                    break;
                case 4:
                    Debug.Log("Items Selected-4");
                    PlayerPrefs.SetInt("ChooseItems", 4);
                    break;
                case 5:
                    Debug.Log("Items Selected");
                    PlayerPrefs.SetInt("ChooseItems", 5);
                    break;
            }
            
            StartCoroutine(StartControlText());
            StartCoroutine(randomTextCo());
            GameoverText.SetActive(false);

            //sound Start Playing
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
            }

        }

        IEnumerator StartControlText()
        {
           //if (PlayerPrefs.GetInt("TutorialShown") == 0)
            {
                
                yield return new WaitForSeconds(5f);
                ControlTextPanel.SetActive(true);
                foreach (GameObject g in ControlsText)
                {
                    g.SetActive(true);
                    yield return new WaitForSeconds(3f);
                    g.SetActive(false);
                  // PlayerPrefs.SetInt("TutorialShown", 1);
                }
            }
            ControlTextPanel.SetActive(false);
        }
        public IEnumerator randomTextCo()
        {
            yield return new WaitForSeconds(17f);
            RandomTexts[0].SetActive(true);
            yield return new WaitForSeconds(3f);
            RandomTexts[0].SetActive(false);
            for (int i=0;i<=3;i++)
            {

                randomNumber = Random.Range(2,5);
                Debug.Log("Random Number" + randomNumber);
                switch (randomNumber)
                {
                    case 1:
                        
                        RandomTexts[0].SetActive(true);
                        yield return new WaitForSeconds(4f);
                        RandomTexts[0].SetActive(false);
                        break;

                    case 2:
                        yield return new WaitForSeconds(30f);
                        RandomTexts[1].SetActive(true);
                        yield return new WaitForSeconds(4f);
                        RandomTexts[1].SetActive(false);
                        break;
                    case 3:
                        yield return new WaitForSeconds(30f);
                        RandomTexts[2].SetActive(true);
                        yield return new WaitForSeconds(4f);
                        RandomTexts[2].SetActive(false);
                        break;
                    case 4:
                        yield return new WaitForSeconds(30f);
                        RandomTexts[3].SetActive(true);
                        yield return new WaitForSeconds(4f);
                        RandomTexts[3].SetActive(false);
                        break;
                    case 5:
                        yield return new WaitForSeconds(30f);
                        RandomTexts[4].SetActive(true);
                        yield return new WaitForSeconds(4f);
                        RandomTexts[4].SetActive(false);
                        break;
                }
                i = 0;

            }
            
        }

        public IEnumerator TutorialSection()
        {
            yield return new WaitForSeconds(1f);
            TutorialPanel.SetActive(true);
            for(int i=0;i<Tutorials.Length;i++)
            {
                
                Tutorials[0].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[0].SetActive(false);
                
                yield return new WaitForSeconds(1f);
                Tutorials[1].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[1].SetActive(false);
                
                yield return new WaitForSeconds(1f);
                Tutorials[2].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[2].SetActive(false);
                
                yield return new WaitForSeconds(1f);
                Tutorials[3].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[3].SetActive(false);
                
                yield return new WaitForSeconds(1f);
                Tutorials[4].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[4].SetActive(false);
                
                yield return new WaitForSeconds(1f);
                Tutorials[5].SetActive(true);
                yield return new WaitForSeconds(3f);
                Tutorials[5].SetActive(false);
                TutorialPanel.SetActive(false);
                PlayerPrefs.SetInt("TutorialShown", 1);
            }
        }


        // Called when the player died
        public void GameOver()
        {
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.StopMusic();
            }

            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            GameState = GameState.GameOver;
            GameCount++;
            isGameOver = true;
            StopCoroutine(randomTextCo());
            StartCoroutine(PlayAds());
            PlayerPrefs.DeleteKey(CURRENT_CHARACTER_KEY);
            Destroy(randomeTextPanel);
            GameoverText.SetActive(true);

            // Add other game over actions here if necessary
        }

        IEnumerator StartingTextCorout()
        {
           //yield return new WaitForSeconds(1f);
            StartingTextPanel.SetActive(true);
            foreach (GameObject g in StartingText)
            {
                g.SetActive(true);
                yield return new WaitForSeconds(1.25f);
                g.SetActive(false);
            }
            StartingTextPanel.SetActive(false);
            GameState = GameState.Playing;
           // GameState = GameState.Idle;
        }

        IEnumerator PlayAds()
        {
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<UnityAds>().ShowInterstialAd();
        }

        // Start a new game
        public void RestartGame(float delay = 0)
        {
            isRestart = true;
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            PlayerPrefs.DeleteKey(CURRENT_CHARACTER_KEY);
            SceneManager.LoadScene(2);
        }

        public void HidePlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(false);
        }

        public void ShowPlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(true);
        }

        private void Update()
        {
            Debug.Log("Game State is " + GameState);
        }

    }
}
       
    

    
