using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class CustomerAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject targetSunbed;

    public GameObject orderUIPrefab;
    private GameObject orderUIInstance;

    public float stopDistance = 2f;

    private bool hasArrived = false;
    private bool orderGiven = false;
    private bool isUnhappy = false;
    private bool isLeaving = false;

    private OrderType currentOrder;

    public float waitBeforeOrderDuration = 5f;
    public float deliveryDuration = 10f;
    private float currentTimer = 0f;

    private enum CustomerState { WaitingForOrder, WaitingForDelivery, Finished }
    private CustomerState state = CustomerState.WaitingForOrder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject[] sunbeds = GameObject.FindGameObjectsWithTag("Sunbed");
        if (sunbeds.Length > 0)
        {
            targetSunbed = FindClosestSunbed(sunbeds);
            agent.SetDestination(targetSunbed.transform.position);
        }

        currentOrder = (OrderType)Random.Range(0, System.Enum.GetValues(typeof(OrderType)).Length);
    }

    void Update()
    {
        if (!hasArrived && targetSunbed != null)
        {
            float dist = Vector3.Distance(transform.position, targetSunbed.transform.position);
            if (dist <= stopDistance)
            {
                agent.isStopped = true;
                hasArrived = true;

                ShowOrderUI();
                StartWaitingForOrder();
            }
        }

        if (state != CustomerState.Finished && currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;

            if (orderUIInstance != null)
            {
                orderUIInstance.transform.position = transform.position + Vector3.up * 2f;

                Slider slider = orderUIInstance.GetComponentInChildren<Slider>();
                if (slider != null)
                {
                    float duration = (state == CustomerState.WaitingForOrder) ? waitBeforeOrderDuration : deliveryDuration;
                    slider.value = currentTimer / duration;
                }
            }

            if (currentTimer <= 0f)
            {
                HandleTimeOut();
            }
        }

        if (isLeaving)
        {
            agent.SetDestination(transform.position + transform.forward * 5f);
        }
    }

    GameObject FindClosestSunbed(GameObject[] sunbeds)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject sunbed in sunbeds)
        {
            float dist = Vector3.Distance(sunbed.transform.position, currentPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = sunbed;
            }
        }

        return closest;
    }
    public bool IsWaitingForDelivery()
    {
        return state == CustomerState.WaitingForDelivery && !isUnhappy && !isLeaving && !orderGiven;
    }

    public void ReceiveDeliveredTaco()
    {
        state = CustomerState.Finished;
        isLeaving = true;

        if (orderUIInstance != null)
            Destroy(orderUIInstance);

        Debug.Log("Customer received the taco and is leaving.");
    }

    void ShowOrderUI()
    {
        if (orderUIPrefab != null)
        {
            orderUIInstance = Instantiate(orderUIPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

            TextMeshProUGUI txt = orderUIInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                txt.text = "Waiting...";
            }

            Slider slider = orderUIInstance.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                slider.maxValue = 1f;
                slider.value = 1f;
            }
        }
    }

    void StartWaitingForOrder()
    {
        state = CustomerState.WaitingForOrder;
        currentTimer = waitBeforeOrderDuration + 0.01f;
    }

    void StartWaitingForDelivery()
    {
        state = CustomerState.WaitingForDelivery;
        currentTimer = deliveryDuration + 0.01f;

        TextMeshProUGUI txt = orderUIInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.text = "Order: " + currentOrder.ToString();
        }
    }

    void HandleTimeOut()
    {
        if (state == CustomerState.WaitingForOrder)
        {
            isUnhappy = true;
            Debug.Log("Order was not taken in time.");
        }
        else if (state == CustomerState.WaitingForDelivery)
        {
            isUnhappy = true;
            Debug.Log("Order was not delivered in time.");
        }

        state = CustomerState.Finished;
        Destroy(orderUIInstance);
        Destroy(gameObject);
    }

    public bool CanReceiveOrder()
    {
        return hasArrived && !orderGiven && !isUnhappy && state == CustomerState.WaitingForOrder;
    }

    public void ReceiveOrder()
    {
        orderGiven = true;
        StartWaitingForDelivery();
        Debug.Log("Order given: " + currentOrder.ToString());
    }

    public bool CanReceiveDelivery()
    {
        return hasArrived && orderGiven && !isUnhappy && state == CustomerState.WaitingForDelivery;
    }

    public void DeliverOrderToCustomer(PlayerInventory inventory)
    {
        if (CanReceiveDelivery() && inventory.IsHoldingOrder())
        {
            if (inventory.GetHeldOrderType() == currentOrder)
            {
                Debug.Log("Correct order delivered!");
            }
            else
            {
                Debug.Log("Wrong order delivered!");
                isUnhappy = true;
            }

            inventory.DeliverOrder();
            state = CustomerState.Finished;
            Destroy(orderUIInstance);
            Destroy(gameObject);
        }
    }

}
