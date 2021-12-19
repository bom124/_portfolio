using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playercontroler : MonoBehaviour
{
  private Transform transPlayer;
  private float speedMove;
  //скорость персонажа
  private float rotationPose;
  //позиция поворота персонажа
  private float rotationBack;
  //переменная, обеспечивающая поворот ног персонажа во время анимации бега спиной вперёд
  private float speedRotation;
  //скорость поворота персонажа
  private Vector3 moveVector;
  // вектор направления движения
  private Vector3 mousePos;
  //вектор, показывающий позицию мыши
  private Plane playerPlane;
  private Ray ray;
  private float hitdist;
  //переменная, оюозначающая границу Raycast
  public Transform noga1;
  public Transform noga2;
  //положения ног
  private CharacterController ch_controller;
  //для обработки движения было решено использовать CharacterController, потому что персонаж не физичен(Rigidbody отсутствует)
  private Animator anim;
  public GameObject weapon;
  //оружие, которое держит персонаж
  public Mesh rifle;
  //сетка винтовки
  public Mesh sword;
  //сетка меча
  private GameObject enemy;
  public Text dobivanieText;
  //текст, выскакивающий при вхождении в зону добивания и исчезающая при выходе из неё

  private void Start()
  {
    transPlayer = transform;
    //задаю transform персонажа,чтобы кешировать его
    hitdist = 0.0f;
    speedMove = 0.08f;
    speedRotation = 100f;
    //задаю скоросить персонаж и поворота, значение выбрал по собственному усмотрению
    ch_controller = GetComponent<CharacterController>();
    anim = GetComponent<Animator>();
  }

  public void LateUpdate()
  {
    //изменения поворота персонажа и движене решил сделать в LateUpdate, чтобы не конфликтовать с предоставленными анимациями, в которых указан поворот. Можно было изменить анимацииЮ, это было бы легче,но мне кажется, вы бы не хотели, чтобы я справился так
    playerPlane = new Plane(Vector3.up, transPlayer.position);
    // реализую поворот персонажа за мышкой. Решил сделать поворот ВСЕГО персонажа за мышкой, а поворот ног сделать зависимым только от движения персонажа
    // для поворота персонажа решил использовать достаточно затратный, но точный метод с определением положения курсора и последующим поворотом
    //сперва обозначаем плоскость
    ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    //создаю луч из камеры в положение курсора
    if (playerPlane.Raycast (ray, out hitdist))
		{
        	Vector3 necessaryPoint = ray.GetPoint(hitdist);
          //найдём координаты  мыши
        	Quaternion necessaryRotation = Quaternion.LookRotation(necessaryPoint - transPlayer.position);
          //определим необходимое вращение
          transPlayer.rotation = Quaternion.Slerp(transPlayer.rotation, necessaryRotation,speedRotation);
          //поворачиваем обЪект до найденого ранее значения
		}
    CharacterMove();
    //функция, осуществляющая движение и поворот ног персонажа
    enemy = GameObject.FindGameObjectWithTag("enemy");
    //обозначаем врага. Так так враг один - сделал через один  GameObject. Если бы врагов было несколько - выполнил вы всё через массив
    if(enemy!=null)
    {
      //проверяем, что враг есть
      if(Vector3.Distance(transPlayer.position,enemy.transform.position)<3&&enemy.GetComponent<enemy>().live==true)
      //проверяю дистанцию до врага и его жизненное состояние
      {
        //в случае близкого расстояние, значение подобрал такое, какое считаю лучшим, и хорошем состояние врага метод запускается
        if(Input.GetKeyDown(KeyCode.Space))
        {
          transPlayer.position = new Vector3(enemy.transform.position.x,enemy.transform.position.y,enemy.transform.position.z-2);
          transPlayer.rotation = enemy.transform.rotation;
          //при нажатии пробела персонаж телепортируется за спину врага, и поворачивается к нему лицом
          speedRotation = 0;
          speedMove = 0;
          //скорость персонаж и поворота решил обнулить, чтобы он зафиксировался во время анимации
          weapon.GetComponent<MeshFilter>().mesh = sword;
          //винтовка меняется на меч
          anim.SetTrigger("finishing");
          //проигрывается анимация добивания
        }
        dobivanieText.enabled = true;
        // при вхождении в зону добивания отображается соответсвующий текст
      }
      else
      {
        dobivanieText.enabled = false;
        //если враг слишком далеко или игрок вышел из зоны добивания, то текст изчезает
      }
    }
  }
  public void enemyDie()
  {
    enemy.GetComponent<enemy>().live = false;
    //метод вызывается во время анимации добивания, заставляет врага умереть
  }
  public void finishingOver()
  {
    //мметод вызывается во время конца анимации добивания
    anim.SetTrigger("finishingOver");
    speedRotation = 100;
    speedMove = 0.08f;
    weapon.GetComponent<MeshFilter>().mesh = rifle;
    //мметод вызывается во время конца анимации добивания, во время него возвращается к прежним значениям скорость персонажа и поворота, меч меняется на винтовку
  }
  private void CharacterMove()
  {

    moveVector.x = Input.GetAxis("Vertical")*speedMove*-1;
    //движение персонажа по оси х. Для удобства восприятия игроком инверсировал значение, чтобы на W персонаж двигался вверх
    moveVector.z = Input.GetAxis("Horizontal")*speedMove;
    //движение персонажа по оси z
    if(transPlayer.eulerAngles.y<=270&transPlayer.eulerAngles.y>180)
    //поворот персонажа для удобства восприятия обозначил по четвертям, как в тригонометрической окружности
    {
      rotationPose=1;
      //первая четверть
    }
    else if(transPlayer.eulerAngles.y<=180&transPlayer.eulerAngles.y>90)
    {
      rotationPose=2;
      //вторая четверть
    }
    else if(transPlayer.eulerAngles.y<=90&transPlayer.eulerAngles.y>0)
    {
      rotationPose=3;
      //третья четверть
    }
    else if(transPlayer.eulerAngles.y<=360&transPlayer.eulerAngles.y>270)
    {
      rotationPose=4;
      //четвёртая четверть
    }
    if(moveVector.x==0&moveVector.z==0)
    //метод, проверяющий стоит ли персонаж
    {
      anim.SetBool("runBack",false);
      anim.SetBool("run",false);
      // в случает отсутвия движения срабатывает соответсвующяя анимация
    }
    else
    {
      anim.SetBool("run",true);
      // исходный случай означает, что персонаж бежит
    }
    if(moveVector.x>0&moveVector.z>0)
    //метод определяющий куда бежит пероснаж и в соответсвии с этим задающий поворот ног
    {
      //пероснаж бежит по диагонали вниз-вправо
      CheckRotation(1,0,-0.3f);
      //метод,проверяющий, бежит ли персонаж спиной
      Rotation(150,rotationBack);
      //метод, разворачивающий ноги
    }
    else if(moveVector.x>0&moveVector.z<0)
    {
      //пероснаж бежит по диагонали вниз-влево
      CheckRotation(4,0,0.2f);
      Rotation(225,rotationBack);
    }
    else if(moveVector.x<0&moveVector.z<0)
    {
      //пероснаж бежит по диагонали вверх-влево
      CheckRotation(3,0,5);
      Rotation(-45,rotationBack);
    }
    else if(moveVector.x<0&moveVector.z>0)
    {
      //пероснаж бежит по диагонали вверх-вправо
      CheckRotation(2,0,5);
      Rotation(45,rotationBack);
    }
    else if(moveVector.z>0)
    {
      //пероснаж бежит по влево
      CheckRotation(1,2,-1);
      Rotation(90,rotationBack);
    }
    else if(moveVector.z<0)
    {
      //пероснаж бежит вправо
      CheckRotation(3,4,-1);
      Rotation(-90,rotationBack);
    }
    else if(moveVector.x>0)
    {
      //пероснаж бежит вниз
      CheckRotation(1,4,0);
      Rotation(180,rotationBack);
    }
    else if(moveVector.x<0)
    {
      //пероснаж бежит вверх
      CheckRotation(2,3,0.5f);
      Rotation(360,rotationBack);
    }
    ch_controller.Move(moveVector);
    //движение персонажа на заданный раннее вектор
  }
  public void CheckRotation(int i,int b,float c)
  //метод,проверяющий, бежит ли персонаж спиной
  {
    if(rotationPose==i||rotationPose==b)
    //проверяет, повёрнут ли персонаж в определённую зону
    {
      rotationBack=c;
      //если повёрнут, то задаёт коэфтцент, в процессе поворачивающий ноги на противоположный градус
      anim.SetBool("runBack",true);
      //задаёт анимацию бега спиной
    }
    else
    {
      anim.SetBool("runBack",false);
      //в ином случае анимация не срабатывает
      rotationBack=1;
      //коэфицент изменения поворота не нужен в таком случае
    }
  }
  public void Rotation(int i,float b)
  //изменяет поворот ног на определённый градус
  {
    noga1.rotation = Quaternion.Euler(noga1.eulerAngles.x,i*b,noga1.eulerAngles.z);
    noga2.rotation = Quaternion.Euler(noga2.eulerAngles.x,i*b,noga2.eulerAngles.z);
  }
}
