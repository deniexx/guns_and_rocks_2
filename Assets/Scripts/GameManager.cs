using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    private static GameManager _manager;

    public static GameManager Instance => _manager;

    private GameObject _player;

    private Shop _shop;

    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }

            return _player;
        }
    }

    public Shop Shop
    {
        get
        {
            if (_shop == null)
            {
                _shop = GameObject.FindObjectOfType<Shop>();
            }

            return _shop;
        }
    }

    private int _experience;

    private int _experienceForNextLevel = 1327;

    private int _currency;

    public UnityEvent<int> onCurrencyChanged;

    public UnityEvent onLevelUp;
    
    public int Currency => _currency;

    [HideInInspector]
    public Camera mainCam;

    public AudioSource gemstoneCollectSound;

    private void Awake()
    {
        if (!_manager)
        {
            _manager = this;
            InitManager();
            return;
        }
        
        Destroy(gameObject);
    }

    private void InitManager()
    {
        mainCam = Camera.main;
    }
    
    /// <summary>
    /// Checks if the player has enough currency
    /// </summary>
    /// <param name="amountToCheck">The amount of currency to check if the player has enough</param>
    /// <returns>TRUE if player has enough currency, FALSE if not</returns>
    public bool HasEnoughCurrency(int amountToCheck)
    {
        return _currency >= amountToCheck;
    }
    
    /// <summary>
    /// Adds to the currency of the player
    /// </summary>
    /// <param name="amountToAdd">How much currency to add</param>
    public void AddToCurrency(int amountToAdd)
    {
        AddToXP(amountToAdd);

        gemstoneCollectSound.Play();
        _currency += amountToAdd * UpgradesStatic.gemstoneIncrease;
        onCurrencyChanged?.Invoke(_currency);
    }

    public void AddToXP(int amountToAdd)
    {
        _experience += amountToAdd;

        if (_experience > _experienceForNextLevel)
        {
            _experience = 0;
            _experienceForNextLevel *= 3;
            UpgradesStatic.gemstoneIncrease++;
            UpgradesStatic.moveSpeedMult += 0.15f;
            UpgradesStatic.damageIncrease += 20;
            UpgradesStatic.monsterHealthMult += 0.15f;
            onLevelUp?.Invoke();
        }
    }

    /// <summary>
    /// Attempts to take away currency from the player
    /// </summary>
    /// <param name="amount">Amount to reduce the currency by</param>
    /// <returns>TRUE if transaction was successful, FALSE if transaction has failed (has not enough currency)</returns>
    public bool TakeAwayFromCurrency(int amount)
    {
        if (!HasEnoughCurrency(amount))
        {
            return false;
        }
        
        // Clamp currency to 0, although it should never go below 0, with the check above
        _currency = Mathf.Max(_currency - amount, 0);
        onCurrencyChanged?.Invoke(_currency);
        return true;
    }
}
