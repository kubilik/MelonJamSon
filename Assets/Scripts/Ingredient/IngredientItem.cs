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
    Lettuce, 
    ShakeEmptyCup,
    Shake1FilledCup,
    Shake1WithIce,
    Shake1WithStraw,
    FinalShake1
}

public class IngredientItem : MonoBehaviour
{
    public IngredientType ingredientType;
}
