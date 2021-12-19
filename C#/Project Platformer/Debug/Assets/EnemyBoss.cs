using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
  public  float timeBtwAttack;
  public float startTimeBtwAttack;
  public int positionOfPatrol;
  public Transform point;
  public bool moveingRight;
  Transform player1;
  public Image bar;
  public float stoppingDistance;
  bool chill = false;
  bool angry = false;
  bool goBack = false;




  public GameObject enemyEffect;
  public int health;
  public float speed;
  public float speed_1;
  public int damage;
  private Animator anim;
  private PlayerController player;


  private void Start()
  {
    player1 = GameObject.FindGameObjectWithTag("Player").transform;
    player = FindObjectOfType<PlayerController>();
    anim = GetComponent<Animator>();

  }
  private void Update()
  {
    bar.fillAmount = health*0.0001f;
    if(Vector2.Distance(transform.position,point.position)<positionOfPatrol && angry==false)
    {
      chill = true;
    }
    if((Vector2.Distance(transform.position,player1.position)<stoppingDistance))
    {
      angry = true;
      chill = false;
      goBack = false;

    }
    if(Vector2.Distance(transform.position, player1.position) > stoppingDistance)
    {
      goBack = true;
      angry = false;
    }

    if (health<=0)
    {
      Instantiate(enemyEffect, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    if(chill==true)
    {
      anim.SetBool("Run", true);
      Chill();
    }
    else if(angry==true)
    {
      anim.SetBool("Run", true);
      anim.SetBool("1", true);
      Angry();
    }
    else if(goBack==true)
    {
      anim.SetBool("Run", true);
      anim.SetBool("1", true);
      GoBack();
    }



  }
  public void TakeDamage(int damage)
  {
    health-=damage;
  }
  public void OnTriggerStay2D(Collider2D other)
  {
    if(other.CompareTag("Player"))
    {
      if(timeBtwAttack<=0)
      {
        anim.SetBool("Run", false);
        speed = 0;
        anim.SetTrigger("attack");
      }
      else
      {
        timeBtwAttack-=Time.deltaTime;
      }
    }
  }
  void Chill()
  {
    if(transform.position.x > point.position.x + positionOfPatrol)
    {
      moveingRight = false;
      transform.eulerAngles = new Vector3(0, 180, 0);


    }
    else if (transform.position.x < point.position.x - positionOfPatrol)
    {
      moveingRight = true;
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    if(moveingRight)
    {
      transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }
    else
    {
      transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }
  }
  void Angry()
  {
    if (player.transform.position.x > transform.position.x)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else
    {
      transform.eulerAngles = new Vector3(0, 180, 0);
    }
    transform.position = Vector2.MoveTowards(transform.position, player1.position, speed * Time.deltaTime);
  }
  void GoBack()
  {
    transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    if (point.transform.position.x > transform.position.x)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else
    {
      transform.eulerAngles = new Vector3(0, 180, 0);
    }
  }
  public void OnEnemyAttackBreak()
    {
        timeBtwAttack = startTimeBtwAttack;
    }
  public void OnEnemyAttack()
  {
    speed = speed_1;
    timeBtwAttack=startTimeBtwAttack;
    player.health-=damage;
    anim.SetTrigger("enemyNotAttack ");

  }
}
