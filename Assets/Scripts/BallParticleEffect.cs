using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticleEffect : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private Rigidbody rb;

    public float maxSpeed = 10.0f; // Vitesse maximale pour l'effet maximal

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (particleSystem != null && rb != null)
        {
            var emissionModule = particleSystem.emission;
            float speed = rb.velocity.magnitude;

            float rate = Mathf.Lerp(0.0f, 50.0f, speed / maxSpeed); // Taux d'émission

            emissionModule.rateOverTime = rate;
        }
    }
}
