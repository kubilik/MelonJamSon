using UnityEngine;

public class ShakeMachineCupPlacementPoint : MonoBehaviour
{
    public GameObject placedCup;

    public bool HasCup()
    {
        return placedCup != null;
    }

    public void PlaceCup(GameObject cup)
    {
        placedCup = cup;
        cup.transform.position = transform.position;
        cup.transform.rotation = transform.rotation;
        cup.transform.SetParent(transform);
    }

    public void RemoveCup()
    {
        if (placedCup != null)
        {
            Destroy(placedCup);
            placedCup = null;
        }
    }
    public GameObject GetPlacedCup()
    {
        return placedCup;
    }

}
