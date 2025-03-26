using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] ParticleSystem thrustParticle;
    [SerializeField] ParticleSystem leftParticle;
    [SerializeField] ParticleSystem rightParticle;
    [SerializeField] AudioClip mainEngineSFX;

    Rigidbody rb;
    AudioSource audioSource;
    void Awake()
    {
       
    }
    void Start() 
    {
        rb = GetComponent<Rigidbody>();    
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() 
    {
        thrust.Enable(); 
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSFX);
            }
            if(!thrustParticle.isPlaying)
            {
                thrustParticle.Play();
            }
        }else{
            StopThrusting();
        }
    }
    void StopThrusting()
    {
        thrustParticle.Stop();
        audioSource.Stop();
    }

    void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if(rotationInput < 0 )
            {
                ApplyRotation(rotationStrength,Direction.LEFT);
            }
            else if(rotationInput > 0)
            {
                ApplyRotation(-rotationStrength,Direction.RIGHT);
            }else{
                StopRotating();
        }
    }

    void ApplyRotation(float rotationThisFrame,Direction dir)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
        switch(dir)
        {
            case Direction.LEFT:
                if (!leftParticle.isPlaying)
                {
                    rightParticle.Stop();
                    leftParticle.Play();
                }
                break;
            case Direction.RIGHT:
                if (!rightParticle.isPlaying)
                {
                    leftParticle.Stop();
                    rightParticle.Play();
                }
                break;
        }
    }
    void StopRotating()
    {
        leftParticle.Stop();
        rightParticle.Stop();
    }
}
