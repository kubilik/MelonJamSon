using UnityEngine;
using System.Collections.Generic;

public class TacoBuilder : MonoBehaviour
{
    public IngredientType[] requiredIngredients = {
        IngredientType.Tortilla,
        IngredientType.Filling,
        IngredientType.Topping
    };

    public GameObject finishedTacoPrefab;
    public Transform finalSpawnPoint;

    public GameObject tortillaPrefab;
    public GameObject fillingPrefab;
    public GameObject toppingPrefab;

    public Transform[] spawnPoints;

    private List<IngredientType> currentIngredients = new List<IngredientType>();
    private List<GameObject> spawnedVisuals = new List<GameObject>();

    public bool hasFinishedTaco = false;

    public void AddIngredient(IngredientType type)
    {
        if (hasFinishedTaco)
        {
            Debug.Log("There is already a finished taco on the counter.");
            return;
        }

        if (currentIngredients.Count >= requiredIngredients.Length)
        {
            Debug.Log("Too many ingredients.");
            return;
        }

        if (currentIngredients.Contains(type))
        {
            Debug.Log("Duplicate ingredient. Resetting builder.");
            ResetBuilder();
            return;
        }

        currentIngredients.Add(type);

        GameObject visual = GetPrefabForIngredient(type);
        if (visual != null && currentIngredients.Count - 1 < spawnPoints.Length)
        {
            Transform spawnPoint = spawnPoints[currentIngredients.Count - 1];
            GameObject visualInstance = Instantiate(visual, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            spawnedVisuals.Add(visualInstance);
        }

        if (currentIngredients.Count == requiredIngredients.Length)
        {
            if (IsCorrectCombination())
            {
                Debug.Log("Taco completed.");

                GameObject taco = Instantiate(finishedTacoPrefab, finalSpawnPoint.position, finalSpawnPoint.rotation);
                FinishedTacoInstance marker = taco.AddComponent<FinishedTacoInstance>();
                marker.originatingBuilder = this;

                hasFinishedTaco = true;
            }
            else
            {
                Debug.Log("Wrong combination. Resetting builder.");
            }

            ResetBuilder();
        }
    }

    private GameObject GetPrefabForIngredient(IngredientType type)
    {
        switch (type)
        {
            case IngredientType.Tortilla:
                return tortillaPrefab;
            case IngredientType.Filling:
                return fillingPrefab;
            case IngredientType.Topping:
                return toppingPrefab;
            default:
                return null;
        }
    }

    private bool IsCorrectCombination()
    {
        IngredientType[] sortedRequired = (IngredientType[])requiredIngredients.Clone();
        IngredientType[] sortedCurrent = currentIngredients.ToArray();

        System.Array.Sort(sortedRequired);
        System.Array.Sort(sortedCurrent);

        for (int i = 0; i < sortedRequired.Length; i++)
        {
            if (sortedRequired[i] != sortedCurrent[i])
                return false;
        }

        return true;
    }

    private void ResetBuilder()
    {
        currentIngredients.Clear();

        foreach (GameObject obj in spawnedVisuals)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedVisuals.Clear();
    }

    public void ClearFinishedTaco()
    {
        hasFinishedTaco = false;
    }
}
