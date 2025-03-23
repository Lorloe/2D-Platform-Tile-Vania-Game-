using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickUp = 1;

    private bool wasCollected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().IncreaseCoinPoint(pointsForCoinPickUp);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position); // At the point when player pick up the coin
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
