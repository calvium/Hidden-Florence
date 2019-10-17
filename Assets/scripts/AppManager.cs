using UnityEngine;
using System.Collections;

public enum ExperienceType{ NONE, NATIONAL_GALLERY, FLORENCE, ELSEWHERE };

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