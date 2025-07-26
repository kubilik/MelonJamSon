using UnityEngine;
using System.Collections.Generic;

public class HamburgerBuilder : MonoBehaviour
{
    public IngredientType[] requiredIngredients = {
        IngredientType.Bun,
        IngredientType.Patty,
        IngredientType.Cheese,
        IngredientType.Lettuce
    };

    public GameObject finishedHamburgerPrefab;
    public Transform finalSpawnPoint;

    public GameObject bunPrefab;
    public GameObject pattyPrefab;
    public GameObject cheesePrefab;
    public GameObject lettucePrefab;

    public Transform[] spawnPoints; // length = 4

    private List<IngredientType> currentIngredients = new List<IngredientType>();
    private List<GameObject> spawnedVisuals = new List<GameObject>();

    public bool hasFinishedHamburger = false;

    public void AddIngredient(IngredientType type)
    {
        if (hasFinishedHamburger)
        {
            Debug.Log("Hamburger already completed.");
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
                Instantiate(finishedHamburgerPrefab, finalSpawnPoint.position, finalSpawnPoint.rotation);
                hasFinishedHamburger = true;
                Debug.Log("Hamburger completed.");
            }
            else
            {
                Debug.Log("Wrong combination. Resetting.");
            }

            ResetBuilder();
        }
    }

    private GameObject GetPrefabForIngredient(IngredientType type)
    {
        switch (type)
        {
            case IngredientType.Bun: return bunPrefab;
            case IngredientType.Patty: return pattyPrefab;
            case IngredientType.Cheese: return cheesePrefab;
            case IngredientType.Lettuce: return lettucePrefab;
            default: return null;
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
        foreach (GameObject visual in spawnedVisuals)
        {
            if (visual != null)
                Destroy(visual);
        }
        spawnedVisuals.Clear();
    }

    public void ClearFinishedHamburger()
    {
        hasFinishedHamburger = false;
    }

    public bool HasFinishedBurger => hasFinishedHamburger;

}
