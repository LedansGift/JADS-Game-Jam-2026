using UnityEngine;

public class StructureButton : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedGraphic;

    [SerializeField]
    private GameObject availableGraphic;

    [SerializeField]
    private GameObject unavailableGraphic;

    public void SetStructureAvailability(bool available)
    {
        availableGraphic.SetActive(false);
        unavailableGraphic.SetActive(false);

        if (available)
        {
            availableGraphic.SetActive(true);
        }
        else
        {
            unavailableGraphic.SetActive(true);
        }
    }

    public void ToggleSelectStructured(bool toggle)
    {
        selectedGraphic.SetActive(toggle);
    }
}
