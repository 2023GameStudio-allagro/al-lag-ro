using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : MonoBehaviour
{
    void Update()
    {
        float speed = Utils.GetBaseSpeed(MusicManager.Instance.bpm);
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
        if(pos.x < Constants.LEFT_BOUND) Destroy(gameObject); 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if(player == null) return;
        if(!player.canHeal) return;
        player.ChangeHealth(1);
        Destroy(gameObject);
    }
}
