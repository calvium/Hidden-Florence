using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceInstructionsManager : MonoBehaviour
{
    public Text Title;
    public TextMeshProUGUI Body;
    public Image Image;
    public GameObject Extra_Button;
    
    public string NationalGallery_Title;
    [TextArea]
    public string NationalGallery_Instructions;
    public Sprite NationalGallery_Image;
    public string NationalGallery_Extra_Button_Title;
    public string NationalGallery_Extra_Button_URL;

    public string Florence_Title;
    [TextArea]
    public string Florence_Instructions;
    public Sprite Florence_Image;
    public string Florence_Extra_Button_Title;
    public string Florence_Extra_Button_URL;

    public string Elsewhere_Title;
    [TextArea]
    public string Elsewhere_Instructions;
    public Sprite Elsewhere_Image;

    private ExperienceType selectedExperience;


    void Awake()
    {
        selectedExperience = AppManager.Instance.SelectedExperience;

        Initialize();
    }

    private void Initialize()
    {
        switch (selectedExperience)
        {
            case ExperienceType.NATIONAL_GALLERY:
                Title.text = NationalGallery_Title;
                Body.SetText(NationalGallery_Instructions);
                Image.sprite = NationalGallery_Image;
                Extra_Button.GetComponentInChildren<Text>().text = NationalGallery_Extra_Button_Title;
                Extra_Button.GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL(NationalGallery_Extra_Button_URL); });
                Extra_Button.SetActive(true);
                break;
            case ExperienceType.FLORENCE:
                Title.text = Florence_Title;
                Body.SetText(Florence_Instructions);
                Image.sprite = Florence_Image;
                Extra_Button.GetComponentInChildren<Text>().text = Florence_Extra_Button_Title;
                Extra_Button.GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL(Florence_Extra_Button_URL); });
                Extra_Button.SetActive(true);
                break;
            case ExperienceType.ELSEWHERE:
                Title.text = Elsewhere_Title;
                Body.SetText(Elsewhere_Instructions);
                Image.sprite = Elsewhere_Image;
                Extra_Button.SetActive(false);
                break;
            default:
                Title.text = "";
                Body.text = "";
                break;
        }
    }
}

