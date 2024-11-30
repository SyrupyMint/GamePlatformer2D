using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    public KeyCode confirmKey = KeyCode.C;  //Btn to confirm any btns
    public KeyCode backKey = KeyCode.X; //Btn to quit/back

    public GameObject mainMenu;           
    public GameObject settingsMenu;
    public GameObject creditsMenu;

    private void Update()
    {
        // Check for Confirm key press
        if (Input.GetKeyDown(confirmKey))
        {
            HandleConfirm();
        }

        // Check for Quit/Back key press
        if (Input.GetKeyDown(backKey))
        {
            HandleBack();
        }
    }

    private void HandleBack()
    {
        // If in the settings menu, go back to the main menu
        if (settingsMenu.activeSelf)
        {
            CloseSettings();
        }
        // If in the credits menu, go back to the main menu
        else if (creditsMenu.activeSelf)
        {
            CloseCredits();
        }
        else
        {
            // Quit the game if on the main menu
            QuitGame();
        }
    }

    private void HandleConfirm()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null)
        {
            // Call the appropriate method based on the button name
            switch (selectedObject.name)
            {
                case "PlayButton":
                    PlayGame();
                    break;
                case "SettingsButton":
                    OpenSettings();
                    break;
                case "CreditsButton":
                    OpenCredits();
                    break;
                case "QuitButton":
                    QuitGame();
                    break;
                case "ReturnButton":
                    if (settingsMenu.activeSelf)
                        CloseSettings();
                    else if (creditsMenu.activeSelf)
                        CloseCredits();
                    break;
                default:
                    break;
            }
        }
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(creditsMenu.transform.Find("ReturnButton").gameObject);
    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);

        // Set the PlayButton as the first selected button
        EventSystem.current.SetSelectedGameObject(mainMenu.transform.Find("PlayButton").gameObject);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);

        // Set the ReturnButton as the first selected button
        EventSystem.current.SetSelectedGameObject(settingsMenu.transform.Find("ReturnButton").gameObject);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);

        // Set the PlayButton as the first selected button
        EventSystem.current.SetSelectedGameObject(mainMenu.transform.Find("PlayButton").gameObject);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            EditorApplication.isPlaying = false;
        #else
            // Quit the application in the build
            Application.Quit();
        #endif
    }
}
