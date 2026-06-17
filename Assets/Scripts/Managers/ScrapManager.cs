using System;
using UnityEngine;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager Instance { get; private set; }

    private int startingScrap = 750;

    private int scrap = 0;

    [SerializeField]
    private SFXObject scrapGetSFX;

    [SerializeField]
    private SFXObject scrapSpendSFX;

    public static EventHandler<int> OnNewScrap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        AddScrap(startingScrap);
    }

    private void UpdateScrapAmount()
    {
        OnNewScrap?.Invoke(this, scrap);
    }

    public int GetAvailableScrap()
    {
        return scrap;
    }

    public bool IsEnoughAvailableScrap(int scrapNeeded)
    {
        return scrapNeeded <= scrap;
    }

    public void AddScrap(int newScrap)
    {
        scrap += newScrap;
        AudioManager.PlaySFX(scrapGetSFX, transform.position);

        UpdateScrapAmount();
    }

    public void SpendScrap(int spentScrap)
    {
        scrap -= spentScrap;
        AudioManager.PlaySFX(scrapSpendSFX, transform.position);
        UpdateScrapAmount();
    }
}
