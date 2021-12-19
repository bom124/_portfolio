using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
  public float speed;
  public float lifetime;
  public float distance;
  public int damage;
  public LayerMask whatIsSolid;
  private PlayerController player;
  public GameObject ballEffect;


  private void Start()
  {
    player = FindObjectOfType<PlayerController>();
  }
  private void Update()
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
    if (hitInfo.collider != null)
    {
      if (hitInfo.collider.CompareTag("Player"))
      {
        player.health -= damage;
      }
      Instantiate(ballEffect, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    transform.Translate(Vector2.up * speed * Time.deltaTime);
  }
}
