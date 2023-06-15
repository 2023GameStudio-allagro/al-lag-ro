using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyMovement : MonoBehaviour, IAttackable, IDamageable
{
    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D rigid;
    private MarkerList _markers;
    private EnemyMarkerUI markUI;

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

    public bool shouldFollowCamera = false;// spawn된 후에 바로 false
    float moveSpeed = 2f;
    // Update is called once per frame
    private void Update()
    {
        MoveLeft();
        if (shouldFollowCamera)
        {
        }
    }

    // Call this method to stop the enemy from following the camera
    public void StopFollowingCamera()
    {
        shouldFollowCamera = false;
    }
    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }


    public void SetHP(int maxHP)
    {

        _markers = new MarkerList(maxHP);
        markUI.SetInitialMarker(markers);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public bool CanDamage(AttackKey key)
    {
        if (hp == 0) return false;
        return (_markers.peak & key) == _markers.peak;
    }
    public void Damage(int damage)
    {
        _markers.Decrease(damage);
        markUI.SetMarker(markers);
        if (damage > 1) animator?.SetTrigger("critHit");
        else animator?.SetTrigger("hit");
        if (hp == 0)
        {
            OnDead();
            Destroy(gameObject);
        }
    }
    public void AttackPlayer(Player player)
    {
        player.Hit(this.attackPower);
    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null) AttackPlayer(player);
    }


    protected virtual void OnDead()
    {
        if (Random.Range(0f, 1f) < 0.05f)
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }

        // // Play dead animation
        // Animator animator = GetComponent<Animator>();
        // if (animator != null)
        // {
        //     animator.SetTrigger("Dead");//TODO: replace "Dead" with the actual parameter name you have set up in animator controller
        // }

        // // Disable other components (e.g., movement, collision)
        // // Add code here to disable any relevant components or scripts on the enemy

        // // Destroy the enemy after the animation duration
        // float animationDuration = 1f;
        // Destroy(gameObject, animationDuration);


    }
}