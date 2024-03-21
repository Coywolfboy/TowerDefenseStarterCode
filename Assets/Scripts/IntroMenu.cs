using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
    public InputField nameInputField;
    public Button startButton;
    public Button quitButton;

    private void Start()
    {
        // Disable start button at the beginning
        startButton.interactable = false;

        // Register a callback for the name input field value changed event
        if (nameInputField != null)
        {
            nameInputField.onValueChanged.AddListener(OnNameValueChanged);
        }

        // Register callbacks for the buttons
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        // Unregister callbacks to prevent memory leaks
        if (nameInputField != null)
        {
            nameInputField.onValueChanged.RemoveListener(OnNameValueChanged);
        }
        startButton.onClick.RemoveListener(StartGame);
        quitButton.onClick.RemoveListener(QuitGame);
    }

    private void OnNameValueChanged(string newName)
    {
        // Check if the name is at least 3 characters long
        if (newName.Length >= 3)
        {
            // Enable the start button if the name is valid
            startButton.interactable = true;
        }
        else
        {
            // Disable the start button if the name is not valid
            startButton.interactable = false;
        }
    }

    private void StartGame()
    {
        // Load the GameScene
        Debug.Log("GG");
        SceneManager.LoadScene("GameScene");
    }

    private void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

}
