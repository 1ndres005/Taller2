using UnityEngine;
using TMPro;

public class NatillaQuestText : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI texto;

    [Header("PASO 1: Slots que deben estar ocupados (olla, natilla, leche)")]
    [Tooltip("Arrastra aquí los TRANSFORMS de los puntos/slots donde quedan los objetos colocados.")]
    public Transform[] slotsIngredientes;  // cuando todos tengan hijos -> paso 2

    [Header("PASO 2: Trigger de interacción (plato iluminado)")]
    public TriggerInteractR triggerPlato;

    [Header("PASO 3: Trigger de interacción (estufa / revolver)")]
    public TriggerInteractR triggerEstufa;

    [Header("PASO 4: Slot de la mesa final (cuando se coloca la natilla)")]
    public Transform[] slotsMesaFinal; // cuando todos tengan hijos -> final

    [Header("Teclas")]
    public KeyCode teclaPlato = KeyCode.E;  // dijiste E en el plato
    public KeyCode teclaR = KeyCode.R;      // para estufa (y los que quieras)

    private int paso = 0;

    void Start()
    {
        MostrarPaso0();

        // Configurar triggers si existen
        if (triggerPlato != null)
        {
            triggerPlato.key = teclaPlato;
            triggerPlato.onInteract += OnPlatoInteract;
        }

        if (triggerEstufa != null)
        {
            triggerEstufa.key = teclaR;
            triggerEstufa.onInteract += OnEstufaInteract;
        }
    }

    void Update()
    {
        // Paso 1 -> cuando ingredientes completos
        if (paso == 0 && SlotsOcupados(slotsIngredientes))
        {
            paso = 1;
            MostrarPaso1();
        }

        // Paso final -> cuando mesa final llena (solo si ya pasaste por estufa)
        if (paso == 3 && SlotsOcupados(slotsMesaFinal))
        {
            paso = 4;
            MostrarPasoFinal();
        }
    }

    // ---------------- TEXTOS ----------------

    void MostrarPaso0()
    {
        if (texto == null) return;

        texto.text =
            "Ve y busca las cosas para revolver la natilla\n\n" +
            "° Olla\n" +
            "° Natilla\n" +
            "° Leche\n\n" +
            "Búscalos en los objetos que están titilando.";
    }

    void MostrarPaso1()
    {
        if (texto == null) return;

        texto.text =
            "Ve adonde alumbra al lado del plato\n" +
            "e interactúa con la tecla E.";
    }

    void MostrarPaso2()
    {
        if (texto == null) return;

        texto.text =
            "Ve y cocínalo en la estufa\n" +
            "y revuélvelo (tecla R).";
    }

    void MostrarPaso3()
    {
        if (texto == null) return;

        texto.text =
            "Ve y ponlo en la mesa.";
    }

    void MostrarPasoFinal()
    {
        if (texto == null) return;

        texto.text =
            "Ya finalizaste la natilla, disfrútala.\n" +
            "Puedes seguir festejando donde quieras.";
    }

    // ------------- EVENTOS DE INTERACCIÓN --------------

    void OnPlatoInteract()
    {
        if (paso != 1) return;
        paso = 2;
        MostrarPaso2();
    }

    void OnEstufaInteract()
    {
        if (paso != 2) return;
        paso = 3;
        MostrarPaso3();
    }

    // ---------------- HELPERS ----------------

    bool SlotsOcupados(Transform[] slots)
    {
        if (slots == null || slots.Length == 0) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) return false;

            // "ocupado" = tiene al menos un hijo (tu sistema ya coloca el objeto como hijo del slot)
            if (slots[i].childCount == 0)
                return false;
        }

        return true;
    }
}
