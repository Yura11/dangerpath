using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RegistrationManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField emailInput;
    public Text registerText;

    private string registerUrl; // URL адреса обробника реєстрації на сервері
    private string checkAccountExistURL;

    void Start()
    {
        // Зчитуємо вміст JSON-файлу, що містить URL-адресу реєстрації
        string json = Resources.Load<TextAsset>("privatedata").text;
        RegisterConfig config = JsonConvert.DeserializeObject<RegisterConfig>(json);
        registerUrl = config.RegisterUrl;
        checkAccountExistURL = config.CheckAccountExistURL;
    }

    public void RegisterUser()
    {
        registerText.text = " ";
        string username = usernameInput.text;
        string password = passwordInput.text;
        string email = emailInput.text;

        Debug.Log(email + password + username);

        IsRegisterDataCorrect(username, password, email);

        var data = new
        {
            username = username,
            password = password,
            email = email
        };

        string jsonData = JsonConvert.SerializeObject(data);

        StartCoroutine(CheckAccountExist(jsonData));
        
    }

    IEnumerator CheckAccountExist(string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(checkAccountExistURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Помилка: " + www.error);
                Debug.Log("URL: " + checkAccountExistURL);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                if(responseText == "exists") 
                {
                    registerText.text = "This Email or Username already in use!";
                } 
                else if (responseText == "not_exists")
                {
                    StartCoroutine(SendRegistrationRequest(jsonData));
                }
            }
        }
    }

    IEnumerator SendRegistrationRequest(string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(registerUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Помилка: " + www.error);
            }
            else
            {
                registerText.text = "Verify your email address!";
                Debug.Log("Реєстрація успішна!");
            }
        }
    }

    private void IsRegisterDataCorrect(string username, string password, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
        {
            Debug.Log("Помилка: Всі поля повинні бути заповнені!");
            registerText.text = " ";
            registerText.text = "Error: All fields must be filled!";
            return;
        }

        if (!IsValidEmail(email))
        {
            Debug.Log("Помилка: Неправильний формат електронної адреси!");
            registerText.text = " ";
            registerText.text = "Error: Invalid email format!";
            return;
        }

        if (!IsValidPassword(password))
        {
            Debug.Log("Помилка: Неправильний формат пароля!");
            registerText.text = " ";
            registerText.text = "Error: Invalid password format!";
            return;
        }
    }

    // Метод для перевірки правильності формату електронної адреси
    public static bool IsValidEmail(string email)
    {
        // Використовується регулярний вираз для перевірки формату адреси
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    // Метод для перевірки правильності формату пароля
    public static bool IsValidPassword(string password)
    {
        // Ось приклад простого правила: пароль повинен містити щонайменше 6 символів
        return password.Length >= 6;
    }

    private class RegisterConfig
    {
        public string RegisterUrl { get; set; }
        public string CheckAccountExistURL { get; set; }
    }
}