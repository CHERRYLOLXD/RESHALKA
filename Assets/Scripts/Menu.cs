using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using 

public class Menu : MonoBehaviour
{
    private const int idScene = 1;
    public void Konec()
    {
        Application.Quit();
    }
    public void Nachalo()
    {
        File.Open(EditorUtility.OpenFilePanel("Select a script", "", "txt"));
        if()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(idScene);
        }
    }
}
