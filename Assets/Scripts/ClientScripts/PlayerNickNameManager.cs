using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerProfileManager : MonoBehaviour 
{
    private string checkLogInUrl;
    public static string gamerNickName;
    public GameObject SignUp;
    public GameObject Username;
    public Text Usernametext;

    // Start is called before the first frame update
    void Start()
    {
        string json = Resources.Load<TextAsset>("privatedata").text;
        RegisterConfig config = JsonConvert.DeserializeObject<RegisterConfig>(json);
        checkLogInUrl = config.CheckLogInUrl;
        string pcIdent = SystemInfo.deviceUniqueIdentifier;
        var data = new
        {
            pcIdent = pcIdent
        };
        string jsonData = JsonConvert.SerializeObject(data);

        StartCoroutine(SendLogInRequest(jsonData));
    }

    IEnumerator SendLogInRequest(string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(checkLogInUrl, "POST"))
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
                string responseText = www.downloadHandler.text;
                if (responseText == "Not logged in")
                {
                    SignUp.SetActive(true);
                    Debug.Log("Not logged in.");
                }
                else if (responseText == "No POST data received.")
                {
                    Debug.Log("No POST data received.");
                }
                else
                {
                    // Знаходимо позицію двокрапки
                    int colonIndex = responseText.IndexOf(":");

                    // Перевіряємо, чи знайдена двокрапка
                    if (colonIndex != -1)
                    {
                        string beforeColon = responseText.Substring(0, colonIndex);
                        if (beforeColon == "Username")
                        {
                            gamerNickName = responseText.Substring(colonIndex + 1);
                            Username.SetActive(true);
                            Usernametext.text = gamerNickName;
                        }
                    } else
                    {
                        Debug.Log("Неправильна відповідь сервера");
                    }
                }
            }
        }
    }

    private class RegisterConfig
    {
        public string CheckLogInUrl { get; set; }
    }
}
