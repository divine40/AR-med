using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string firstLearningScene = "HeartScene";

    public void StartLearning()
    {
        SceneManager.LoadScene(firstLearningScene);
    }

    public void ExitApp()
    {
        Debug.Log("Exit button pressed");

        Application.Quit();
    }
}