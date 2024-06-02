using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarListUI : MonoBehaviour
{
    public GameObject[] carArray;

    private static int carId = 0;

    void Start()
    {
        carArray[carId].gameObject.SetActive(true);
    }

    public static int GetIdOfChosenCar()
    {
        return carId;
    }

    public void OnClickNextCarBTN()
    {
        if (carId < carArray.Length - 1)
        {
            carId++;
        }
        else
        {
            carId = 0;
        }

        if (carId == 0)
        {
            carArray[carArray.Length - 1].gameObject.SetActive(false);
        }
        else
        {
            carArray[carId - 1].gameObject.SetActive(false);
        }
        carArray[carId].gameObject.SetActive(true);
    }

    public void OnClickPreviosCarBTN()
    {
        if (carId != 0)
        {
            carId--;
        }
        else
        {
            carId = carArray.Length - 1;
        }

        if (carId == carArray.Length - 1)
        {
            carArray[0].gameObject.SetActive(false);
        }
        else
        {
            carArray[carId + 1].gameObject.SetActive(false);
        }
        carArray[carId].gameObject.SetActive(true);
    }
}
