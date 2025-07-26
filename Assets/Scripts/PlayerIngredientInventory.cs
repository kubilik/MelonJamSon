using UnityEngine;

public class PlayerIngredientInventory : MonoBehaviour
{
    public Transform holdPoint;
    private GameObject heldObject;

    private IngredientType currentIngredientType = IngredientType.None;

    public bool IsCarrying()
    {
        return heldObject != null;
    }

    public IngredientType GetHeldIngredientType()
    {
        return currentIngredientType;
    }

    public void PickUpIngredient(IngredientType type, GameObject prefab)
    {
        if (IsCarrying())
            return;

        GameObject obj = Instantiate(prefab, holdPoint.position, holdPoint.rotation, holdPoint);
        heldObject = obj;
        currentIngredientType = type;
    }

    public IngredientType DropIngredient()
    {
        if (!IsCarrying())
            return IngredientType.None;

        Destroy(heldObject);
        heldObject = null;

        IngredientType dropped = currentIngredientType;
        currentIngredientType = IngredientType.None;

        return dropped;
    }

    public void DropAndDestroy()
    {
        if (!IsCarrying())
            return;

        Destroy(heldObject);
        heldObject = null;
        currentIngredientType = IngredientType.None;
    }

    //NEW: Allow dropping crafted taco
    public bool IsHoldingCraftedTaco()
    {
        return heldObject != null && currentIngredientType == IngredientType.None;
    }
}
