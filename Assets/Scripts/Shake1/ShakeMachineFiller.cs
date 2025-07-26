using UnityEngine;

public class ShakeMachineFiller : MonoBehaviour
{
    public ShakeMachineCupPlacementPoint cupPlacementPoint; // Altýndaki bardak noktasý referansý
    public GameObject filledCupPrefab; // Shake1FilledCup prefabý

    public void TryFillCup()
    {
        if (!cupPlacementPoint.HasCup())
        {
            Debug.Log("No cup to fill.");
            return;
        }

        GameObject currentCup = cupPlacementPoint.GetPlacedCup();
        if (currentCup == null)
        {
            Debug.Log("Cup object missing.");
            return;
        }

        // Cup'ý sahneden sil
        Destroy(currentCup);

        // Yerine dolu bardak spawnla
        GameObject filled = Instantiate(filledCupPrefab, cupPlacementPoint.transform.position, cupPlacementPoint.transform.rotation);
        cupPlacementPoint.PlaceCup(filled); // Ayný noktaya yerleþtir
        Debug.Log("Cup filled with shake.");
    }
}
