using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogOut : MonoBehaviour
{
    private string logOutUrl;
    public GameObject signUp;
    public GameObject username;
    public Text usernametext;

    public void OnClickLogOut()
    {
        string json = Resources.Load<TextAsset>("privatedata").text;
        JsonConfig config = JsonConvert.DeserializeObject<JsonConfig>(json);
        logOutUrl = config.LogOutUrl;
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
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(logOutUrl, "POST"))
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
                if (responseText == "logged out!")
                {
                    gameObject.SetActive(false);
                    PlayerProfileManager.gamerNickName = null;
                    usernametext.text = PlayerProfileManager.gamerNickName;
                    username.SetActive(false);
                    signUp.SetActive(true);
                    Debug.Log("Розлогінився");
                }
                else if (responseText == "PCIdentifier is missing in the request.")
                {
                    Debug.Log("Нема ідентифікатора пк");
                }
                else
                {
                        Debug.Log("Неправильна відповідь сервера");
                }
            }
        }
    }

    private class JsonConfig
    {
        public string LogOutUrl { get; set; }
    }
}
