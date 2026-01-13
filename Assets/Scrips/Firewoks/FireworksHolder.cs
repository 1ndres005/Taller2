using UnityEngine;
using System.Collections;

public class LoopActivator : MonoBehaviour
{
    [System.Serializable]
    public class LoopElement
    {
        public GameObject target;
        public float activeTime = 2f;
        public float inactiveTime = 2f;
    }

    [Header("Elementos en Loop")]
    public LoopElement element1;
    public LoopElement element2;

    void Start()
    {
        if (element1.target != null)
            StartCoroutine(LoopRoutine(element1));

        if (element2.target != null)
            StartCoroutine(LoopRoutine(element2));
    }

    IEnumerator LoopRoutine(LoopElement element)
    {
        while (true)
        {
            // ACTIVAR
            element.target.SetActive(true);
            yield return new WaitForSeconds(element.activeTime);

            // DESACTIVAR
            element.target.SetActive(false);
            yield return new WaitForSeconds(element.inactiveTime);
        }
    }
}
