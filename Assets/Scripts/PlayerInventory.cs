using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasOrder = false;
    public OrderType currentOrderType;

    public GameObject tacoPrefab;
    public GameObject coconotPrefab; 

    private GameObject orderInstance;
    public Transform handTransform;

    public void PickUpOrder(OrderType type)
    {
        if (hasOrder) return;

        hasOrder = true;
        currentOrderType = type;
        Debug.Log("Picked up order: " + type);

        GameObject prefabToSpawn = null;

        switch (type)
        {
            case OrderType.Taco:
                prefabToSpawn = tacoPrefab;
                break;
            case OrderType.Shake1:
                prefabToSpawn = coconotPrefab;
                break; 
        }

        if (prefabToSpawn != null && handTransform != null)
        {
            orderInstance = Instantiate(prefabToSpawn, handTransform.position, handTransform.rotation, handTransform);
        }
    }

    public void DeliverOrder()
    {
        if (!hasOrder) return;

        hasOrder = false;
        Destroy(orderInstance);
        Debug.Log("Order delivered.");
    }

    public bool IsHoldingOrder()
    {
        return hasOrder;
    }

    public OrderType GetHeldOrderType()
    {
        return currentOrderType;
    }
}
