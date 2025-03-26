using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{   
    enum GameSequence
    {
        SUCCESS,
        CRASH
    }
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Update() 
    {
        RespondToDebugKeys();        
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (!isControllable || !isCollidable) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Finish":
                startSequence(GameSequence.SUCCESS);
                break;
            case "Respawn":
                break;
            case "Safe":
                break;
            default:
                startSequence(GameSequence.CRASH);
                break;
        }
    }

    void startSequence(GameSequence s)
    {
        isControllable = false;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        switch(s)
        {
            case GameSequence.SUCCESS:
                audioSource.PlayOneShot(successSFX);
                successParticles.Play();
                Invoke("LoadNextLevel", levelLoadDelay);
                break;
            case GameSequence.CRASH:
                audioSource.PlayOneShot(crashSFX);
                crashParticles.Play();
                Invoke("ReloadLevel", levelLoadDelay);
                break;
        }
        
    }
   

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        
        SceneManager.LoadScene(nextScene);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

}
