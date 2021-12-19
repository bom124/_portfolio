using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrelok : MonoBehaviour
{
  public float timeBtwAttack;
  public float startTimeBtwAttack;
  public int positionOfPatrol;
  public Transform point;
  public bool moveingRight;
  Transform player1;
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
  public GameObject bullet;
  public Transform shotPoint;
  public int distance;

  private void Start()
  {
    player1 = GameObject.FindGameObjectWithTag("Player").transform;
    player = FindObjectOfType<PlayerController>();
    anim = GetComponent<Animator>();

  }

  private void Update()
  {
    if (Vector2.Distance(transform.position, player1.position) <= distance && ((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)<=6))
    {
      speed = 0;
      anim.SetTrigger("attack");
    }
    if((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)>6)
    {
      speed = speed_1;
      anim.SetTrigger("enemyNotAttack ");
    }
    if(Vector2.Distance(transform.position, player1.position) > distance)
    {
      speed = speed_1;
      anim.SetTrigger("enemyNotAttack ");
    }
    if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
    {
      chill = true;
    }
    if((Vector2.Distance(transform.position,player1.position)<stoppingDistance)&& ((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)<=6))
    {
      angry = true;
      chill = false;
      goBack = false;

    }
    if((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)>6)
    {
      goBack = true;
      angry = false;
    }
    if (Vector2.Distance(transform.position, player1.position) > stoppingDistance)
    {
      goBack = true;
      angry = false;
    }
    if (health <= 0)
    {
      Instantiate(enemyEffect, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    if (chill == true)
    {
      Chill();
    }
    else if (angry == true)
    {
      Angry();
    }
    else if (goBack == true)
    {
      GoBack();
    }

  }
  public void TakeDamage(int damage)
  {
    health -= damage;
  }
  public void Attack()
  {
    Instantiate(bullet, shotPoint.position, shotPoint.rotation);
  }
  void Chill()
  {
    if (transform.position.x > point.position.x + positionOfPatrol)
    {
      moveingRight = false;


    }
    else if (transform.position.x < point.position.x - positionOfPatrol)
    {
      moveingRight = true;
    }
    if (moveingRight)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
      transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }
    else
    {
      transform.eulerAngles = new Vector3(0, 180, 0);
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
    if (point.transform.position.x > transform.position.x)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else
    {
      transform.eulerAngles = new Vector3(0, 180, 0);
    }
    transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
  }
}
