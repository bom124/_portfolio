using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed;
  public float lifetime;
  public float distance;
  public int damage;
  public LayerMask whatIsSolid;
  public GameObject ballEffect;

  private void Update()
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
    if(hitInfo.collider != null)
    {
      if(hitInfo.collider.CompareTag("Enemy"))
      {
        hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
      }
      if (hitInfo.collider.CompareTag("EnemyStrelok"))
      {
        hitInfo.collider.GetComponent<EnemyStrelok>().TakeDamage(damage);
      }
      if (hitInfo.collider.CompareTag("EnemyBomba"))
      {
        hitInfo.collider.GetComponent<EnemyBomba>().TakeDamage(damage);
      }
      if (hitInfo.collider.CompareTag("EnemyBoss"))
      {
        hitInfo.collider.GetComponent<EnemyBoss>().TakeDamage(damage);
      }
      Instantiate(ballEffect, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    transform.Translate(Vector2.up * speed * Time.deltaTime);
  }
}
