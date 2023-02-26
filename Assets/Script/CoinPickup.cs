using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;
    [SerializeField] private int coinValue = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameSession>().AddToScore(coinValue);
            
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position,.15f);
            Destroy(gameObject);
        }
    }
}
