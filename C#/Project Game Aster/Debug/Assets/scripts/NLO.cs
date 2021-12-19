using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLO : MonoBehaviour
{
  private float speed;
  private int reward;
  private GameObject player;
  private const float time_reload_const=3.5f;
  private float time_reload=0f;
  private asteroidControl asteroidControl;
  //класс-контроллер
  private AudioSource[] sounds;
  //массив со звуками
  void Start()
  {
    reward = 200;
    speed = 3f;
    sounds = GetComponents<AudioSource>();
    asteroidControl = FindObjectOfType<asteroidControl>();
    if(transform.position.x==-asteroidControl.x_NLO_spawn)
    {
      speed*=1;
    }
    if(transform.position.x==asteroidControl.x_NLO_spawn)
    {
      speed*=-1;
    }
    //задаётся направление в соответсвии с позицей (правая граница или левая)
  }
  void Update()
  {
    if(sounds[0].isPlaying==false)
    {
      sounds[0].Play();
    }
    //проигрывание звука, только если он уже не проигрывается
    if(transform.position.x<=-(asteroidControl.x_NLO_spawn+ asteroidControl.size_rate) || transform.position.x>= (asteroidControl.x_NLO_spawn + asteroidControl.size_rate))
    {
      Death();
      //смерть, если НЛО вылетело за пределы экрана
    }
    if(time_reload<=0)
    //метод стрельбы
    {
      player = asteroidControl.player;
      //поиск живого игрока
      GameObject bullet = ObjectPool.SharedInstance.GetPooledObjectBulletNLO();
      if(!asteroidControl.die)
      {
        bullet.transform.position = transform.position;
        bullet.transform.up =  player.transform.position - transform.position;
        bullet.SetActive(true);
        //метод, вызывающий стрельбу в сторону игрока
        sounds[1].Play();
        //проигрывание звука выстрела
        time_reload= time_reload_const;
        //задаётсяы новое время перезарядки
      }
    }
    else
    {
      time_reload-=Time.deltaTime;
      //уменьшение таймера перезарядки
    }

    transform.Translate(Vector2.right * speed * Time.deltaTime);
    //движение НЛО
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("player"))
      //если игрок сталкивается с НЛО, то НЛО уничтожается, игрок уничтожается, игрок получает очки
    {
      asteroidControl.score+= reward;
      asteroidControl.Death();
      Death();
    }
    if(other.CompareTag("Assteroid"))
      //если астероид сталкивается с НЛО, то НЛО уничтожается, игрок не получает очки
    {
      Death();
    }
    if(other.CompareTag("BulletPlayer"))
    //если пуля игрока сталкивается с НЛО, то НЛО уничтожается, игрок получает очки
    {
      asteroidControl.score+= reward;
      Death();
    }
  }
  public void Death()
  {
    asteroidControl.timeRespawnNLO = asteroidControl.timeRespawnNLO_const;
    //зажаётся новое время рэспауна
    asteroidControl.NLOdie=true;
    Destroy(gameObject);
  }
}
