using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[] mapArray;
    // Start is called before the first frame update
    void Awake()
    {
        mapArray[CrossScaneInfoHolder.MapNumber].gameObject.SetActive(true);
    }
}
