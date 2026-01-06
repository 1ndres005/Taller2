using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelayedSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;
    public float delaySeconds = 5f;

    void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(sceneToLoad);
    }
}
