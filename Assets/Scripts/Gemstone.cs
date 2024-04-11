using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gemstone : MonoBehaviour
{
    [SerializeField]
    private int value = 10;

    [SerializeField] 
    private float moveSpeed = 15;
    
    private Transform _targetTransform;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GemstoneCollector"))
        {
            _targetTransform = other.transform;
            StartCoroutine(GoToCollector());
        }
    }

    public void SetValue(int newValue)
    {
        value = Mathf.Max(newValue, 0);
        float scale = (float)value / 10;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private IEnumerator GoToCollector()
    {
        Vector3 direction = _targetTransform.position - transform.position;
        float distance = (direction).magnitude;
        while (Mathf.Abs(distance) > 0.3f)
        {
            direction = _targetTransform.position - transform.position;
            distance = (direction).magnitude;
            transform.position += direction.normalized * (moveSpeed * Time.deltaTime);
            transform.Rotate(0f, 0f, 10f);
            yield return null;
        }
     
        GameManager.Instance.AddToCurrency(value);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
