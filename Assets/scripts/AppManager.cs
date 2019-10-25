using UnityEngine;
using System.Collections;

public enum ExperienceType{ FLORENCE, ELSEWHERE, NATIONAL_GALLERY, NONE };

public class AppManager : MonoBehaviour {

    public static AppManager Instance { get; private set; }

    public ExperienceType SelectedExperience;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}