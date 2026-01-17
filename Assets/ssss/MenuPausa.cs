using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuUI;

    private bool juegoPausado = false;

    void Start()
    {
        // 🔒 Bloquea el cursor por defecto al iniciar el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        // 🖱️ Libera el cursor para navegar en el menú
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reanudar()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        // 🔒 Oculta y bloquea el cursor al volver al juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
