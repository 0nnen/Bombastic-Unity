using UnityEngine;

public class BallParticleEffect : MonoBehaviour
{
    private ParticleSystem particlesSystem;
    private Rigidbody rb;

    public float maxSpeed = 50.0f; // Vitesse maximale pour l'effet maximal

    void Start()
    {
        particlesSystem = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (particlesSystem != null && rb != null)
        {
            var emissionModule = particlesSystem.emission;
            float speed = rb.velocity.magnitude;

            float rate = Mathf.Lerp(0.0f, 50.0f, speed / maxSpeed); // Taux d'émission

            emissionModule.rateOverTime = rate;
        }
    }
}
