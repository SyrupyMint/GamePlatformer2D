using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        // Populate the dropdown with available resolutions
        PopulateResolutionDropdown();

        // Set the OnValueChanged listener for the dropdown
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

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
            // Check if the selected object is a TMP_Dropdown
            if (selectedObject.GetComponent<TMP_Dropdown>() != null)
            {
                TMP_Dropdown dropdown = selectedObject.GetComponent<TMP_Dropdown>();

                if (!dropdown.IsExpanded)
                {
                    // Open the dropdown if it's not already expanded
                    dropdown.Show();
                }
                else
                {
                    // Confirm the current selection and close the dropdown
                    dropdown.Hide();
                    ChangeResolution(dropdown.value);
                }
                return; // Exit since we handled the dropdown
            }

            // Check if the selected object is a Toggle
            if (selectedObject.GetComponent<Toggle>() != null)
            {
                // Toggle the current state
                Toggle toggle = selectedObject.GetComponent<Toggle>();
                toggle.isOn = !toggle.isOn;
                return;
            }

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

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions; // Get all available resolutions
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            // Check if this is the current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ChangeResolution(int index)
    {
        if (resolutions == null || resolutions.Length <= index)
        {
            return;
        }

        // Change the resolution based on the dropdown selection
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }
}
