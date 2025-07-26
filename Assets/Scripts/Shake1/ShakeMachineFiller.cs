using UnityEngine;

public class ShakeMachineFiller : MonoBehaviour
{
    public ShakeMachineCupPlacementPoint cupPlacementPoint; // Alt�ndaki bardak noktas� referans�
    public GameObject filledCupPrefab; // Shake1FilledCup prefab�

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

        // Cup'� sahneden sil
        Destroy(currentCup);

        // Yerine dolu bardak spawnla
        GameObject filled = Instantiate(filledCupPrefab, cupPlacementPoint.transform.position, cupPlacementPoint.transform.rotation);
        cupPlacementPoint.PlaceCup(filled); // Ayn� noktaya yerle�tir
        Debug.Log("Cup filled with shake.");
    }
}
