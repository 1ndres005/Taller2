using UnityEngine;
using Unity.Cinemachine;

public class CambioCamaraSimple : MonoBehaviour
{
    public CinemachineCamera camaraA;
    public CinemachineCamera camaraB;

    private bool enCamaraA = true;

    void Update()
    {
        // Cambia de cámara al presionar la tecla "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (enCamaraA)
            {
                camaraA.Priority = 0;
                camaraB.Priority = 10;
            }
            else
            {
                camaraA.Priority = 10;
                camaraB.Priority = 0;
            }

            enCamaraA = !enCamaraA;
        }
    }
}
