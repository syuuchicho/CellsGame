using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void SceneReset()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeSceneName);
    }

    public void NextScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        int buildIndex = scene.buildIndex;

        buildIndex = buildIndex + 1;

        SceneManager.LoadScene(buildIndex);
    }

    public void ChangeScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    public void BossScene()
    {
        SceneManager.LoadScene("bossScene");
    }
}
