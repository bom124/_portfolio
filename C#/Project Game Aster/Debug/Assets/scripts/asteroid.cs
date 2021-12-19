using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class asteroid : MonoBehaviour
//класс, отвечающий за астероиды
{
  public float speed;
  internal float speedAsteroidPosle;
  //скорость выпавших после разламывания астероида
  internal float speedAsteroidGranicaNiz;
  //нижняя граница случайной скорости выпавших после разламывания астероида
  public float speedAsteroidGranicaVerh;
  //верхняя граница случайной скорости выпавших после разламывания астероида
  internal asteroidControl asteroidControl;
  internal int Type_asteroid;
  internal int count_small_asteroids_lose;
  internal int reward;
  public virtual void Start()
  {
    asteroidControl = FindObjectOfType<asteroidControl>();
    //определение класса-контроллера
  }
  public virtual void Update()
  {
    transform.Translate(Vector2.up * speed * Time.deltaTime);
    //метод движения астероида
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("NLO"))
    //если астероид сталкивается с НЛО, он полностью уничтожается  и после него не остаётся ничего,  игрок не получает очков
    {
      DestroyAsteroid(count_small_asteroids_lose, reward * 0);
      //в соответсвии с этим из числа маленьких астероидов вычитаеся соответсвующее число потерянных маленьких астероидов
    }
    else if(other.CompareTag("player"))
    //если астероид сталкивается с игроком, он уничтожается,   и после него не остаётся ничего, игрок получает очки,игрок умирает,
    {
      asteroidControl.Death();
      DestroyAsteroid(count_small_asteroids_lose, reward);
      //прибавляется соответсвующее число очком
      //в соответсвии с этим из числа маленьких астероидов вычитаеся соответсвующее число потерянных маленьких астероидов
    }
  }
  public void DestroyAsteroid(in int count_small_asteroids_lose, in int reward_on_metod)
  {
    asteroidControl.countAs -= count_small_asteroids_lose;
    asteroidControl.score += reward;
    asteroidControl.soundExplosion(Type_asteroid);
    gameObject.SetActive(false);
    //уничтожение астероида и проигрывание соответсвующего звука
  }
}
