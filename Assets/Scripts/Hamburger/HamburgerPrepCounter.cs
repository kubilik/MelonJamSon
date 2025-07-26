using UnityEngine;

public class HamburgerPrepCounter : MonoBehaviour
{
    public IngredientType[] acceptedTypes = {
        IngredientType.Bun,
        IngredientType.Patty,
        IngredientType.Cheese,
        IngredientType.Lettuce
    };

    public HamburgerBuilder builder;
}
