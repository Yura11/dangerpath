using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailerRenderereHandler : MonoBehaviour
{
    TopDownCarController TopDownCarController;
    TrailRenderer trailRenderer;

   

    void Awake()
    {
        TopDownCarController = GetComponentInParent<TopDownCarController>();    
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }
    // Start is called before the first frame update
    
    
    // Update is called once per frame
    void Update()
    {
        if (TopDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        { 
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
