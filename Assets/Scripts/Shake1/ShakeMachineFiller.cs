using UnityEngine;

public class ShakeMachineFiller : MonoBehaviour
{
    public ShakeMachineCupPlacementPoint cupPlacementPoint; // Alt�ndaki bardak yerle�tirme noktas�
    public GameObject filledCupPrefab; // Shake1FilledCup prefab�

    public void TryFillCup()
    {
        // Yer bo�sa dolduramay�z
        if (!cupPlacementPoint.HasCup())
        {
            Debug.Log("No cup placed.");
            return;
        }

        GameObject currentCup = cupPlacementPoint.GetPlacedCup();

        // E�er cup objesi null ise hata ver
        if (currentCup == null)
        {
            Debug.LogWarning("Cup object reference lost.");
            return;
        }

        // E�er bardak zaten doluysa tekrar doldurma (opsiyonel g�venlik)
        IngredientItem id = currentCup.GetComponent<IngredientItem>();
        if (id != null && id.ingredientType != IngredientType.ShakeEmptyCup)
        {
            Debug.Log("Cup is not empty.");
            return;
        } 

        // Mevcut bo� barda�� sahneden kald�r
        Destroy(currentCup);

        // Yerine dolu bardak yerle�tir
        GameObject filled = Instantiate(filledCupPrefab, cupPlacementPoint.transform.position, cupPlacementPoint.transform.rotation);
        cupPlacementPoint.PlaceCup(filled);

        Debug.Log("Cup filled with shake.");
    }
}
