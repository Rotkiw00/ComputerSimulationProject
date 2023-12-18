using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWindowMenu : MonoBehaviour
{
    public void OpenAuthorsScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenInstructionsScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void OpenSimulationCreatorScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public const int MainWindowIndex = 0;
    public void NavigateBack()
    {
        SceneManager.LoadScene(MainWindowIndex);
    }

    public string SimulationFilePath;
    public void OpenFileBrowser()
    {
        SimulationFilePath = EditorUtility.OpenFilePanel("Wybierz plik symulacji (.json)", "", "json");

        if (!string.IsNullOrEmpty(SimulationFilePath))
        {
            if (Path.GetExtension(SimulationFilePath) == ".json")
            {
                Debug.Log("Successfully .json !");
            }
            else
            {
                Debug.LogWarning("File chosen but not .json");
            }
        }
        else
        {
            Debug.LogError("Nothing has been chosen");
        }
    }
}
