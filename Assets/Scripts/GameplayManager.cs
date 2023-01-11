using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameplayManager : Singleton<GameplayManager>
{
    public delegate void GameStateCallback();

    public static event GameStateCallback OnGamePaused;
    public static event GameStateCallback OnGamePlaying;

    private HUDController m_HUD;
    private PauseMenuController m_pauseMenu;
    private int m_points = 0;
    private bool condition;

    public int LifetimeHits;

    List<IRestartableObject> m_restartableObjects = new List<IRestartableObject>();

    public enum EGameState
    {
        Playing,
        Paused
    }

    private EGameState m_state;

    private void OnEnable()
    {
        condition = true;
    }

    private void Start()
    {
        m_state = EGameState.Playing;
        GetAllRestartableObjects();
        m_HUD = FindObjectOfType<HUDController>();
        m_pauseMenu = FindObjectOfType<PauseMenuController>();
        Points = 0;
        LoadHitcount();

        //bug creator - DELETE later
        /*
        int[] Test = new int[2] { 0, 0 };

        try
        {
            TestThrow();
            Test[2] = 1;
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.Log("Indez excepction: " + e.Message);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Null exception: " + e.Message);
        }
        */
        //StartCoroutine(FrameCounter());
        //StartCoroutine(TestCoroutine());
        //AsyncFrameCounter();
        //SecondTestAsync();
    }

    public void SaveHitcount()
    {
        PlayerPrefs.SetInt("LifetimeHits", LifetimeHits);
    }

    public void LoadHitcount()
    {
        LifetimeHits = PlayerPrefs.GetInt("LifetimeHits");
        Debug.Log("Lifetime Hits: " + LifetimeHits);
    }

    async void AsyncFrameCounter()
    {
        while (condition)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            Debug.Log(Time.frameCount / Time.time);
        }
    }

    private void OnDisable()
    {
        condition = false;
    }

    async Task TestAsync()
    {
        Debug.Log("Starting async method");
        await Task.Delay(TimeSpan.FromSeconds(3));
        Debug.Log("Async done after 3sec.");
    }

    async void SecondTestAsync()
    {
        Debug.Log("Starting second async method");
        await TestAsync();
        Debug.Log("Second async done");
    }

    private IEnumerator FrameCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(Time.frameCount / Time.time); //podpowiedŸ w æwiczeniu sugeruje, ¿e autorowi chodzi³o o œredni¹ iloœæ klatek na sekundê. Inaczej by³oby 1/Time.deltaTime
        }
    }

    private IEnumerator TestCoroutine()
    {
        Debug.Log("Starting coroutine method");
        yield return new WaitForSeconds(3);
        Debug.Log("Coroutine done after 3 seconds");
    }

    private void TestThrow()
    {
        throw new NullReferenceException("Test exception");
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
                        m_HUD.gameObject.SetActive(false);
                        m_pauseMenu.gameObject.SetActive(true);
                        break;
                }
                case EGameState.Playing:
                {
                    if (OnGamePlaying != null) OnGamePlaying(); 
                        m_HUD.gameObject.SetActive(true);
                        m_pauseMenu.gameObject.SetActive(false);
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
