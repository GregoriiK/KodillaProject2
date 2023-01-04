using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public bool Pause = false;

    void Start()
    {
        Pause = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Pause = !Pause;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

        if (Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }
}
