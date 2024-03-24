using Newtonsoft.Json;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//SystemInfo.deviceUniqueIdentifier
public class LogInManager : MonoBehaviour
{
    public InputField logInInput;
    public InputField logInPasswordInput;
    public GameObject Username;
    public Text logInText;
    public Text Usernametext;

    private string logInUrl; // URL ������ ��������� ��������� �� ������

    void Start()
    {
        // ������� ���� JSON-�����, �� ������ URL-������ ���������
        string json = Resources.Load<TextAsset>("privatedata").text;
        RegisterConfig config = JsonConvert.DeserializeObject<RegisterConfig>(json);
        logInUrl = config.LogInUrl;
    }

    public void LogInUser()
    {
        logInText.text = " ";
        string logIn = logInInput.text;
        string password = logInPasswordInput.text;
        string pcIdent = SystemInfo.deviceUniqueIdentifier;
        if(IsLogInDataCorrect(logIn, password, pcIdent)==false)
        {
            return;
        }

        var data = new
        {
            logIn = logIn,
            password = password,
            pcIdent = pcIdent
        };

        string jsonData = JsonConvert.SerializeObject(data);

        StartCoroutine(SendLogInRequest(jsonData));

    }

    IEnumerator SendLogInRequest(string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(logInUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("�������: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                // ��������� ������� ���������
                int colonIndex = responseText.IndexOf(":");

                // ����������, �� �������� ���������
                if (colonIndex != -1)
                {
                    // �������� ������� ����� �� ���������
                    string beforeColon = responseText.Substring(0, colonIndex);
                    if (beforeColon == "User authenticated successfully! PlayerUsename")
                    {
                        PlayerProfileManager.gamerNickName = responseText.Substring(colonIndex + 2);
                        logInText.text = "LoggedIn!";
                        Debug.Log("��������� ������!");
                        gameObject.SetActive(false);
                        Username.SetActive(true);
                        Usernametext.text = PlayerProfileManager.gamerNickName;
                    }
                }
                else if (responseText == "Invalid password.")
                {
                    logInText.text = "Invalid password.";
                    Debug.Log("������������ ������.");
                }
                else if (responseText == "Invalid login credentials or account not verified.")
                {
                    logInText.text = "Invalid login credentials or account not verified.";
                    Debug.Log("������ ������ ��� ��� �������� ����� �� �����������.");
                }
                else if (responseText == "Error decoding JSON data.")
                {
                    logInText.text = "Error decoding JSON data.";
                    Debug.Log("������� ����������� ����� JSON.");
                }
                else if (responseText == "No POST data received.")
                {
                    logInText.text = "No POST data received.";
                    Debug.Log("���� ����� POST.");
                }
            }
        }
    }

    private bool IsLogInDataCorrect(string logIn, string password, string pcIdent)
    {
        if (string.IsNullOrEmpty(logIn) || string.IsNullOrEmpty(password))
        {
            Debug.Log("�������: �� ���� ������ ���� ��������!");
            logInText.text = " ";
            logInText.text = "Error: All fields must be filled!";
            return false;
        }

        if (!RegistrationManager.IsValidPassword(password))
        {
            Debug.Log("�������: ������������ ������ ������!");
            logInPasswordInput.text = " ";
            logInPasswordInput.text = "Error: Invalid password format!";
            return false;
        }
        if (pcIdent == null) 
        {
            Debug.Log("�������: ��������� �������� ������������� ������ ��������!");
            logInPasswordInput.text = " ";
            logInPasswordInput.text = "Error: Unable to get your device ID!";
            return false;
        }
        return true;
    }

    private class RegisterConfig
    {
        public string LogInUrl { get; set; }
    }
}
