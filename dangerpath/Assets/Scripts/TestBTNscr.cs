using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBTNscr : MonoBehaviour
{
    public void OnTestMSGbtn() 
    {
        LogPlayerNicknamesRequest request = new LogPlayerNicknamesRequest { };
        NetworkClient.Send(request);
    }
}
