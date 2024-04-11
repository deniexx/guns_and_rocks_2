using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 8f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _fireAction;
    private Transform _transform;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _fireAction = _playerInput.actions["Fire"];
        _fireAction.started += Fire;
        _transform = transform;
    }

    private void Update()
    {
        if (_moveAction.IsPressed())
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>() * Time.deltaTime * moveSpeed;
            _transform.position += new Vector3(moveInput.x, moveInput.y, 0);
        }
    }

    private void Fire(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Firing!");
    }
}
