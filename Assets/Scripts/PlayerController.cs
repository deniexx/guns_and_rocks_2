using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*****************************************/
    /**************** WEAPON *****************/
    /***************************************/
    public Weapon currentWeapon;
    public GameObject bullet;

    /*****************************************/
    /**************** INPUT *****************/
    /***************************************/

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _fireAction;

    /*****************************************/
    /*************** MOVEMENT ***************/
    /***************************************/
    
    public float accelerationForce = 5;
    public float maxSpeed = 10f;
    
    [Range(0f, 1f)]
    public float friction = 0.15f;
    
    private Vector2 _velocity = Vector2.zero;
    private Rigidbody2D _rigidbody2D;

    private bool _impulsing = false;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _fireAction = _playerInput.actions["Fire"];
        _fireAction.started += FireStarted;
        _fireAction.canceled += FireStopped;
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_moveAction.IsPressed())
        {
            Vector2 forceToAdd = _moveAction.ReadValue<Vector2>() * accelerationForce;
            _rigidbody2D.velocity += forceToAdd;
        }
        
        // Apply friction
        Vector2 newVelocity = _rigidbody2D.velocity;
        float frictionCoefficient = 1 - friction;
        newVelocity *= frictionCoefficient;
        
        // Clamp Speed, only if we are not being impulsed by something
        if (!_impulsing)
        {
            float clampedX = Mathf.Clamp(newVelocity.x, -maxSpeed, maxSpeed);
            float clampedY = Mathf.Clamp(newVelocity.y, -maxSpeed, maxSpeed);
            newVelocity = new Vector2(clampedX, clampedY);
            _rigidbody2D.velocity = newVelocity;
        }
    }

    private void FireStarted(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Fire Start");
        currentWeapon.Shoot(GameObject.Find("FirePoint").transform.position);

    }

    private void FireStopped(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Fire Stop");
    }

    /// <summary>
    /// Applies an impulse away from the mouse position
    /// </summary>
    /// <param name="power">The strength/power of the impulse applied</param>
    /// <param name="duration">The duration for which the impulse should be applied for</param>
    public void ApplyImpulseAwayFromMousePos(float power, float duration = 0.1f)
    {
        Vector3 direction = GameplayStatics.GetDirectionFromMouseToLocation(transform.position);
        ApplyImpulse(new Vector2(direction.x, direction.y) * power, duration);
    }

    /// <summary>
    /// Applies an impulse in a given direction
    /// </summary>
    /// <param name="impulseVelocity">The velocity to be applied, not normalized, should have a high magnitude</param>
    /// <param name="duration">The duration for which the impulse should be applied for</param>
    public void ApplyImpulse(Vector2 impulseVelocity, float duration = 0.1f)
    {
        _impulsing = true;
        _rigidbody2D.velocity = impulseVelocity;
        StartCoroutine(ResetImpulse(duration));
    }

    private IEnumerator ResetImpulse(float duration)
    {
        yield return new WaitForSeconds(duration);
        _rigidbody2D.velocity = Vector2.zero;
        _impulsing = false;
    }
}
