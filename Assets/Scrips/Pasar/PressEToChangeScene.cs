using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEToChangeScene : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPanel;   // Panel o Canvas de la UI

    [Header("Scene")]
    public string sceneToLoad;   // Nombre exacto de la escena

    [Header("Key")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInside = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false); // UI oculta al inicio
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }
}
