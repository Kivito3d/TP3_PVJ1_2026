using UnityEditor.VersionControl;
using UnityEngine;

// permite crear archivos asset desde el menu en proyecto
[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Stats/PVP/PlayerStats")]
public class PlayerStats : ScriptableObject
{
     [Header("Estadísticas Base")] 
    public float maxHealth = 100f; 
    public float currentHealth; 
    public float moveSpeed = 5f; 
    [Header("Progreso de Partida")] 
    public int currentScore; 
    public int currentLevel = 1;

    public MeshRenderer skin;
    public Asset prefab;

    // metodo para reiniciar valores al iniciar una nueva partida
    public void ResetStats() 
    { 
        currentHealth = maxHealth; 
        currentScore = 0; 
        currentLevel = 1; 
    } 
}
