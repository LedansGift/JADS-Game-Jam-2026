using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
