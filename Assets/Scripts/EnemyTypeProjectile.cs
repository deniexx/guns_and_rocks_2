using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTypeProjectile : MonoBehaviour
{
    //set variables for AI's (player) location & enemy/chaser speed
    private Transform playerTransform;
    private Transform enemyTransform;
    public float speed = 3.5f;
    public float approachThreashold = 10f;

    [SerializeField] GameObject enemyBullet;
    [SerializeField] public float fireRate = 3;

    private HealthComponent _healthComponent;
    private MaterialPropertyBlock _mpb;
    private SpriteRenderer _spriteRenderer;

    private Coroutine _flashCoroutine;

    [SerializeField]
    private GameObject gemstone;

    private static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
    private static readonly int LerpAlpha = Shader.PropertyToID("_LerpAlpha");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void Start()
    {
        //link player transform to player game object   &   link up enemy transform to local transform
        playerTransform = GameManager.Instance.Player.transform;
        enemyTransform = transform;

        _healthComponent.ApplyMaxHealthMult(UpgradesStatic.monsterHealthMult);
        //shoot first bullet(s) 3 seconds after game starts
        StartCoroutine(EnemyShootTimer(3));
    }

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.onHealthChanged.AddListener(OnHealthChanged);

        _mpb = new MaterialPropertyBlock();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //update enemy by moving it towards the AI/client's location
    void Update()
    {
        //if the enemy is far enough away - move towards player  |  when enemy get close enough, it will try to keep its distance as it shoots
        if ((playerTransform.position - enemyTransform.position).magnitude > approachThreashold) enemyTransform.position += getDireciton() * (speed * Time.deltaTime);
        else if ((playerTransform.position - enemyTransform.position).magnitude < approachThreashold - 1) enemyTransform.position += -getDireciton() * (speed * Time.deltaTime); 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //player - enemy collisions
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<HealthComponent>().ApplyHealthDelta(-10f);
    }

    Vector3 getDireciton()
    {
        return (playerTransform.position - transform.position).normalized;
    }

    private IEnumerator EnemyShootTimer(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);

        Instantiate(enemyBullet, enemyTransform.position, quaternion.identity);
        StartCoroutine(EnemyShootTimer(fireRate));
    }

    private void OnHealthChanged(float health, float delta)
    {
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashColorForDuration(delta < 0 ? Color.red : Color.green, 0.5f));
        
        GameObject gemstoneGO = Instantiate(gemstone, transform.position, Quaternion.identity);
        gemstoneGO.GetComponent<Gemstone>().SetValue(Random.Range(20, 60));
        gemstoneGO.GetComponent<Rigidbody2D>().AddForce(getDireciton() * 300);
        
        if (gameObject == null) return;
        if (health <= 0f) Destroy(gameObject);
    }

    private IEnumerator FlashColorForDuration(Color color, float duration)
    {
        float alpha = 0;
        _mpb.SetColor(FlashColor, color);
        _mpb.SetTexture(MainTex, _spriteRenderer.sprite.texture);
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * (1 / duration * 2);
            _mpb.SetFloat(LerpAlpha, alpha);
            _spriteRenderer.SetPropertyBlock(_mpb);
            yield return null;
        }

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * (1 / duration * 2);
            _mpb.SetFloat(LerpAlpha, alpha);
            _spriteRenderer.SetPropertyBlock(_mpb);
            yield return null;
        }
    }
}
