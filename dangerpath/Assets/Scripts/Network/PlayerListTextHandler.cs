using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListTextHandler : MonoBehaviour
{
    public Text playerNameText; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayerNameText(string playername)
    {
        playerNameText.text = playername;
    }
}
