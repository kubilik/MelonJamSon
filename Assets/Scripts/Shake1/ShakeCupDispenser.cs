using UnityEngine;

public class ShakeCupDispenser : MonoBehaviour
{
    public IngredientType cupType = IngredientType.ShakeEmptyCup;
    public GameObject cupVisualPrefab; // eline alýnacak görsel prefab

    public void TryGiveCup(PlayerIngredientInventory inventory)
    {
        if (!inventory.IsCarrying())
        {
            inventory.PickUpIngredient(cupType, cupVisualPrefab);
            Debug.Log("Player took a shake empty cup.");
        }
    }
}
