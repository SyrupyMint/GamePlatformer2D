using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private Animator transAnim;

    [SerializeField] private int mainMenuSceneIndex = 0;
    [SerializeField] private int lastMapSceneIndex = 3;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the Animator only in scenes where it should exist
        if (scene.name != "MainMenu") // Replace "MainMenu" with your actual main menu scene name
        {
            GameObject transitionObject = GameObject.Find("Scene Transition");
            if (transitionObject != null)
            {
                transAnim = transitionObject.GetComponent<Animator>();
            }
        }
        else
        {
            transAnim = null; // Clear the reference in the Main Menu
        }
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        // Check if the current scene is the last map
        if (SceneManager.GetActiveScene().buildIndex == lastMapSceneIndex)
        {
            // Return to the main menu
            SceneManager.LoadSceneAsync(mainMenuSceneIndex);
        }
        else
        {
            // Load the next level
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        transAnim.SetTrigger("start");
    }
}
