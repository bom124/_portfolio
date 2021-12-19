using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posOut : MonoBehaviour
//метод, телепортирующий объекты при выходе их за экран
{
  const float size_rate = 1; 
  private float border_up;
  private float border_dowm;
  private float border_left;
  private float border_right;
  void Awake()
  {
    border_up = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y+ size_rate;
    border_dowm = -border_up;
    border_right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + size_rate;
    border_left = -border_right;
  }
  void Update()
  {
    if(transform.position.x<border_left)
    //значения выьрал по ощущению
    //метод, вызывающийся при выходе за экран влево
    {
      transform.position = new Vector3(transform.position.x+border_right*2,transform.position.y*-1,transform.position.z);
    }
    else if(transform.position.x> border_right)
    //метод, вызывающийся при выходе за экран вправо
    {
      transform.position = new Vector3(transform.position.x-border_right*2, transform.position.y*-1,transform.position.z);
    }
    else if(transform.position.y<border_dowm)
    //метод, вызывающийся при выходе за экран вниз
    {
      transform.position = new Vector3(transform.position.x*-1,transform.position.y+border_up*2, transform.position.z);
    }
    else if(transform.position.y> border_up)
    //метод, вызывающийся при выходе за экран вверх 
    {
      transform.position = new Vector3(transform.position.x*-1,transform.position.y-border_up*2, transform.position.z);
    }
  }
}
