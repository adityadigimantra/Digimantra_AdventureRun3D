using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeEnvironments : MonoBehaviour
{
    public GameObject[] Environments;
    public Button nextButton;
    public Button previousButton;
    private int enviroNumber;
    private int enviroNumber2;

    private void Start()
    {
       enviroNumber = 0;
       enviroNumber2 = 0;
        if (enviroNumber==0)
        {
            previousButton.GetComponent<Button>().interactable = false;
        }
        Environments[0].SetActive(true);
    }

    public void nextButtonFn(int number)
    {
        enviroNumber += number;
        switch(enviroNumber)
        {
            case 0:
                Environments[0].SetActive(true);
                Debug.Log("Value of Number" + enviroNumber);
                break;
            case 1:
                Environments[1].SetActive(true);
                Environments[0].SetActive(false);
                previousButton.GetComponent<Button>().interactable = true;
                Debug.Log("Value of Number" + enviroNumber);
                break;
            case 2:
                Environments[2].SetActive(true);
                Environments[1].SetActive(false);
                Debug.Log("Value of Number" + enviroNumber);
                break;
            case 3:
                Environments[3].SetActive(true);
                Environments[2].SetActive(false);
                Debug.Log("Value of Number" + enviroNumber);
                break;
            case 4:
                Environments[4].SetActive(true);
                Environments[3].SetActive(false);
                Debug.Log("Value of Number" + enviroNumber);
                nextButton.GetComponent<Button>().interactable = false;
               enviroNumber = 6;
                Debug.Log("Value of Number" + enviroNumber);
                break;

        }
    }

    public void previousButtonFn(int number)
    {
        enviroNumber += number;
        switch(enviroNumber)
        {
            case 5:
                Environments[3].SetActive(true);
                Environments[4].SetActive(false);
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                enviroNumber = 3;
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                nextButton.GetComponent<Button>().interactable = true;
                break;

            case 4:
                Environments[3].SetActive(true);
                Environments[4].SetActive(false);
                nextButton.GetComponent<Button>().interactable = true;
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                break;

            case 3:
                Environments[2].SetActive(true);
                Environments[3].SetActive(false);
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                break;
            case 2:
                Environments[2].SetActive(true);
                Environments[3].SetActive(false);
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                break;
            case 1:
                Environments[1].SetActive(true);
                Environments[2].SetActive(false);
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                break;
            case 0:
                Environments[0].SetActive(true);
                Environments[1].SetActive(false);
                Debug.Log("Previous Button-Value of Number" + enviroNumber);
                previousButton.GetComponent<Button>().interactable = false;
                break;
        }
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}




