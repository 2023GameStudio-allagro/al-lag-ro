using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IAttackable
{
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
            return _markers.hp;
        }
    }
    public int moveSpeed{get; protected set;}

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject markUIPrefab;

    protected virtual void Awake()
    {
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
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        MoveTowardPlayer();
    }

    protected void MoveTowardPlayer()
    {
        if(player == null || player.Equals(null)) return;
        Vector3 velocity = (player.transform.position - transform.position);
        if(velocity.magnitude < 0.5f) return;
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;
    }

    public void SetHP(int maxHP)
    {
        _markers = new MarkerList(maxHP);
        markUI.SetInitialMarker(markers);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public bool CanDamage(AttackKey key)
    {
        if(hp == 0) return false;
        return (_markers.peak & key) == _markers.peak;
    }
    public void Damage(int damage)
    {
        _markers.Decrease(damage);
        markUI.SetMarker(markers);
        animator?.SetTrigger("hit");
        if(hp == 0) OnDead();
    }

    protected virtual void OnDead()
    {
        Destroy(gameObject);
    }
}
