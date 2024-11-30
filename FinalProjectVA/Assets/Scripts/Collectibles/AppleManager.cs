using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AppleManager : MonoBehaviour
{
    public static AppleManager instance;
    private int apples;
    [SerializeField] private TMP_Text applesDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Register callback for scene load
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the ApplesDisplay TMP_Text object in the new scene
        if (scene.name != "MainMenu") // Replace "MainMenu" with your actual main menu scene name
        {
            GameObject applesDisplayObject = GameObject.Find("ApplesDisplay");
            if (applesDisplayObject != null)
            {
                applesDisplay = applesDisplayObject.GetComponent<TMP_Text>();
                ResetApples();
            }

        }
    }

    private void ResetApples()
    {
        apples = 0; 
    }

    private void OnGUI()
    {
        if (applesDisplay != null)
        {
            applesDisplay.text = apples.ToString();
        }
    }

    public void ChangeApples(int amount)
    {
        apples += amount;
    }
}