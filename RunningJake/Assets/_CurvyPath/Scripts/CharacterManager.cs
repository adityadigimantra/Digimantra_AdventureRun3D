using UnityEngine;
using System.Collections;

namespace CurvyPath
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";

        public int CurrentCharacterIndex
        {
            get
            {
                return PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY, 0);
            }
            set
            {
                PlayerPrefs.SetInt(CURRENT_CHARACTER_KEY, value);
                PlayerPrefs.Save();
                Debug.Log(PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY));
            }
        }

        public GameObject[] characters;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            Debug.Log("CurrentCharacteris"+PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY));
            int currentCharacter = PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY);
            if(currentCharacter==0)
            {
                GameManager.Instance.speed =70;
                GameManager.Instance.limitSpeed = 100;
            }
            if(currentCharacter==1)
            {
                GameManager.Instance.speed =50;
                GameManager.Instance.limitSpeed =70;
            }
            if(currentCharacter==2)
            {
                GameManager.Instance.speed = 50;
                GameManager.Instance.limitSpeed = 70;
            }
        }
    }
}