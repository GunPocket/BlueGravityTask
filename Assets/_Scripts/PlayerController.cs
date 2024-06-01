using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject commentUI;
    [SerializeField] private GameObject dressingRoomUI;

    private bool canInteract = false;

    public bool inputEnabled = true;

    private InteractableObject currentInteractable;

    private PlayerInput playerInput;

    private PlayerState currentState = PlayerState.Walking;

    private enum PlayerState {
        Walking,
        Interacting,
        Dressing,
        LookingAtInventory
    }

    private void Awake() {
        playerInput = new PlayerInput();
        commentUI.SetActive(false);
        dressingRoomUI.SetActive(false);

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
        currentState = PlayerState.Walking;

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
        if (currentState == PlayerState.Interacting) {
            commentUI.GetComponent<CommentUI>().HideUi();
            currentState = PlayerState.Walking;
            currentInteractable.ShowSprite();
            return;
        }

        if (currentState == PlayerState.Walking && canInteract) {
            currentInteractable.HideSprite();
            if (currentInteractable.CompareTag("DressingRoom")) {
                inputEnabled = false;
                EnterDressingRoom();
            } else {
                rb.velocity = Vector2.zero;
                ClothingItem item = currentInteractable.ClothingItem;
                commentUI.GetComponent<CommentUI>().ShowUi(currentInteractable.Message, item);
                currentState = PlayerState.Interacting;
            }
        }
    }

    private void EnterDressingRoom() {
        dressingRoomUI.SetActive(true);
        currentState = PlayerState.Dressing;
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void Exit() {
        if (currentState == PlayerState.Interacting) {
            currentState = PlayerState.Walking;
            commentUI.SetActive(false);
        } else if (currentState == PlayerState.Dressing) {
            currentState = PlayerState.Walking;
            dressingRoomUI.SetActive(false);
        } else if (currentState == PlayerState.LookingAtInventory) {
            currentState = PlayerState.Walking;
            inventoryManager.HideInventoryUI();
        }
    }

    private void ShowInventory() {
        if (currentState == PlayerState.Walking) {
            rb.velocity = Vector2.zero;
            currentState = PlayerState.LookingAtInventory;
            inventoryManager.ShowInventoryUI();
        } else if (currentState == PlayerState.LookingAtInventory) {
            inventoryManager.HideInventoryUI();
            currentState = PlayerState.Walking;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Interactable") || other.CompareTag("DressingRoom")) {
            canInteract = true;
            currentInteractable = other.GetComponent<InteractableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Interactable") || other.CompareTag("DressingRoom")) {
            canInteract = false;
            currentInteractable = null;
        }
    }
}
