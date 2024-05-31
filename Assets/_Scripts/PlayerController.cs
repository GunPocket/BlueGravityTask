using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InventoryManager inventoryManager;

    private Collider2D interactCollider;

    private Vector2 moveInput;
    private PlayerInput playerInput;
    private bool canInteract = false;
    private bool confirmAction = false;

    private void Awake() {
        playerInput = new PlayerInput();

        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }

        if (playerCollider == null) {
            playerCollider = GetComponent<Collider2D>();
        }

        if (inventoryManager == null) {
            inventoryManager = GetComponent<InventoryManager>();
        }
    }

    private void OnEnable() {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Interact.performed += OnInteract;
        playerInput.Player.Exit.performed += OnExit;
        playerInput.Player.Inventory.performed += OnInventory;
    }

    private void OnDisable() {
        playerInput.Player.Disable();
        playerInput.Player.Move.performed -= OnMove;
        playerInput.Player.Move.canceled -= OnMove;
        playerInput.Player.Interact.performed -= OnInteract;
        playerInput.Player.Exit.performed -= OnExit;
        playerInput.Player.Inventory.performed -= OnInventory;
    }

    private void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context) {
        if (canInteract) {
            InteractableObject interactableObject = interactCollider.GetComponent<InteractableObject>();
            ClothingItem item = interactableObject.ClothingItem;
            if (interactableObject.ItemHere) {
                inventoryManager.AddItem(item);
                interactableObject.CollectItem();
                Debug.Log("Item collected: " + item.name);
            } else {
                inventoryManager.RemoveItem(item);
                interactableObject.ReturnItem();
                Debug.Log("Item removed: " + item.name);
            }

        }

        if (confirmAction) {
            interactCollider?.GetComponent<InteractableObject>().Interact();
        }
    }

    private void OnExit(InputAction.CallbackContext context) {
        if (confirmAction) {
            //close interaction window
        }
    }

    private void OnInventory(InputAction.CallbackContext context) {
        inventoryManager.ShowInventory();
    }

    private void FixedUpdate() {
        Vector2 moveVelocity = moveInput * moveSpeed;
        rb.velocity = moveVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            canInteract = true;
            other.GetComponent<InteractableObject>().ShowInteractSprite();
            interactCollider = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            canInteract = false;
            other.GetComponent<InteractableObject>().HideInteractSprite();
            interactCollider = null;
        }
    }
}
