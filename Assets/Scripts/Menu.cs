using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public struct Button
{
    public int NextSceneId;
    public string Label;
}

public struct Scene
{
    public string DescriptionText;
    public Button LeftButton;
    public Button RightButton;
}

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI leftButtonText;
    [SerializeField] private TextMeshProUGUI rightButtonText;

    private const string START_LABEL = "Начало";
    private const string END_LABEL = "Конец";

    private int currentSceneId = 0;
    private readonly Dictionary<int, Scene> scenes = new()
    {
        {
            0,
            new Scene
            {
                DescriptionText = "Сделайте выбор",
                LeftButton = new Button
                {
                    NextSceneId = 0,
                    Label = START_LABEL
                },
                RightButton = new Button
                {
                    NextSceneId = 0,
                    Label = END_LABEL
                }
            }
        }
    };

    private void Start()
    {
        currentSceneId = 0;
        Scene nextScene = scenes[currentSceneId];
        LoadScene(ref nextScene);
    }

    public async void LeftButton()
    {
        if (currentSceneId == 0)
        {
            string scriptFile = EditorUtility.OpenFilePanel("Выберите сценарий", "", "txt");
            if (!File.Exists(scriptFile))
            {
                return;
            }
            using (StreamReader reader = new(scriptFile))
            {
                while (!reader.EndOfStream)
                {
                    string descriptionLine = await reader.ReadLineAsync();
                    string leftButtonLine = await reader.ReadLineAsync();
                    string rightButtonLine = await reader.ReadLineAsync();
                    Debug.Log(descriptionLine + leftButtonLine + rightButtonLine);
                    scenes.Add(
                        Convert.ToInt32(descriptionLine[..descriptionLine.IndexOf(" ")]),
                        new Scene
                        {
                            DescriptionText = descriptionLine[descriptionLine.IndexOf(" ")..],
                            LeftButton = new()
                            {
                                NextSceneId = Convert.ToInt32(leftButtonLine[leftButtonLine.LastIndexOf(" ")..]),
                                Label = leftButtonLine[..leftButtonLine.LastIndexOf(" ")]
                            },
                            RightButton = new()
                            {
                                NextSceneId = Convert.ToInt32(rightButtonLine[rightButtonLine.LastIndexOf(" ")..]),
                                Label = rightButtonLine[..rightButtonLine.LastIndexOf(" ")]
                            }
                        });
                }
            }
            currentSceneId = 1;
            if (scenes.TryGetValue(currentSceneId, out Scene nextScene))
            {
                LoadScene(ref nextScene);
            }
            else
            {
                currentSceneId = 0;
                nextScene = scenes[currentSceneId];
                LoadScene(ref nextScene);
            }
        }
        else
        {
            currentSceneId = scenes[currentSceneId].LeftButton.NextSceneId;
            if (scenes.TryGetValue(currentSceneId, out Scene nextScene))
            {
                LoadScene(ref nextScene);
            }
            else
            {
                currentSceneId = 0;
                nextScene = scenes[currentSceneId];
                LoadScene(ref nextScene);
            }
        }
    }

    public void RightButton()
    {
        if (currentSceneId == 0)
        {
            Application.Quit();
        }
        else
        {
            currentSceneId = scenes[currentSceneId].RightButton.NextSceneId;
            if (scenes.TryGetValue(currentSceneId, out Scene nextScene))
            {
                LoadScene(ref nextScene);
            }
            else
            {
                currentSceneId = 0;
                nextScene = scenes[currentSceneId];
                LoadScene(ref nextScene);
            }
        }
    }

    private void LoadScene(ref Scene scene)
    {
        descriptionText.text = scene.DescriptionText;
        leftButtonText.text = scene.LeftButton.Label;
        rightButtonText.text = scene.RightButton.Label;
    }
}
