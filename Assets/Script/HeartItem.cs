using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if(player == null) return;
        if(!player.canHeal) return;
        player.ChangeHealth(1);
        Destroy(gameObject);
    }
}
