using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IAttackable, IDamageable
{
    private SpriteRenderer sprite;
    private Animator animator;
    protected Rigidbody2D rigid;
    private IEnemyMarker _markers;
    private EnemyMarkerUI markUI;
    protected float xVelocity;
    protected float speed;

    public List<AttackKey> markers
    {
        get
        {
            return _markers.ToList();
        }
    }
    public AttackKey currentMarker
    {
        get
        {
            return _markers.peak;
        }
    }
    public int hp
    {
        get
        {
            if (_markers != null)
            {
                return _markers.hp;
            }
            else
            {
                // Return a default value or throw an exception based on your requirements
                return 0;
            }
        }
    }
    public int attackPower { get { return 1; } }

    [SerializeField] private GameObject markUIPrefab;
    [SerializeField] private GameObject healthItemPrefab;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        GameObject markUIObject = Instantiate(markUIPrefab);
        markUIObject.transform.position = new Vector3(0f, 1.2f, 0f);
        markUIObject.transform.SetParent(transform, false);
        markUI = markUIObject.GetComponent<EnemyMarkerUI>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        sprite.flipX = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAnimation();
        if(rigid.position.x < Constants.LEFT_BOUND) Destroy(gameObject); 
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null) AttackPlayer(player);
    }

    protected virtual void SetAnimation()
    {
        Vector2 movement = rigid.velocity;
        if (!Mathf.Approximately(movement.x, 0.0f)) sprite.flipX = (movement.x < 0f);
    }

    public void SetMarker(IEnemyMarker markers)
    {
        _markers = markers;
        markUI.SetInitialMarker(this.markers);
    }
    public virtual void SetSpeed(float multiplier)
    {
        speed = multiplier;
        xVelocity = -speed * Utils.GetBaseSpeed(MusicManager.Instance.bpm);
        rigid.velocity = new Vector2(xVelocity, 0f);
    }

    public bool CanDamage(AttackKey key)
    {
        if (hp == 0) return false;
        return (_markers.peak & key) == _markers.peak;
    }
    public void Damage(int damage)
    {
        if (hp == 0) return;
        _markers.Decrease(damage);
        markUI.SetMarker(markers);
        if (damage > 1) animator?.SetTrigger("critHit");
        else animator?.SetTrigger("hit");
        if (hp == 0)
        {
            OnDead();
            Destroy(gameObject);
        }
        else Knockback();
    }
    public void AttackPlayer(Player player)
    {
        player.Hit(this.attackPower);
    }

    protected virtual void OnDead()
    {
        if (Random.Range(0f, 1f) < 0.05f)
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
    }
    private void Knockback()
    {
        float knockbackDistance = Constants.BASE_ENEMY_DISTANCE / 8 * speed;
        Vector2 toMove = rigid.position;
        toMove.x += knockbackDistance;
        rigid.MovePosition(toMove);
    }
}
