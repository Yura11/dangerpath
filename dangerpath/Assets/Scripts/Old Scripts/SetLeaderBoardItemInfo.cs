using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SetLeaderBoardItemInfo : MonoBehaviour
{
    public string playerName;
    public Text positionText;
    public Text driverNameText;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }

    public void SetDriverName(string newDriverName)
    {
        driverNameText.text = newDriverName;
    }
}
