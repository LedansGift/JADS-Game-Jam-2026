using UnityEngine;

public class GridMouseVisual : MonoBehaviour
{
    private bool mouseVisualActive = false;
    private InputManager input;

    [SerializeField]
    private Transform mouseGridVisual;

    //private GridSystemVisualSingle mouseGridVisualScript;


    private void Start()
    {
        //mouseGridVisualScript = mouseGridVisual.GetComponent<GridSystemVisualSingle>();
        //mouseGridVisualScript.ToggleTransparencyOscillation(true);

        input = InputManager.Instance;

        ToggleMouseVisibility(true);
    }

    private void OnDisable() { }

    void Update()
    {
        if (!mouseVisualActive)
        {
            return;
        }

        SetMouseVisualPosition();
    }

    private void SetMouseVisualPosition()
    {
        GridPosition mouseGridPosition;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(input.MousePosition);

        mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);

        Debug.Log("Current Grid Position: " + mouseGridPosition.ToString());

        mouseGridVisual.position = LevelGrid.Instance.GetWorldPosition(mouseGridPosition);
    }

    private void ToggleMouseVisibility(bool toggle)
    {
        mouseVisualActive = toggle;
        mouseGridVisual.gameObject.SetActive(toggle);
        SetMouseVisualPosition();
    }
}
