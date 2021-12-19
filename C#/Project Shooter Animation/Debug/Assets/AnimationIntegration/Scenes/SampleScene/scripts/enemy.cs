using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
  private Animator anim;
  public GameObject body;
  //Тело, откуда будет браться Animator
  public Rigidbody[] AllRigidbodys;
  //беру все Rigidbody, чтобы задавать им состояние
  public bool live;
  //переменная, показывающая жив ли враг.
  private float timeRespawn;
  //время респауна врага
  public void Awake()
  {
    timeRespawn = 5;
    //задаю время рэспауна, значение взял из ТЗ
    for(int i=0;i<AllRigidbodys.Length;i++)
    {
      anim = body.GetComponent<Animator>();
      AllRigidbodys[i].isKinematic=true;
    }
    //задаю всем Rigidbody во вражеском персонаже состояние
  }
  public void Update()
  {
    if(live==false)
    {
      //проверяю, жив ли вражеский персонаж
      anim.enabled = false;
      for(int i=0;i<AllRigidbodys.Length;i++)
      {
        AllRigidbodys[i].isKinematic=false;
      }
      //если персонаж мёртв, то выключаю Animator и задаю Rigidbody соответсвующее состояние
      if(timeRespawn<=0)
      {
        //проверяю время респауна
        transform.position = new Vector3(Random.Range(-10,10),transform.position.y,Random.Range(-10,10));
        timeRespawn = 5;
        live=true;
        anim.enabled = true;
        //если оно меньше 5,значение взял из ТЗ, задаётся новая позицию(значения взял случайные, но не слишком далёкие),
        // время респуна, Animator и переменная жизни  обнуляется
        for(int i=0;i<AllRigidbodys.Length;i++)
        {
          AllRigidbodys[i].isKinematic=true;
        }
        //Rigidbody задаётся состояние для рэгдолла
      }
      else
      {
        timeRespawn-=Time.deltaTime;
        //если оно не меньше 5, то включается таймер
      }
    }
  }
}
