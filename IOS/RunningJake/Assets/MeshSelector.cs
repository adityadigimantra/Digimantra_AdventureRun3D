using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSelector : MonoBehaviour
{
    public GameObject[] fooditems;
    void Update()
    {
        for(int i=0;i<=fooditems.Length;i++)
        {
            if(PlayerPrefs.GetInt("ChooseItems")==1)
            {
                fooditems[0].SetActive(true);
            }
            if(PlayerPrefs.GetInt("ChooseItems")==2)
            {
                fooditems[1].SetActive(true);
            }
            if (PlayerPrefs.GetInt("ChooseItems") == 3)
            {
                fooditems[2].SetActive(true);
            }
            if (PlayerPrefs.GetInt("ChooseItems") == 4)
            {
                fooditems[3].SetActive(true);
            }
            if (PlayerPrefs.GetInt("ChooseItems") == 5)
            {
                fooditems[4].SetActive(true);
            }
        }
    }
}
