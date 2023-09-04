using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHP = 100;
    public GameObject projectile;
    public Transform projectilePoint;
    public Animator animator;

    public void Shoot()
    {
        GameObject bullet = Instantiate(projectile, projectilePoint.position, Quaternion.Euler(0, 180, 0));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 30, ForceMode.Impulse);
        Destroy(bullet, 2);
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHP -= damageAmount;
        if (enemyHP <= 0)
        {
            animator.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;
            PlayerManager.score++;
        }
        else
            animator.SetTrigger("damage");

    }
}
