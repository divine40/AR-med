using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // TEMP: force logout so it won't auto-skip login while testing
        // If you later want auto-login, remove the SignOut line
    }

    public async void RegisterUser()
    {
        errorText.text = "";

        try
        {
            await auth.CreateUserWithEmailAndPasswordAsync(
                emailInput.text,
                passwordInput.text
            );

            errorText.text = "Registration Successful!";
        }
        catch (System.Exception )
        {
            errorText.text = "Registration Failed";
        }
    }

    public async void LoginUser()
    {
        errorText.text = "";

        try
        {
            await auth.SignInWithEmailAndPasswordAsync(
                emailInput.text,
                passwordInput.text
            );

            SceneManager.LoadScene("MenuScene");
        }
        catch (System.Exception e)
        {
            errorText.text = "Login Failed";
        }
    }

    public void LogoutUser()
    {
        auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }
}