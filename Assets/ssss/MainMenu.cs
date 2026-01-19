using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nombre o índice de la escena a cargar
    [SerializeField] private string sceneToLoad;

    // BOTÓN JUGAR
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // BOTÓN SALIR
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
