using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class SimpleWebServer : MonoBehaviour
{
    // Порт для веб-сервера
    public int port = 9099;

    // Об'єкт HttpListener для прослуховування вхідних підключень
    private HttpListener listener;

    public static bool isActivated = false;
    // Метод для старту веб-сервера
    void Start()
    {
        // Перевірка, чи порт є вільним
        if (!IsPortAvailable(port))
        {
            Debug.LogError("Port " + port + " is not available. Please choose another port.");
            return;
        }

        // Створення адреси для прослуховування
        string serverUrl = "http://localhost:" + port + "/";
        listener = new HttpListener();
        listener.Prefixes.Add(serverUrl);

        try
        {
            // Старт веб-сервера
            listener.Start();
            Debug.Log("Server started at: " + serverUrl);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to start server: " + e.Message);
            return;
        }

        // Початок асинхронного прослуховування вхідних підключень
        listener.BeginGetContext(OnRequestReceived, null);
    }

    // Метод для обробки вхідних запитів
    private void OnRequestReceived(IAsyncResult result)
    {
        // Отримання контексту запиту
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        // Отримання шляху запиту
        string path = request.Url.AbsolutePath;

        // Обробка запиту на "/dangerpath/register"
        if (path == "/dangerpath/register")
        {
            // Відправити подію для включення кнопки реєстрації
            isActivated = true;
            //StopServer();
            Debug.Log(isActivated);
            // Виведення повідомлення в дебаг лог
            Debug.Log("Received request to register at: " + DateTime.Now);
        }

        // Формування відповіді
        string responseString = "<html><body><h1>Verified Successfully!</h1></body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        // Встановлення параметрів відповіді
        response.ContentType = "text/html";
        response.ContentLength64 = buffer.Length;

        // Відправлення відповіді клієнту
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();

        // Початок асинхронного прослуховування наступного запиту
        listener.BeginGetContext(OnRequestReceived, null);
    }

    // Метод для перевірки доступності порту
    private bool IsPortAvailable(int port)
    {
        // Спроба створення TcpListener для заданого порту
        try
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();
            tcpListener.Stop();
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Метод для зупинки веб-сервера
    public void StopServer()
    {
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            Debug.Log("Server stopped.");
        }
    }

    // Викликається при завершенні роботи компонента
    private void OnDestroy()
    {
        StopServer();
    }
}
