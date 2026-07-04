using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour{
    public Rigidbody rb;

    public InputActionReference move;
    public InputActionReference jump;

    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    public float rotationSpeed = 0.5f;

    private Vector2 moveInput;

    private void OnEnable(){
        jump.action.performed += Jump;
    }

    private void OnDisable(){
        jump.action.performed -= Jump;
    }

    void Update(){
        moveInput = move.action.ReadValue<Vector2>();
    }

    void FixedUpdate(){
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;
        rb.linearVelocity = velocity;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    void Jump(InputAction.CallbackContext context){
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}