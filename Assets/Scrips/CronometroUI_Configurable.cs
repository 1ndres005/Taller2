using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CronometroUI_Configurable : MonoBehaviour
{
    [Header("Tiempo")]
    public int minutos = 7;
    public int segundos = 0;

    [Header("Texto del cronómetro")]
    public TextMeshProUGUI textoCronometro;

    [Header("UI cuando termina")]
    public GameObject uiTiempoTerminado;

    [Header("Delay antes de reiniciar")]
    public float delayRecarga = 2f;

    private float tiempoRestante;
    private bool terminado = false;

    void Start()
    {
        tiempoRestante = (minutos * 60) + segundos;

        if (uiTiempoTerminado != null)
            uiTiempoTerminado.SetActive(false);

        ActualizarTexto();
    }

    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            terminado = true;
            StartCoroutine(FinDelTiempo());
        }

        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        int min = Mathf.FloorToInt(tiempoRestante / 60f);
        int seg = Mathf.FloorToInt(tiempoRestante % 60f);

        textoCronometro.text = $"{min:00}:{seg:00}";
    }

    IEnumerator FinDelTiempo()
    {
        if (uiTiempoTerminado != null)
            uiTiempoTerminado.SetActive(true);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delayRecarga);
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
