using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [Header("Receta")]
    public int ingredientesNecesarios = 4;   // cuántos ingredientes hacen la receta
    public GameObject objetoFinal;           // el objeto que aparece al completar (plato listo, etc.)

    int ingredientesActuales = 0;
    bool completada = false;

    void Start()
    {
        if (objetoFinal != null)
            objetoFinal.SetActive(false); // al inicio oculto
    }

    public void RegistrarIngredienteCorrecto()
    {
        if (completada)
            return;

        ingredientesActuales++;

        Debug.Log("Ingrediente agregado. Total: " + ingredientesActuales + "/" + ingredientesNecesarios);

        if (ingredientesActuales >= ingredientesNecesarios)
        {
            completada = true;
            DesbloquearObjetoFinal();
        }
    }

    void DesbloquearObjetoFinal()
    {
        if (objetoFinal != null)
        {
            objetoFinal.SetActive(true);
            Debug.Log("¡Receta completada! Objeto final desbloqueado.");
        }
    }
}
