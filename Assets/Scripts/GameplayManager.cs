using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager>
{
    public delegate void GameStateCallback();

    public static event GameStateCallback OnGamePaused;
    public static event GameStateCallback OnGamePlaying;

    private HUDController m_HUD;
    private int m_points = 0;

    List<IRestartableObject> m_restartableObjects = new List<IRestartableObject>();

    public enum EGameState
    {
        Playing,
        Paused
    }

    private EGameState m_state;

    private void Start()
    {
        m_state = EGameState.Playing;
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
            case EGameState.Playing: { GameState = EGameState.Paused; } break;
            case EGameState.Paused: { GameState = EGameState.Playing; } break;
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
        foreach (var restartableObject in m_restartableObjects) restartableObject.DoRestart();

        Points = 0;
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
