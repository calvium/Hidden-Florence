using UnityEngine;
using UnityEngine.UI;

public class SceneSetup_TapToPlace : MonoBehaviour
{
    public GameObject church;
    public Text helpTitleText;
    public Text helpStepsText;
    [TextArea] public string helpSteps_Florence;
    [TextArea] public string helpSteps_Elsewhere;

    private ExperienceType selectedExperience;

    private void Awake()
    {
        selectedExperience = AppManager.Instance.SelectedExperience;
    }

    void Start()
    {
        Transform churchTransform = church.GetComponent<Transform>();
        Animator churchAnimator = church.GetComponent<Animator>();
        Vector3 newPosition = churchTransform.position;
        RuntimeAnimatorController newAnimator = null;

        if (selectedExperience == ExperienceType.FLORENCE)
        {
            newPosition = new Vector3(1.79f, 2.2f, 31.3f);
            newAnimator = Resources.Load<RuntimeAnimatorController>("Models/_church0.2/animations/florenceAR/florenceAR (Tap to Place)");

            helpTitleText.text = "How to use the app - Florence";
            helpStepsText.text = helpSteps_Florence;
        }
        else if (selectedExperience == ExperienceType.ELSEWHERE)
        {
            newPosition = new Vector3(1.79f, 0.19f, 18.24f);
            newAnimator = Resources.Load<RuntimeAnimatorController>("Models/_church0.2/animations/elsewhereAR/elsewhereAR (Tap to Place)");

            helpTitleText.text = "How to use the app - Elsewhere";
            helpStepsText.text = helpSteps_Elsewhere;
        }

        churchTransform.position = newPosition;
        churchAnimator.runtimeAnimatorController = newAnimator;
    }
}
