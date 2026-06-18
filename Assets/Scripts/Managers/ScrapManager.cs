using System;
using System.Collections;
using UnityEngine;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager Instance { get; private set; }

    private int startingScrap = 400;

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
        StartCoroutine(InitialScrapAdd());
    }

    private IEnumerator InitialScrapAdd()
    {
        yield return new WaitForSeconds(1f);
        scrap = startingScrap;
        UpdateScrapAmount();
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
        if (newScrap <= 0)
        {
            return;
        }

        scrap += newScrap;
        AudioManager.PlaySFX(scrapGetSFX, transform.position);

        TextBarkManager.SpawnBark(transform.position, "+" + newScrap.ToString(), Color.green);

        UpdateScrapAmount();
    }

    public void SpendScrap(int spentScrap)
    {
        scrap -= spentScrap;
        AudioManager.PlaySFX(scrapSpendSFX, transform.position);

        TextBarkManager.SpawnBark(transform.position, "-" + spentScrap.ToString(), Color.red);

        UpdateScrapAmount();
    }
}
