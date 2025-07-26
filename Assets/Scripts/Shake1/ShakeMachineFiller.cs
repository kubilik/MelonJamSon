using UnityEngine;

public class ShakeMachineFiller : MonoBehaviour
{
    public ShakeMachineCupPlacementPoint cupPlacementPoint; // Altýndaki bardak yerleþtirme noktasý
    public GameObject filledCupPrefab; // Shake1FilledCup prefabý

    public void TryFillCup()
    {
        // Yer boþsa dolduramayýz
        if (!cupPlacementPoint.HasCup())
        {
            Debug.Log("No cup placed.");
            return;
        }

        GameObject currentCup = cupPlacementPoint.GetPlacedCup();

        // Eðer cup objesi null ise hata ver
        if (currentCup == null)
        {
            Debug.LogWarning("Cup object reference lost.");
            return;
        }

        // Eðer bardak zaten doluysa tekrar doldurma (opsiyonel güvenlik)
        IngredientItem id = currentCup.GetComponent<IngredientItem>();
        if (id != null && id.ingredientType != IngredientType.ShakeEmptyCup)
        {
            Debug.Log("Cup is not empty.");
            return;
        } 

        // Mevcut boþ bardaðý sahneden kaldýr
        Destroy(currentCup);

        // Yerine dolu bardak yerleþtir
        GameObject filled = Instantiate(filledCupPrefab, cupPlacementPoint.transform.position, cupPlacementPoint.transform.rotation);
        cupPlacementPoint.PlaceCup(filled);

        Debug.Log("Cup filled with shake.");
    }
}
