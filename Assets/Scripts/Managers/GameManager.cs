using System;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


public class GameManager : MonoBehaviour
{
 
    public static GameManager Instance { get; private set; }
    //static
    public enum GameState
    {
        Start,
        Playing,
        Paused,
        Victory,
        GameOver,
    }
    //settings
    [Header("State Settings")]
    [SerializeField] private GameState currentState;
    public GameState CurrentState => currentState;

    [Header("Victory Settings")]
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesRemaining;

    public event Action<GameState> OnChangeState;
    public event Action<int> OnEnemyCountChanged;

    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//UI y escenas
    }
    void Start()
    {

        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        OnEnemyCountChanged?.Invoke(enemiesRemaining);
        ChangeState(GameState.Playing);
    }

    void Update()
    {
        //pausa inputs
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused();
        }
    }

   
    //switch estados
    public void ChangeState(GameState newState)
    {
        if (currentState == newState && newState != GameState.Playing) return;

        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 1f;//0f - 1f
                Debug.Log("Inicio el juego");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("<color=green>ESTADO: jugando</color>");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                UnlockCursor();
                Debug.Log("<color=yellow>ESTADO: pausa</color>");
                break;
            case GameState.Victory:
                Time.timeScale = 0f;
                UnlockCursor();
                //corrutina para detener el tiempo
                Debug.Log("<color=cyan>ESTADO: Victoria</color>");
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                UnlockCursor();
                Debug.Log("<color=red>ESTADO: Game Over</color>");
                break;
        }
        OnChangeState?.Invoke(currentState);
    }

    //pausa
    public void Paused()
    {
        if(currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
            
        }
        else if(currentState == GameState.Paused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ChangeState(GameState.Playing);
        }
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //notificaciones
    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.GameOver);
    }

    public void NotifyEnemyDefeat()
    {
        if (currentState != GameState.Playing) return;

        enemiesRemaining--;

        OnEnemyCountChanged?.Invoke(enemiesRemaining);
        if (enemiesRemaining <= 0)
        {
            ChangeState(GameState.Victory);
        }
    }

    public void NotifyGoalZone()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.Victory);
    }

}
