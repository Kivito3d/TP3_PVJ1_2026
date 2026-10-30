using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header ("UI Objects")]
    [SerializeField]private GameObject Pause_Panel;
    [SerializeField] private GameObject Hud_Panel;
    [SerializeField] private GameObject Victory_Panel;
    [SerializeField] private GameObject GameOver_Panel;
    [SerializeField]private TextMeshProUGUI enemiesText;
    [SerializeField] private Slider slider;


    [Header ("References")]
    [SerializeField] private HealthSystem playerHealth;



    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState -= HandleStateChange;
            GameManager.Instance.OnEnemyCountChanged -= UpdateEnemyHUD;
        }
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState += HandleStateChange;
            GameManager.Instance.OnEnemyCountChanged += UpdateEnemyHUD;
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }

    }

    //metodo se ejecuta cada ves que cambia el estado
    private void HandleStateChange(GameManager.GameState newState)
    {
        Debug.Log("Entro");
        if (Pause_Panel) Pause_Panel.SetActive(false);
        if (Victory_Panel) Victory_Panel.SetActive(false);
        if(GameOver_Panel) GameOver_Panel.SetActive(false);

        switch (newState)
        {
            case GameManager.GameState.Paused:
                if (Pause_Panel) Pause_Panel.SetActive(true);
                break;
            case GameManager.GameState.Victory:
                if (Victory_Panel) Victory_Panel.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                if (GameOver_Panel) GameOver_Panel.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                
                break;
        }
    }

    private void UpdateEnemyHUD(int currentEnemies)
    {
        if (enemiesText != null)
        {
            enemiesText.text = $"{currentEnemies}";
        }
    }


    private void UpdateHealthBar(float healthPercentage)
    {
        if (slider != null)
        {
            slider.value = healthPercentage;
        }
    }

    public void ChangeScene(SceneAsset scene)
    {
        SceneManager.LoadScene(scene.name);
    }
    public void TogglePanel(GameObject Panel)
    {
        if(Panel !=null)
        {
            bool toggle = Panel.activeInHierarchy ? false : true;
            Panel.SetActive(toggle);
        }
    }
    public void OpenPanel(GameObject panelToOpen)
    { 
        // Desactivamos todos los paneles
        // cargados en memoria 

        // Activamos solo el elgido
        panelToOpen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
