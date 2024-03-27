using JetBrains.Annotations;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject _logIn;
    public GameObject _sign_In;
    public GameObject _profile;
    public GameObject _settings;

    public void LogIn_button()
    {
        if (_sign_In.activeSelf)
        {
            _sign_In.SetActive(false);
        }
        _logIn.SetActive(true);
    }

    public void Sing_In_button()
    {
        if (_logIn.activeSelf)
        {
            _logIn.SetActive(false);
        }
        _sign_In.SetActive(true);
    }

    public void Profile_button()
    {
        if (_sign_In.activeSelf)
        {
            _sign_In.SetActive(false);
        }

        if (_logIn.activeSelf)
        {
            _logIn.SetActive(false);
        }

        _profile.SetActive(true);
    }

    public void Settings_button()
    {
        _settings.SetActive(true);
    }
}
