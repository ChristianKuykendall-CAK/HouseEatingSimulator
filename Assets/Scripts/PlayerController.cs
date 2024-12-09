using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public Transform Head;
    public Collider Sucker;
    public Camera Camera;
    public AudioSource audioSource;
    public CraftingManager craftingManager;
    public InventoryItemData itemToCraft;

    [Header("Configurations")]
    public static PlayerController instance;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float itemPickupDistance;

    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = false;
    bool isJumping = false;
    Transform attachedObject = null;
    float attachedDistance = 1.5f;
    float rotationSpeed = 15f;
    bool isEating = false;
    int eatCounter = 600;

    private void Awake()
    {
        instance = this;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

        Cursor.visible = true;

        // Horizontal Rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);

        newVelocity = Vector3.up * rb.velocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        // Jump control
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                newVelocity.y = jumpSpeed;
                isJumping = true;
            }
        }
        rb.velocity = transform.TransformDirection(newVelocity);

        // Picking objects
        RaycastHit hit;
        bool cast = Physics.Raycast(Head.position, Head.forward, out hit, itemPickupDistance);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (attachedObject != null)
            {
                attachedObject.SetParent(null);

                if (attachedObject.GetComponent<Rigidbody>() != null)
                    attachedObject.GetComponent<Rigidbody>().isKinematic = false;

                if (attachedObject.GetComponent<Collider>() != null)
                    attachedObject.GetComponent<Collider>().enabled = true;

                attachedObject = null;
            }
            else
            {
                craftingManager.DropAllItems();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Sucker.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            craftingManager.Craft(itemToCraft);
        }

        if (isEating == true)
        {
            eatCounter--;
            attachedDistance -= .0025f;
            if (eatCounter < 0)
            {
                EatItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (attachedObject != null && isEating == false) {

                attachedObject.TryGetComponent(out ItemObject item);
                audioSource.PlayOneShot(item.referenceItem.eatingSound, 1);

                int totalItems = 0;

                foreach (var heldItem in InventorySystem.current.Inventory)
                {
                    totalItems += heldItem.StackSize;
                }

                if (totalItems > 4)
                {
                    craftingManager.DropAllItems();
                    return;
                }

                isEating = true;
            }
            if (attachedObject == null) {
                if (cast) {
                    if (hit.transform.CompareTag("Pickable")) {
                        attachedObject = hit.transform;
                        attachedObject.SetParent(transform);

                        if (attachedObject.GetComponent<Rigidbody>() != null)
                            attachedObject.GetComponent<Rigidbody>().isKinematic = true;

                        if (attachedObject.GetComponent<Collider>() != null)
                            attachedObject.GetComponent<Collider>().enabled = false;
                        
                    }
                }
            }
        }
    }

    void LateUpdate() {

        // Vertical Rotation
        Vector3 e = Head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x, -85, 85f);
        Head.eulerAngles = e;

        // Pick up object
        if (attachedObject != null) {

            Vector3 newPosition = Head.position + Head.forward * attachedDistance;

            if (isEating)
            {
            Quaternion lookRotation = Quaternion.LookRotation(Head.position - attachedObject.position);
            lookRotation *= Quaternion.Euler(100f, 0f, 0f);
            lookRotation *= Quaternion.Euler(0f, 90f, 0f);
            attachedObject.rotation = Quaternion.Slerp(attachedObject.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            newPosition.y -= 0.4f;
            }
            else
            {
            Quaternion lookRotation = Quaternion.LookRotation(Head.position - attachedObject.position);
            lookRotation *= Quaternion.Euler(0f, 90f, 0f);
            attachedObject.rotation = Quaternion.Slerp(attachedObject.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            attachedObject.position = newPosition;
        }
    }

    public void EatItem()
    {
        attachedObject.TryGetComponent(out ItemObject item);
        item.OnHandlePickupItem();
        isEating = false;
        attachedObject = null;
        eatCounter = 600;
        attachedDistance = 1.5f;
    }
    

    public void SetItemToCraft(InventoryItemData item)
    {
        itemToCraft = item;
    }

    // Restrict the vertical head rotation (prevent from bending backwards)
    public static float RestrictAngle(float angle, float angleMin, float angleMax) {
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        if (angle > angleMax)
            angle = angleMax;
        if (angle < angleMin)
            angle = angleMin;

        return angle;
    }

    void OnCollisionStay(Collision col) {
        isGrounded = true;
        isJumping = false;
    }

    void OnCollisionExit(Collision col) {
        isGrounded = false;
    }
}
