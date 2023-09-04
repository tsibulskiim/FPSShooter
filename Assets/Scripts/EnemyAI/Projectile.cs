using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float radius = 1;
    public int damageAmount = 15;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            PlayerManager.TakeDamage(damageAmount);
    }
}
