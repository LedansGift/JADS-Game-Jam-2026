using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildStructureElement : MonoBehaviour
{
    private const float inactiveOpacity = 0.6f;

    [SerializeField]
    private GameObject unavailableImage;

    [SerializeField]
    private Color unavailableColour;

    [SerializeField]
    private Color unaffordableColour;

    [SerializeField]
    private Image structureImage;

    [SerializeField]
    private TextMeshProUGUI structureCost;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        unavailableImage.SetActive(false);
        structureImage.color = Color.white;
    }

    public void ToggleOpacity(bool toggle)
    {
        if (toggle)
        {
            fader.SetCanvasGroupAlpha(1f);
        }
        else
        {
            fader.SetCanvasGroupAlpha(inactiveOpacity);
        }
    }

    public void ToggleUnavailable(bool toggle)
    {
        unavailableImage.SetActive(toggle);

        if (toggle)
        {
            structureImage.color = unavailableColour;
        }
        else
        {
            structureImage.color = Color.white;
        }
    }

    public void TogglePurchaseable(bool toggle)
    {
        if (toggle)
        {
            structureCost.color = Color.white;
        }
        else
        {
            structureCost.color = unaffordableColour;
        }
    }

    public void SetScrapCost(int cost)
    {
        structureCost.text = cost.ToString();
    }
}
