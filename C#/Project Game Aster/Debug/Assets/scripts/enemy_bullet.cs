using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_bullet : bullet
{
  // Start is called before the first frame update
  internal override void Start()
  {
      base.Start();
  }

  // Update is called once per frame
  internal override void Update()
  {
      base.Update(); 
  }
  internal virtual void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("player"))
    {
      //при попадании пули НЛО в игрока пуля уничтожается, в скрипт-контролле вызывается метод смерти пероснажа
      asteroidControl.Death();
      DestroyBullet();
    }
  }

}
