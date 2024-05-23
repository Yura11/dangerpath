using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCinemachine : CinemachineConfiner
{
    public GameObject maps;

    void Awake()
    {
        if (maps != null)
        {
            // Find the PolygonCollider2D in the maps GameObject
            PolygonCollider2D polyCollider = maps.GetComponentInChildren<PolygonCollider2D>();

            if (polyCollider != null)
            {
                // Set the PolygonCollider2D as the Bounding Shape 2D
                m_BoundingShape2D = polyCollider;
            }
            else
            {
                Debug.LogWarning("No PolygonCollider2D found in maps GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Maps GameObject is not assigned.");
        }
    }
}
