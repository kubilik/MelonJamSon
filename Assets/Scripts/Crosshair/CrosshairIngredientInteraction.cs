using UnityEngine;
using TMPro;

public class CrosshairIngredientInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public TextMeshProUGUI interactionText;
    public LayerMask interactionLayer;

    public GameObject handHeldTacoPrefab;
    public GameObject handHeldHamburgerPrefab;

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

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            PlayerIngredientInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIngredientInventory>();

            // 1. Taco IngredientPickup
            IngredientPickup pickup = hit.collider.GetComponent<IngredientPickup>();
            if (pickup != null && !inventory.IsCarrying())
            {
                interactionText.text = "[E] Pick up: " + pickup.type.ToString();
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventory.PickUpIngredient(pickup.type, pickup.visualPrefab);
                    Debug.Log("Picked up: " + pickup.type);
                }
                return;
            }

            // 2. Place on Taco Prep Counter
            PrepCounter prep = hit.collider.GetComponent<PrepCounter>();
            if (prep != null && inventory.IsCarrying())
            {
                IngredientType carried = inventory.GetHeldIngredientType();

                if (prep.builder.hasFinishedTaco)
                {
                    interactionText.text = "Taco ready - clear it first";
                    interactionText.gameObject.SetActive(true);
                    return;
                }

                if (System.Array.Exists(prep.acceptedTypes, t => t == carried))
                {
                    interactionText.text = "[E] Place on counter";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        inventory.DropIngredient();
                        prep.builder.AddIngredient(carried);
                        Debug.Log("Placed on prep counter: " + carried);
                    }
                }
                else
                {
                    interactionText.text = "Cannot place this item here";
                    interactionText.gameObject.SetActive(true);
                }

                return;
            }

            // 3. Place on Hamburger Prep Counter
            HamburgerPrepCounter burgerPrep = hit.collider.GetComponent<HamburgerPrepCounter>();
            if (burgerPrep != null && inventory.IsCarrying())
            {
                IngredientType carried = inventory.GetHeldIngredientType();

                if (burgerPrep.builder.HasFinishedBurger)
                {
                    interactionText.text = "Burger ready - clear it first";
                    interactionText.gameObject.SetActive(true);
                    return;
                }

                if (System.Array.Exists(burgerPrep.acceptedTypes, t => t == carried))
                {
                    interactionText.text = "[E] Place on burger counter";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        inventory.DropIngredient();
                        burgerPrep.builder.AddIngredient(carried);
                        Debug.Log("Placed on burger counter: " + carried);
                    }
                }
                else
                {
                    interactionText.text = "Cannot place this item here";
                    interactionText.gameObject.SetActive(true);
                }

                return;
            }

            // 4. Trash Can
            TrashCan trash = hit.collider.GetComponent<TrashCan>();
            if (trash != null && (inventory.IsCarrying() || inventory.IsHoldingCraftedTaco()))
            {
                interactionText.text = "[E] Discard in trash";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventory.DropAndDestroy();
                    Debug.Log("Item discarded in trash");
                }
                return;
            }

            // 5. Finished Taco
            if (hit.collider.CompareTag("FinishedTaco") && !inventory.IsCarrying())
            {
                interactionText.text = "[E] Pick up Taco";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventory.PickUpIngredient(IngredientType.None, handHeldTacoPrefab);

                    FinishedTacoInstance instance = hit.collider.GetComponent<FinishedTacoInstance>();
                    if (instance != null && instance.originatingBuilder != null)
                    {
                        instance.originatingBuilder.ClearFinishedTaco();
                    }

                    Destroy(hit.collider.gameObject);
                    Debug.Log("Picked up finished taco");
                }
                return;
            }

            // 6. Finished Hamburger
            if (hit.collider.CompareTag("FinishedHamburger") && !inventory.IsCarrying())
            {
                interactionText.text = "[E] Pick up Hamburger";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventory.PickUpIngredient(IngredientType.None, handHeldHamburgerPrefab);

                    // HamburgerBuilder'ý bulup temizle
                    HamburgerBuilder[] allBuilders = Object.FindObjectsByType<HamburgerBuilder>(FindObjectsSortMode.None);
                    foreach (var builder in allBuilders)
                    {
                        if (builder.HasFinishedBurger)
                        {
                            builder.ClearFinishedHamburger();
                            break;
                        }
                    }


                    Destroy(hit.collider.gameObject);
                    Debug.Log("Picked up finished hamburger");
                }
                return;
            }

            // ShakeCupDispenser
            ShakeCupDispenser shakeDispenser = hit.collider.GetComponent<ShakeCupDispenser>();
            if (shakeDispenser != null && !inventory.IsCarrying())
            {
                interactionText.text = "[E] Take cup";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    shakeDispenser.TryGiveCup(inventory);
                }

                return;
            }

            // Shake Machine Cup Placement Point
            ShakeMachineCupPlacementPoint shakePoint = hit.collider.GetComponent<ShakeMachineCupPlacementPoint>();
            if (shakePoint != null && inventory.IsCarrying() && inventory.GetHeldIngredientType() == IngredientType.ShakeEmptyCup)
            {
                if (!shakePoint.HasCup())
                {
                    interactionText.text = "[E] Place cup under machine";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameObject cupVisual = inventory.DropAndReturnVisual();
                        if (cupVisual != null)
                        {
                            shakePoint.PlaceCup(cupVisual);
                            Debug.Log("Placed cup under shake machine.");
                        }

                    }
                }
                else
                {
                    interactionText.text = "Cup already placed";
                    interactionText.gameObject.SetActive(true);
                }

                return;
            }

            // 5. Shake Fill Button
            ShakeMachineFiller filler = hit.collider.GetComponent<ShakeMachineFiller>();
            if (filler != null)
            {
                interactionText.text = "[E] Fill cup";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    filler.TryFillCup();
                }
                return;
            }




        }

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
}
