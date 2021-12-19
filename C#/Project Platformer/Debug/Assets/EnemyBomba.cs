using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomba : MonoBehaviour
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


  private void Start()
  {
    player1 = GameObject.FindGameObjectWithTag("Player").transform;
    player = FindObjectOfType<PlayerController>();
    anim = GetComponent<Animator>();

  }
  private void Update()
  {
    if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
    {
      anim.SetBool("Run", true);
      chill = true;
    }
    if (Vector2.Distance(transform.position, player1.position) < stoppingDistance && ((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)<=6))
    {
      anim.SetBool("Angry", true);
      angry = true;
      chill = false;
      goBack = false;

    }
    if((transform.position.y-player1.position.y)*(transform.position.y-player1.position.y)>6)
    {
      anim.SetBool("Angry", false);
      goBack = true;
      angry = false;
    }
    if (Vector2.Distance(transform.position, player1.position) > stoppingDistance)
    {
      anim.SetBool("Angry", false);
      goBack = true;
      angry = false;
    }
    if (health <= 0)
    {
      Death();
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
  public void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      speed = 0;
      anim.SetTrigger("bom");
    }
  }
  void Chill()
  {
    if (transform.position.x > point.position.x + positionOfPatrol)
    {
      moveingRight = false;
      transform.eulerAngles = new Vector3(0, 180, 0);


    }
    else if (transform.position.x < point.position.x - positionOfPatrol)
    {
      moveingRight = true;
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    if (moveingRight)
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
  void Death()
  {
    Instantiate(enemyEffect, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }
}
