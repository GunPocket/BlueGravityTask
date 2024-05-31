using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject commentUI;

    [SerializeField] private bool canInteract = false;

    private InteractableObject currentInteractable;

    private PlayerInput playerInput;

    private PlayerState currentState = PlayerState.Walking;

    private enum PlayerState {
        Walking,
        Interacting
    }

    private void Awake() {
        playerInput = new PlayerInput();
        commentUI.SetActive(false);
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
        playerInput.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Player.Move.canceled += ctx => Move(Vector2.zero);
        playerInput.Player.Interact.performed += ctx => OnInteract();
        playerInput.Player.Exit.performed += ctx => Exit();
        playerInput.Player.Inventory.performed += ctx => ShowInventory();
    }

    private void OnDisable() {
        playerInput.Player.Disable();
        playerInput.Player.Move.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Player.Move.canceled -= ctx => Move(Vector2.zero);
        playerInput.Player.Interact.performed -= ctx => OnInteract();
        playerInput.Player.Exit.performed -= ctx => Exit();
        playerInput.Player.Inventory.performed -= ctx => ShowInventory();
    }

    private void Move(Vector2 direction) {
        if (currentState == PlayerState.Walking) {
            Vector2 moveVelocity = direction * moveSpeed;
            rb.velocity = moveVelocity;
        }
    }

    private void OnInteract() {
        print(currentState);
        print(canInteract);
        if (currentState == PlayerState.Interacting) {
            commentUI.GetComponent<CommentUI>().HideUi();
            currentState = PlayerState.Walking;
            return;
        }

        if (currentState == PlayerState.Walking && canInteract) {
            rb.velocity = Vector2.zero;
            ClothingItem item = currentInteractable.ClothingItem;
            commentUI.GetComponent<CommentUI>().ShowUi(currentInteractable.Message, item);
            currentState = PlayerState.Interacting;
            return;
        }
    }

    private void Exit() {
        if (currentState == PlayerState.Interacting) {
            currentState = PlayerState.Walking;
            commentUI.SetActive(false);
        }
    }

    private void ShowInventory() {
        if (currentState == PlayerState.Walking) {
            inventoryManager.ShowInventory();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            canInteract = true;

            currentInteractable = other.GetComponent<InteractableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            canInteract = false;

            currentInteractable = null;
        }
    }
}
