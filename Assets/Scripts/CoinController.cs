using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameController>().CoinPickup(transform.position);
        }
    }
}
