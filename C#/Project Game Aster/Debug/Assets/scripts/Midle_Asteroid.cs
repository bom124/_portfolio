using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midle_Asteroid : asteroid
{
// Start is called before the first frame update
  public override void Start()
  {
    count_small_asteroids_lose = 2;
    reward = 100;
    Type_asteroid = 1;
    base.Start();
  }

// Update is called once per frame
  public override void Update()
  {
    base.Update();
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    base.OnTriggerEnter2D(other);
    if (other.CompareTag("BulletPlayer"))
    //если астероид сталкивается с пулей игрока, астероид разламывается, игрок получает очки
    {
      DestroyAsteroid(count_small_asteroids_lose * 0, reward);
      speedAsteroidPosle = Random.Range(speedAsteroidGranicaNiz, speedAsteroidGranicaVerh);
      //задаётся случайная скорость
      GameObject asteroid = ObjectPool.SharedInstance.GetPooledObjectAsSmall();
      //получание неактивного астероида
      asteroid.transform.position = transform.position;
      asteroid.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 45);
      //астероид поменьше поворачивается на 45 градусов в другую сторону
      asteroid.GetComponent<asteroid>().speed = speedAsteroidPosle;
      //задаётся случайная скорость астероиду поменьше
      asteroid.SetActive(true);
      asteroid = ObjectPool.SharedInstance.GetPooledObjectAsSmall();
      //получание неактивного астероида
      asteroid.transform.position = transform.position;
      asteroid.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 45);
      //астероид поменьше поворачивается на 45 градусов в другую сторону
      asteroid.GetComponent<asteroid>().speed = speedAsteroidPosle;
      //задаётся так же случайная скорость астероиду поменьше
      asteroid.SetActive(true);
      //прибавление очков
      
    }
  }
}
