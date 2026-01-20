using UnityEngine;

public class CameraDolly_M_y_VuelveSiObjetoDesaparece : MonoBehaviour
{
    [Header("Cámara")]
    public Transform camTransform; // Main Camera

    [Header("Destino del Dolly")]
    public Transform puntoDestino;
    public float duracionIda = 1.2f;
    public float duracionVuelta = 1.2f;

    [Header("UI Interacción (Presiona M)")]
    public GameObject uiInteract;

    [Header("Scripts que mueven la cámara (se desactivan durante el dolly)")]
    public ThirdPersonCameraRig thirdPersonRig;
    public CameraTargetSmooth cameraTargetSmooth;

    [Header("Al terminar la VUELTA: objetos que desaparecen")]
    public GameObject[] objetosADesaparecerAlVolver; // 👈 arrastra aquí los objetos

    bool jugadorCerca = false;
    bool enCinematica = false;
    bool camaraEnDestino = false;

    Vector3 posGuardada;
    Quaternion rotGuardada;

    void Start()
    {
        if (camTransform == null && Camera.main != null)
            camTransform = Camera.main.transform;

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca || enCinematica) return;
        if (camTransform == null || puntoDestino == null) return;

        // 👉 M SOLO SIRVE PARA IR
        if (!camaraEnDestino && Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MoverCamaraIda());
        }
    }

    // ✅ SOLO SE LLAMA DESDE OTROS SCRIPTS (ej: diálogo)
    public void ForzarVuelta()
    {
        if (enCinematica) return;
        if (!camaraEnDestino) return;

        StartCoroutine(MoverCamaraVuelta());
    }

    // ==========================
    //            IDA
    // ==========================
    System.Collections.IEnumerator MoverCamaraIda()
    {
        enCinematica = true;

        // 🔒 Apagar scripts que pisan la cámara
        if (thirdPersonRig != null) thirdPersonRig.enabled = false;
        if (cameraTargetSmooth != null) cameraTargetSmooth.enabled = false;

        // Guardar estado original
        posGuardada = camTransform.position;
        rotGuardada = camTransform.rotation;

        if (uiInteract != null)
            uiInteract.SetActive(false);

        Vector3 inicioPos = camTransform.position;
        Quaternion inicioRot = camTransform.rotation;

        float t = 0f;
        float dur = Mathf.Max(duracionIda, 0.0001f);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            camTransform.position = Vector3.Lerp(inicioPos, puntoDestino.position, t);
            camTransform.rotation = Quaternion.Slerp(inicioRot, puntoDestino.rotation, t);
            yield return null;
        }

        camTransform.position = puntoDestino.position;
        camTransform.rotation = puntoDestino.rotation;

        camaraEnDestino = true;
        enCinematica = false;
    }

    // ==========================
    //          VUELTA
    // ==========================
    System.Collections.IEnumerator MoverCamaraVuelta()
    {
        enCinematica = true;

        Vector3 inicioPos = camTransform.position;
        Quaternion inicioRot = camTransform.rotation;

        float t = 0f;
        float dur = Mathf.Max(duracionVuelta, 0.0001f);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            camTransform.position = Vector3.Lerp(inicioPos, posGuardada, t);
            camTransform.rotation = Quaternion.Slerp(inicioRot, rotGuardada, t);
            yield return null;
        }

        camTransform.position = posGuardada;
        camTransform.rotation = rotGuardada;

        camaraEnDestino = false;
        enCinematica = false;

        // 🔓 Reactivar scripts de cámara
        if (cameraTargetSmooth != null) cameraTargetSmooth.enabled = true;
        if (thirdPersonRig != null) thirdPersonRig.enabled = true;

        // ✅ AL TERMINAR LA VUELTA: desaparecer objetos
        if (objetosADesaparecerAlVolver != null)
        {
            for (int i = 0; i < objetosADesaparecerAlVolver.Length; i++)
            {
                if (objetosADesaparecerAlVolver[i] != null)
                    objetosADesaparecerAlVolver[i].SetActive(false);
            }
        }

        if (jugadorCerca && uiInteract != null)
            uiInteract.SetActive(true);
    }

    // ==========================
    //          TRIGGERS
    // ==========================
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = true;

        if (!camaraEnDestino && uiInteract != null)
            uiInteract.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = false;

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }
}
