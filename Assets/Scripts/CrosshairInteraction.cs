using UnityEngine;
using TMPro;

public class CrosshairInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI interactionText;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green);

            CustomerAI customer = hit.collider.GetComponent<CustomerAI>();
            if (customer != null)
            {
                // Order taking
                if (customer.CanReceiveOrder())
                {
                    interactionText.text = "[E] Take Order";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        customer.ReceiveOrder();
                        interactionText.gameObject.SetActive(false);
                    }
                    return;
                }

                // Order delivery
                if (customer.CanReceiveDelivery())
                {
                    interactionText.text = "[E] Deliver Order";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PlayerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
                        customer.DeliverOrderToCustomer(inventory);
                        interactionText.gameObject.SetActive(false);
                    }
                    return;
                }
            }
        }

        // If not interacting
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
}
