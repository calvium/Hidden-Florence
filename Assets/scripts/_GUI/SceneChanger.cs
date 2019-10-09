using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChanger : MonoBehaviour
{

    public GameObject background;
	public Animator animator;

    private string previousScene;

    private void Awake()
    {
        background.SetActive(true);

    }

    public void FadeToPreviousScene()
	{
		string sceneName = SceneManager.GetActiveScene().name;

		switch(sceneName) {
			case "ExperienceLoader":
            case "imageAnchor":
            case "tapToPlace":
				previousScene = "ExperienceSelector";
				break;
            default:
				previousScene = "MainMenu";
				break;
		}

		animator.SetTrigger("FadeOut");
	}

    public void FadeToScene(string sceneName)
	{
		previousScene = sceneName;
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(previousScene);
		previousScene = null;
	}

    public void SelectExperience(string experienceName)
	{
        Debug.Log(experienceName);
        switch(experienceName)
        {
            case "NATIONAL_GALLERY":
                AppManager.Instance.SelectedExperience = ExperienceType.NATIONAL_GALLERY;
                break;
            case "FLORENCE":
                AppManager.Instance.SelectedExperience = ExperienceType.FLORENCE;
                break;
            case "ELSEWHERE":
                AppManager.Instance.SelectedExperience = ExperienceType.ELSEWHERE;
                break;
            default:
                AppManager.Instance.SelectedExperience = ExperienceType.NONE;
                break;
        }
	}

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
