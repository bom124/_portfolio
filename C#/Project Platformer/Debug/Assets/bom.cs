using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bom : MonoBehaviour
{
  public float reload;
  public float damage;
  private PlayerController player;

  private void Start()
  {
    player = FindObjectOfType<PlayerController>();
  }

  void Destroy()
  {
    Destroy(gameObject);
  }
  public void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if(reload==0)
      {
        reload = 1;
        player.health -= damage;
      }
    }
  }
}
