using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class w : MonoBehaviour
{
    float particleEmmisionRate=0;

    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
        particleSystemSmoke = GetComponent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission ;

        particleSystemEmissionModule.rateOverTime = 0;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particleEmmisionRate=Mathf.Lerp(particleEmmisionRate, 0, Time.deltaTime*5);
        particleSystemEmissionModule.rateOverTime = particleEmmisionRate;

        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                particleEmmisionRate = 3;
            else particleEmmisionRate = Mathf.Abs(lateralVelocity) * 20;
        }


    }
}
