using UnityEngine;

public class TextBarkManager : MonoBehaviour
{
    private static int barkPoolIndex = 0;

    [SerializeField]
    private TextBarkUI[] localBarkPool;
    private static TextBarkUI[] barkPool;

    private void Start()
    {
        barkPool = localBarkPool;
        barkPoolIndex = 0;
    }

    public static void SpawnBark(Vector3 position, string barkText, Color barkColour)
    {
        TextBarkUI bark = barkPool[barkPoolIndex];
        bark.SpawnBark(barkText, barkColour);
        bark.transform.position = position;

        barkPoolIndex++;

        if (barkPoolIndex >= barkPool.Length)
        {
            barkPoolIndex = 0;
        }
    }
}
