using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Asteroid : asteroid
{
  public override void Start()
  {
    count_small_asteroids_lose = 1;
    reward = 150;
    Type_asteroid = 2;
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
      DestroyAsteroid(count_small_asteroids_lose, reward);

    }
  }
}
