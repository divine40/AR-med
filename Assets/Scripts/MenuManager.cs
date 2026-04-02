using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string firstLearningScene = "HeartScene";
    public string menuScene = "MenuScene";

    public GameObject privacyCanvas; // your new canvas

    public void StartLearning()
    {
        SceneManager.LoadScene(firstLearningScene);
    }

    public void ExitApp()
    {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    //  OPEN PRIVACY CANVAS
    public void OpenPrivacyCanvas()
    {
        privacyCanvas.SetActive(true);
    }

    // CLOSE PRIVACY CANVAS
    public void ClosePrivacyCanvas()
    {
        privacyCanvas.SetActive(false);
    }
}