using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class SimpleWebServer : MonoBehaviour
{
    // ���� ��� ���-�������
    public int port = 9099;

    // ��'��� HttpListener ��� ��������������� ������� ���������
    private HttpListener listener;

    public static bool isActivated = false;
    // ����� ��� ������ ���-�������
    void Start()
    {
        // ��������, �� ���� � ������
        if (!IsPortAvailable(port))
        {
            Debug.LogError("Port " + port + " is not available. Please choose another port.");
            return;
        }

        // ��������� ������ ��� ���������������
        string serverUrl = "http://localhost:" + port + "/";
        listener = new HttpListener();
        listener.Prefixes.Add(serverUrl);

        try
        {
            // ����� ���-�������
            listener.Start();
            Debug.Log("Server started at: " + serverUrl);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to start server: " + e.Message);
            return;
        }

        // ������� ������������ ��������������� ������� ���������
        listener.BeginGetContext(OnRequestReceived, null);
    }

    // ����� ��� ������� ������� ������
    private void OnRequestReceived(IAsyncResult result)
    {
        // ��������� ��������� ������
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        // ��������� ����� ������
        string path = request.Url.AbsolutePath;

        // ������� ������ �� "/dangerpath/register"
        if (path == "/dangerpath/register")
        {
            // ³�������� ���� ��� ��������� ������ ���������
            isActivated = true;
            //StopServer();
            Debug.Log(isActivated);
            // ��������� ����������� � ����� ���
            Debug.Log("Received request to register at: " + DateTime.Now);
        }

        // ���������� ������
        string responseString = "<html><body><h1>Verified Successfully!</h1></body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        // ������������ ��������� ������
        response.ContentType = "text/html";
        response.ContentLength64 = buffer.Length;

        // ³���������� ������ �볺���
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();

        // ������� ������������ ��������������� ���������� ������
        listener.BeginGetContext(OnRequestReceived, null);
    }

    // ����� ��� �������� ���������� �����
    private bool IsPortAvailable(int port)
    {
        // ������ ��������� TcpListener ��� �������� �����
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

    // ����� ��� ������� ���-�������
    public void StopServer()
    {
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            Debug.Log("Server stopped.");
        }
    }

    // ����������� ��� ��������� ������ ����������
    private void OnDestroy()
    {
        StopServer();
    }
}
