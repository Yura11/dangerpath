using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public GameObject _logIn;
    public GameObject _sign_In;
    public GameObject _profile;
    public GameObject _settings;

    public void OnClickPlayOnline()
    {
        CrossScaneInfoHolder.GamerNickName = PlayerProfileManager.gamerNickName;
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickPlay()
    {
        CrossScaneInfoHolder.GamerNickName = GenerateRandomName(6);
        SceneManager.LoadScene("Lobby");
    }

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

    public string GenerateRandomName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // All uppercase English letters
        var random = new System.Random();
        var randomName = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomName[i] = chars[random.Next(chars.Length)];
        }
        return new string(randomName);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit");
    }
}
