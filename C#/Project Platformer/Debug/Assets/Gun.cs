using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  public GameObject bullet;
  public Transform shotPoint;
  public Transform shotPoint_d1;
  public Transform shotPoint_d2;
  public float moveInput;
  public float drobovik;

  public float basketbol;

  public float sword;
  public float sword_range_attack;
  public int damage;


  public float timeBtwShots;
  public float timeBtwShots_1;
  public float startTimeBtwShots_1;
  public float startTimeBtwShots;
  public float getShoot;
  public float speed;
  public LayerMask enemy;
  private PlayerController player;
  private Animator anim;

  private void Start()
  {
    anim = GetComponent<Animator>();
    player = FindObjectOfType<PlayerController>();

  }

  private void FixedUpdate()
  {
    if(getShoot==0)
    {
      anim.SetTrigger("drop");
    }
    if(getShoot==1)
    {
      anim.SetTrigger("hand");

      if(Input.GetMouseButton(0) && timeBtwShots<=0)
      {
        anim.SetTrigger("shoot");
      }
      else
      {
        anim.SetTrigger("hand");
        timeBtwShots-=Time.deltaTime;
      }
    }



  }
  void sword_break()
  {
    timeBtwShots=startTimeBtwShots;
  }
  void sword_shoot()
  {
    anim.SetTrigger("sword_break");
    Collider2D[] enemies = Physics2D.OverlapCircleAll(shotPoint.position, sword_range_attack,enemy);
    for(int i =0;i<enemies.Length;i++)
    {
      if(enemies[i].GetComponent<Enemy>()!=null)
      {
        enemies[i].GetComponent<Enemy>().TakeDamage(damage);
      }
      if(enemies[i].GetComponent<EnemyBomba>()!=null)
      {
        enemies[i].GetComponent<EnemyBomba>().TakeDamage(damage);
      }
      if(enemies[i].GetComponent<EnemyStrelok>()!=null)
      {
        enemies[i].GetComponent<EnemyStrelok>().TakeDamage(damage);
      }
      if(enemies[i].GetComponent<EnemyBoss>()!=null)
      {
        enemies[i].GetComponent<EnemyBoss>().TakeDamage(damage);
      }
    }
  }
  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(shotPoint.position,sword_range_attack);
  }
  void drobovik_shoot()
  {
    Instantiate(bullet, shotPoint.position, shotPoint_d1.rotation);
    Instantiate(bullet, shotPoint.position, shotPoint_d2.rotation);
    Instantiate(bullet, shotPoint.position, shotPoint.rotation);
    timeBtwShots=startTimeBtwShots;

  }
}
