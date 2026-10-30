using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
