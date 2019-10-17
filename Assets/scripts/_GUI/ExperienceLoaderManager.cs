﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExperienceLoaderManager : MonoBehaviour
{
    public Image loadingBar;
    public Text warningText;
    [TextArea]
    public string NationalGallery_Warning;
    [TextArea]
    public string Florence_Warning;
    [TextArea]
    public string Elsewhere_Warning;

    private ExperienceType selectedExperience;
    private string experienceScene;
    private AsyncOperation asyncOp;
    private bool buttonPressed;


    private void Awake()
    {
        selectedExperience = AppManager.Instance.SelectedExperience;
        buttonPressed = false;

        Initialize();

        StartCoroutine(Loading());
    }

    private void Initialize()
    {
        switch (selectedExperience)
        {
            case ExperienceType.NATIONAL_GALLERY:
                warningText.text = NationalGallery_Warning;
                experienceScene = "imageAnchor";
                break;
            case ExperienceType.FLORENCE:
                warningText.text = Florence_Warning;
                experienceScene = "FlorenceAR";
                break;
            case ExperienceType.ELSEWHERE:
                warningText.text = Elsewhere_Warning;
                experienceScene = "tapToPlace";
                break;
            default:
                warningText.text = "";
                break;
        }
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(1f);
        yield return null;

        asyncOp = SceneManager.LoadSceneAsync(experienceScene);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            loadingBar.fillAmount = asyncOp.progress + (0.1f * asyncOp.progress);

            if (asyncOp.progress >= 0.9f && buttonPressed && asyncOp.allowSceneActivation == false)
            {

                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void HandleContinuePress()
    {
        Debug.Log("DEBUG: Button pressed, " + asyncOp.progress + "  " + asyncOp.isDone);
        buttonPressed = true;
    }
}

