using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager>
{
    public delegate void GameStateCallback();

    public static event GameStateCallback OnGamePaused;
    public static event GameStateCallback OnGamePlaying;

    public GameSettingsDatabase GameDatabase;

    public GameObject InGameObjects;

    private HUDController m_HUD;
    private int m_points = 0;

    public int missCount = 0;

    List<IRestartableObject> m_restartableObjects = new List<IRestartableObject>();

    public enum EGameState
    {
        Playing,
        Paused
    }

    private EGameState m_state;


    private void Start()
    {
        GetAllRestartableObjects();
        m_HUD = FindObjectOfType<HUDController>();
        Points = 0;
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) PlayPause();

        if (Input.GetKeyDown(KeyCode.Escape)) GameState = EGameState.Paused;

        if (Input.GetKeyUp(KeyCode.R)) Restart();
    }


    public void PlayPause()
    {
        switch (GameState)
        {
            case EGameState.Playing: { GameState = EGameState.Paused; }  break;
            case EGameState.Paused: { GameState = EGameState.Playing; }  break;
        }
    }

    public EGameState GameState
    {
        get { return m_state; }
        set 
        {
            m_state = value;

            switch(m_state)
            {
                case EGameState.Paused:
                {
                    if (OnGamePaused != null) OnGamePaused();
                    break;
                }
                case EGameState.Playing:
                {
                    if (OnGamePlaying != null) OnGamePlaying();
                    break;
                }
            }
        }
    }

    private void GetAllRestartableObjects()
    {
        m_restartableObjects.Clear();

        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            IRestartableObject[] childrenInterfaces = rootGameObject.GetComponentsInChildren<IRestartableObject>();

            foreach (var childInterface in childrenInterfaces)
            {
                m_restartableObjects.Add(childInterface);
            }
        }
    }

    public void Restart()
    {
        // 3. Optimization - for loop instead of foreach.
        for (int i = 0; i < m_restartableObjects.Count;i++) m_restartableObjects[i].DoRestart(); 
        //foreach (var restartableObject in m_restartableObjects) restartableObject.DoRestart();

        Points = 0;

        GameState = EGameState.Playing;
    }

    public int Points
    {
        get { return m_points; }
        set
        {
            m_points = value;
            m_HUD.UpdatePoints(m_points);
        }
    }
}
