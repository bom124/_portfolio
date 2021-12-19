using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
//класс, отвечающий за поведение пуль
{
  internal const float speed=7f;
  //скорость пули
  internal const float lifeTime = 2.5f;
  //время жизни пули
  internal float time;
  //переменная времени
  internal asteroidControl asteroidControl;
  //класс-контроллер


  internal virtual void Start()
  {
    asteroidControl = FindObjectOfType<asteroidControl>();
  }
  internal virtual void Update()
  {
    if(time<lifeTime)
    {
      time+=Time.deltaTime;
      //проверка времени жизни пули и прибавление прошедшего времени
    }
    else
    {
      //в противном случае уничтожение пули
      DestroyBullet();
    }
    transform.Translate(Vector2.up * speed * Time.deltaTime);
    //метод движения пули
  }
  internal virtual void OnTriggerEnter2D(Collider2D other)
  //метод,проверяющий попадание пули в другие объекты
  {
    if(other.CompareTag("Assteroid"))
    {
      DestroyBullet();
      //при попадании пули игрока в астероид пуля уничтожается
    }
    else if(other.CompareTag("NLO"))
    {
      DestroyBullet();
      //при попадании пули игрока в НЛО пуля уничтожается
    }
  }
  internal void DestroyBullet()
  {
    time=0;
    gameObject.SetActive(false);
    //пуля после уничтожения попадает обратно в object pool, поэтому время необходимо обнулить
  }
}
