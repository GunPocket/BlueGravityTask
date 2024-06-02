using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [Header("Player References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerAppearance playerAppearance;
    [SerializeField] private ExitGameInteractable exitGameInteractable;

    [Header("UI References")]
    [SerializeField] private GameObject commentUI;
    [SerializeField] private GameObject dressingRoomUI;
    [SerializeField] private GameObject checkoutUI;
    [SerializeField] private GameObject exitGameUI;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private ClothingSelectionManager clothingSelectionManager;

    [Header("Variables about player")]
    [SerializeField] private float playerMoney = 200.00f;
    [SerializeField] private float moveSpeed = 5f;

    public List<ClothingItem> CurrentlyWearing { get; private set; } = new List<ClothingItem>();

    private bool canInteract = false;
    private InteractableObject currentInteractable;
    private PlayerInput playerInput;
    private PlayerState currentState = PlayerState.Walking;

    public InventoryManager InventoryManager { get { return inventoryManager; } }

    public float PlayerMoney {
        get { return playerMoney; }
        set { playerMoney = value; }
    }
    public TMP_Text MoneyText {
        get { return moneyText; }
    }

    private enum PlayerState {
        Walking,
        Interacting,
        Dressing,
        LookingAtInventory,
        OnCheckOut
    }

    private void Start() {
        UpdateMoney($"Money: ${playerMoney:F2}");

        commentUI.SetActive(false);
        dressingRoomUI.SetActive(false);
        checkoutUI.SetActive(false);

        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }

        if (playerCollider == null) {
            playerCollider = GetComponent<Collider2D>();
        }

        if (inventoryManager == null) {
            inventoryManager = GetComponent<InventoryManager>();
        }

        CurrentlyWearing = inventoryManager.Inventory.ToList();
        playerAppearance.UpdateAppearance(CurrentlyWearing);
    }

    private void OnEnable() {
        currentState = PlayerState.Walking;

        playerInput = new PlayerInput();

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
                EnterDressingRoom();
            } else if (currentInteractable.CompareTag("Checkout")) {
                EnterCheckout();
            } else if (currentInteractable.CompareTag("ExitGame")) {
                ExitGameQuestion();
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

        clothingSelectionManager.SendCurrentAppearance(CurrentlyWearing);
    }

    private void EnterCheckout() {
        checkoutUI.SetActive(true);
        rb.velocity = Vector2.zero;
        currentState = PlayerState.OnCheckOut;
        gameObject.SetActive(false);
    }

    private void ExitGameQuestion() {
        exitGameUI.SetActive(true);
        rb.velocity = Vector2.zero;
        currentState = PlayerState.Interacting;
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
        } else if (currentState == PlayerState.OnCheckOut) {
            currentState = PlayerState.Walking;
            checkoutUI.SetActive(false);
        }
        gameObject.SetActive(true);
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
        if (other.CompareTag("Interactable") || other.CompareTag("DressingRoom") || other.CompareTag("Checkout") || other.CompareTag("ExitGame")) {
            canInteract = true;
            currentInteractable = other.GetComponent<InteractableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Interactable") || other.CompareTag("DressingRoom") || other.CompareTag("Checkout") || other.CompareTag("ExitGame")) {
            canInteract = false;
            currentInteractable = null;
        }
    }

    public void UpdateMoney(string text) {
        moneyText.text = text;
    }

    public void EquipItem(ClothingItem item) {
        UnequipItemsOfType(item.Type);
        CurrentlyWearing.Add(item);
        playerAppearance.UpdateAppearance(CurrentlyWearing);
    }

    public void UnequipItemsOfType(ClothingItem.ItemType itemType) {
        for (int i = CurrentlyWearing.Count - 1; i >= 0; i--) {
            if (CurrentlyWearing[i].Type == itemType) {
                CurrentlyWearing.RemoveAt(i);
            }
        }
        playerAppearance.UpdateAppearance(CurrentlyWearing);
    }

    public void SetWalkingState() {
        currentState = PlayerState.Walking;
        gameObject.SetActive(true);
    }

    public void ExitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
