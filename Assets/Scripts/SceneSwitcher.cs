using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public float delay;
    public int SceneIndex;

    public void SwitchSceneDelay() {
        StartCoroutine(SwitchAfterTime());
    }

    public void SwitchScene() {
        SceneManager.LoadScene(SceneIndex);
    }

    public IEnumerator SwitchAfterTime() {
        yield return new WaitForSeconds(delay);
        SwitchScene();
    }
}
