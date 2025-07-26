using UnityEngine;

public enum IngredientType
{
    None,
    Tortilla,
    Filling,
    Topping,
    FinishedTaco,
    EmptyCup,
    FilledCup,
    IcedCup,
    FinalShake,
    Ice,
    Straw
}

public class IngredientItem : MonoBehaviour
{
    public IngredientType ingredientType;
}
