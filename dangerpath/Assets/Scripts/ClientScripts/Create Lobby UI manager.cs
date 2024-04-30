using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUImanager : MonoBehaviour
{
    public GameObject[] mapArray;

    public Text numberOfPlayersText;
    public Text numberOfLapsText;

    public GameObject lobbuListCanvas;

    private static int numberOfPlayers = 3;
    private static int numberOfLaps = 1;
    private static int mapNumber = 0;

    void Start()
    {
        mapArray[mapNumber].gameObject.SetActive(true);
    }

    public static int GetNumberOfPlayersInLobby()
    {
        return numberOfPlayers;
    }

    public static int GetNumberOfLapsInLobby()
    {
        return numberOfLaps;
    }

    public static int GetMapNumberInLobby()
    {
        return mapNumber;
    }

    public void OnClickNextMapBTN()
    {
        if (mapNumber < mapArray.Length - 1)
        {
            mapNumber++;
        }
        else
        {
            mapNumber = 0;
        }

        if (mapNumber == 0)
        {
            mapArray[mapArray.Length - 1].gameObject.SetActive(false);
        }
        else
        {
            mapArray[mapNumber - 1].gameObject.SetActive(false);
        }
        mapArray[mapNumber].gameObject.SetActive(true);
    }

    public void OnClickPreviosMapBTN()
    {
        if (mapNumber != 0)
        {
            mapNumber--;
        }
        else
        {
            mapNumber = mapArray.Length - 1;
        }

        if( mapNumber == mapArray.Length - 1)
        {
            mapArray[0].gameObject.SetActive(false);
        }
        else
        {
            mapArray[mapNumber + 1].gameObject.SetActive(false);
        }
        mapArray[mapNumber].gameObject.SetActive(true);
    }

    public void OnClickGoBackBTN()
    {
        gameObject.SetActive(false);
        lobbuListCanvas.SetActive(true);
    }

    public void OnClickIncreaseNumberOfPlayersBTN()
    {
        if (numberOfPlayers < 10)
        {
            numberOfPlayers++;
            numberOfPlayersText.text = numberOfPlayers.ToString();
        }
    }

    public void OnClickDecreaseNumberOfPlayersBTN()
    {
        if (numberOfPlayers > 1)
        {
            numberOfPlayers--;
            numberOfPlayersText.text = numberOfPlayers.ToString();
        }
    }

    public void OnClickIncreaseNumberOfLapsBTN()
    {
        numberOfLaps++;
        numberOfLapsText.text = numberOfLaps.ToString();
    }

    public void OnClickDecreaseNumberOfLapsBTN()
    {
        if (numberOfLaps > 1)
        {
            numberOfLaps--;
            numberOfLapsText.text = numberOfLaps.ToString();
        }
    }
}
