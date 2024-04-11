using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    private int _cost = 0;
    
    [SerializeField]
    private Weapon[] weapons;

    [SerializeField] 
    private Image image;
    
    [SerializeField] 
    private TMP_Text name;
    
    [SerializeField]
    private TMP_Text price;

    private Weapon _chosenWeapon;

    private bool _firstPurchase = true;

    private int _lastIndex = -1;

    private void Start()
    {
        _chosenWeapon = PickNextWeapon();
        DisplayNextWeapon();
    }

    private Weapon PickNextWeapon()
    {
        if (_firstPurchase)
        {
            _firstPurchase = false;
        }
        else
        {
            _cost = _cost * 2 + 1763 - Random.Range(0, 100);
        }

        int randomRoll = Random.Range(0, weapons.Length);
        if (randomRoll == _lastIndex)
        {
            if (++randomRoll > weapons.Length)
            {
                randomRoll = 0;
            }
        }
        
        return weapons[randomRoll];
    }

    private void DisplayNextWeapon()
    {
        name.SetText(_chosenWeapon.GetComponent<Weapon>().name);
        price.SetText(_cost.ToString());
        image.sprite = _chosenWeapon.GetComponent<SpriteRenderer>().sprite;
    }

    public void BuyWeapon()
    {
        if (GameManager.Instance.TakeAwayFromCurrency(_cost))
        {
            Instantiate(_chosenWeapon.gameObject, GameManager.Instance.Player.transform.position, Quaternion.identity);
            _chosenWeapon = PickNextWeapon();
            DisplayNextWeapon();
        }
    }
}
