using TMPro;
using UnityEngine;

public class ScrapUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scrapAmountText;

    private void Start()
    {
        ScrapManager.OnNewScrap += UpdateScrapText;
    }

    private void OnDisable()
    {
        ScrapManager.OnNewScrap -= UpdateScrapText;
    }

    private void UpdateScrapText(object sender, int newScrap)
    {
        scrapAmountText.text = newScrap.ToString();
    }
}
