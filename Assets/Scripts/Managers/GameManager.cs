using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //static


    //settings
    [Header("State Settings")]
    [SerializeField] private GameState currentState;
    public GameState CurrentState => currentState;

    [Header("Victory Settings")]
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesRemaining;


    //eventos estado y cambio total enemigos

    public event Action<GameState> OnChangeState;

    public event Action<int> OnEnemyChange;

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
        //estados iniciales
        //EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        OnEnemyChange?.Invoke(enemiesRemaining);
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
                Debug.Log("<color=yellow>ESTADO: pausa</color>");
                break;
            case GameState.Victory:
                Time.timeScale = 0f;
                //corrutina para detener el tiempo
                Debug.Log("<color=cyan>ESTADO: Victoria</color>");
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
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
            ChangeState(GameState.Playing);
        }
    }

    //lockmouse metodo

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

        OnEnemyChange?.Invoke(enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            ChangeState(GameState.Victory);
        }
    }


}
