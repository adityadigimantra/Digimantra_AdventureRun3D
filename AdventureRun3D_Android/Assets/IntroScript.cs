using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangingSceneToNextLevel());   
    }
    IEnumerator ChangingSceneToNextLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
   
}
