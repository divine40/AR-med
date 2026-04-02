using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePassword : MonoBehaviour
{
    public TMP_InputField passwordField;
    public Toggle showPasswordToggle;

    void Start()
    {
        showPasswordToggle.onValueChanged.AddListener(TogglePasswordVisibility);
    }

    void TogglePasswordVisibility(bool isOn)
    {
        if (isOn)
        {
            passwordField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            passwordField.contentType = TMP_InputField.ContentType.Password;
        }

        passwordField.ForceLabelUpdate();
    }
}