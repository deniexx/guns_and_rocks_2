using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*****************************************/
    /**************** WEAPON *****************/
    /***************************************/
    private Weapon _currentWeapon;

    [SerializeField]
    private float pickupRadius = 4f;
    
    [SerializeField]
    private Transform weaponAttachPoint;

    /*****************************************/
    /**************** INPUT *****************/
    /***************************************/

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _fireAction;
    private InputAction _equipWeapon;

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

        _equipWeapon = _playerInput.actions["EquipWeapon"];
        _equipWeapon.started += AttemptToEquipWeapon;
        
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
            newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
            _rigidbody2D.velocity = newVelocity;
        }
        
        // Rotate player towards mouse
        Vector3 playerToMouse = -GameplayStatics.GetDirectionFromMouseToLocation(transform.position);
        float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x);
        transform.rotation = Quaternion.AngleAxis((angle * Mathf.Rad2Deg) + 90, Vector3.forward);
    }

    private void FireStarted(InputAction.CallbackContext callbackContext)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.StartFiring();
        }
    }

    private void FireStopped(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Fire Stop");
        if (_currentWeapon != null)
        {
            _currentWeapon.EndFiring();
        }
    }

    private void AttemptToEquipWeapon(InputAction.CallbackContext callbackContext)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
        foreach (var collider in colliders)
        {
            if (collider != null && collider.CompareTag("Weapon"))
            {
                EquipWeapon(collider.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Equips a new weapon
    /// </summary>
    /// <param name="weapon">The weapon to equip</param>
    void EquipWeapon(GameObject weapon)
    {
        Weapon weaponComp = weapon.GetComponent<Weapon>();
        if (weaponComp == null)
        {
            return;
        }
        
        if (_currentWeapon)
        {
            _currentWeapon.EndFiring();
            Destroy(_currentWeapon.gameObject);
        }
        
        BoxCollider2D boxCollider2D = weapon.GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        weapon.transform.parent = weaponAttachPoint;
        weapon.transform.localPosition = new Vector3(0f, 0f);
        _currentWeapon = weaponComp;
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
