using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [Header("Receta")]
    public int ingredientesNecesarios = 4;
    public GameObject objetoFinal;

    [Header("Objetos a desaparecer al completar")]
    public GameObject[] objetosADesaparecer;

    int ingredientesActuales = 0;
    bool completada = false;

    void Start()
    {
        // El objeto final normalmente empieza oculto
        if (objetoFinal != null)
            objetoFinal.SetActive(false);

        // ✅ IMPORTANTE: NO tocamos objetosADesaparecer aquí
        // para que mantengan el estado que tú les dejaste en el Inspector.
    }

    public void RegistrarIngredienteCorrecto()
    {
        if (completada) return;

        ingredientesActuales++;
        Debug.Log($"Ingrediente agregado. Total: {ingredientesActuales}/{ingredientesNecesarios}");

        if (ingredientesActuales >= ingredientesNecesarios)
        {
            completada = true;
            DesbloquearObjetoFinal();
        }
    }

    void DesbloquearObjetoFinal()
    {
        if (objetoFinal != null)
            objetoFinal.SetActive(true);

        if (objetosADesaparecer != null)
        {
            foreach (GameObject obj in objetosADesaparecer)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        Debug.Log("¡Receta completada! Objeto final desbloqueado.");
    }
}
