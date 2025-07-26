using UnityEngine;

public enum IngredientType
{
    None,
    Tortilla,
    Filling,
    Topping,
    FinishedTaco,
    Bun,         
    Patty,       
    Cheese,      
    Lettuce
}

public class IngredientItem : MonoBehaviour
{
    public IngredientType ingredientType;
}
