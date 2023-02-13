using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI2 : MonoBehaviour
{
    public GameObject WinGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if ( GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            Time.timeScale = 0f;
            WinGame.SetActive(true);
        }
    }
}
