using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [Header("Health Settings")]

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;


    [SerializeField] private bool isDead;
    public bool IsDead
    {
        get => isDead;
          
    }

    [SerializeField] private GameObject vfx;

    private Rigidbody rb;
    private Collider col;
    private Attack attack;
    private MeshRenderer mesh;

    public event Action<float> OnChangeHealthPlayer;
    private void Awake() 
    { 
        //currentHealth = maxHealth; 
        isDead = false; 
        playerStats.ResetStats();
        enemyStats.ResetStats();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        attack = GetComponent<Attack>();
        mesh = GetComponent<MeshRenderer>();
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        if (gameObject.CompareTag("Player"))
        {
            playerStats.currentHealth -= amount;
            OnChangeHealthPlayer?.Invoke(playerStats.currentHealth);

            if (playerStats.currentHealth <= 0f)
            {
                Dead();
            }
        }

        if (gameObject.CompareTag("Character"))
        {
            enemyStats.currentHealth -= amount;

            if (enemyStats.currentHealth <= 0f)
            {
                Dead();
            }
        }


    }
    private void Dead() 
    { 
        isDead = true;

        //agregar condicional comparando tags player y character, notificar al gameManager

        if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance.NotifyPlayerDead();
        }
        else if(gameObject.CompareTag("Character"))
        {
            GameManager.Instance.NotifyEnemyDefeat();
        }

        vfx.SetActive(true);
        //activar vfx
        StartCoroutine(DestroyTransition());
        //iniciar Corrutina


        Debug.Log("Character eliminado"); 
        // Notificación al GameManager si existiera 
        //GameManager.Instance.ChangeState(GameState.GameOver);
        //Destroy object
        //efectos
        //animaciones
    }

    IEnumerator DestroyTransition()
    {
        col.enabled = false;
        rb.isKinematic = true;
        mesh.enabled = false;
        attack.enabled = false;

        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);
    }
    //crear corrutina deshabilitando componentes y destruyendo el objeto

}
