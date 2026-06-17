using TMPro;
using UnityEngine;

public class TextBarkUI : MonoBehaviour
{
    private bool barkActive = false;
    private float barkMovementSpeed = 0.5f;
    private float barkTimer;
    private float barkDisappearTime = 2.5f;

    [SerializeField]
    private TextMeshProUGUI barkTextMesh;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        fader.SetCanvasGroupAlpha(0f);
    }

    private void Update()
    {
        if (!barkActive)
        {
            return;
        }

        MoveBark();
    }

    private void MoveBark()
    {
        transform.position += new Vector3(0f, barkMovementSpeed * Time.deltaTime, 0f);

        barkTimer += Time.deltaTime;

        if (barkTimer >= 0.5f)
        {
            float faderValue = Mathf.Lerp(1.2f, 0f, barkTimer / barkDisappearTime);

            fader.SetCanvasGroupAlpha(faderValue);
        }

        if (barkTimer > barkDisappearTime)
        {
            barkActive = false;
            fader.SetCanvasGroupAlpha(0f);
        }
    }

    public void SpawnBark(string barkText, Color barkColour)
    {
        barkTextMesh.text = barkText;
        barkTextMesh.color = barkColour;

        fader.ToggleFade(true);

        barkActive = true;
        barkTimer = 0f;
    }
}
