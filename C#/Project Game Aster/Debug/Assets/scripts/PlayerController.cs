using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
//класс отвечающий за управение игроком
//в соответсвии с ТЗ реализовал ускорение с минимальным сопротивоение воздуха и поворот с определённой скорость. Помойму при минимальном сопротивлении воздуха упралять пероснажем неудобно, но я делал всё в соответсвии с ТЗ
{
  private Transform transPlayer;
  //transform персонажа
  private float moveInputVertical;
  //переменная, отвечающая за коэфицент поврота
  public float aceleration;
  private float moveInputHorizontal;
  //переменная, отвечающая за ускорение на кнопку
  private  Vector3 mousePos;
  //  позиция мыши
  private Rigidbody2D rb;
  private SpriteRenderer sprite;
  private BoxCollider2D Collider2D;
  private bool imune;
  private const int imune_time=3;
  private float time;
  // четыре переменные,реализующие неуязвимость и мерцание
  public float maxSpeed;
  public float rotationSpeed;
  private double timeReload;
  public Transform shotPoint;
  //точка, откуда будут вылетать снаряды

  public bool keybordControl;
  //переменная, отвечающая за схему управления
  private AudioSource[] sounds;
  //список звуков


    public void Awake()
    {
      sounds = GetComponents<AudioSource>();
      sprite= GetComponentInChildren<SpriteRenderer>();
      rb = GetComponent<Rigidbody2D>();
      //для реализации инертности и реалистичност было решено использовать затратный метом с использованием сил
      Collider2D = GetComponent<BoxCollider2D>();
    }
    public void Start()
    {
      transPlayer = transform;
      //обозначаем transform для последующего кэширования
      Collider2D.enabled = false;
      //выключаем коллайдер для реализации неузвимости, потому что с объектом ничего не будет взаимодействовать, а он будет полностью дееспесобен
    }
    public void Update()
    {
      if(time<imune_time)
      //проверяем прошествие время неузвимости
      {
        time+=Time.deltaTime;
        //неуязвимость ещё идёт, значит прибавляем прошедшее время
        if(!imune)
        {
          sprite.enabled = false;
          imune=true;
        }
        else
        {
          sprite.enabled = true;
          imune=false;
          //реализуем мерцание. С каждым кадром спрайт будет включаться и выключаться
        }
      }
      else
      {
        sprite.enabled = true;
        Collider2D.enabled = true;
        //если время неузвимости прошло включаем коллайдер и спрайт игрока
      }
      //реализуем схемы управления
      if(keybordControl==false)
      //True означает схему управления клавиатура + мышь
      {
        //реализуем поворот пероснажа за мышкой
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)- transPlayer.position;
        //находим разницу положения мыши и персонажа
        float rotation =  transPlayer.eulerAngles.z+90;
        //определяем вращение персонажа в переменную, чтобы можно было её изменять и не взаимодействовать с поворотом персонажа
        if(rotation>180)
        {
          rotation-=360;
          //чтобы не было путницы с несколькими проходами по кругам Эйлера было решено всё округлить до одного
        }
        float diferentRotate = Mathf.Atan2(mousePos.y,mousePos.x)*Mathf.Rad2Deg - rotation;
        //находим необходимое вращение
        if(diferentRotate>-10&diferentRotate<10)
        {
          moveInputHorizontal*=0;
          //чтобы не было дёрганий реализуем мёртвую зону вращения, путём обнуления силы при малых значениях необходимого поворота
        }
        else if(diferentRotate<-181)
        {
          moveInputHorizontal=-1;
          //чтобы ращение шло по наименьшей дуге реализовал такой метод, вращающийй персонажа в другую сторону
        }
        else if(diferentRotate>181)
        {
          moveInputHorizontal=1;
            //чтобы ращение шло по наименьшей дуге реализовал такой метод, вращающийй персонажа в другую сторону
        }
        else if(diferentRotate>0)
        {
          moveInputHorizontal=-1;
          //метод, реализующий вращение против часовой стрелке
        }
        else if(diferentRotate<0)
        {
          moveInputHorizontal=1;
          //метод, реализующий вращение по часовой стрелке
        }
        else
        {
          moveInputHorizontal=0;
          //если персонаж повёрнут к мыши, то крутиться он не будет
        }
        if(Input.GetMouseButton(1)^Input.GetAxis("Vertical")>0)
        {
          if(sounds[1].isPlaying==false)
          {
            sounds[1].Play();
          }
          //проигрывание звука, только если он уже не проигрывается
          moveInputVertical+=aceleration;
          //ускорение на правую кнопку мыши, кнопку W иили стрелку вверх
        }
        else
        {
          moveInputVertical=0;
          //если не нажата кнопка ускорения,то скорость обнуляется
        }
        if(Input.GetMouseButtonDown(0)&timeReload<=0)
        {
          sounds[0].Play();
          //проигрывание звука выстрела
          GameObject bullet = ObjectPool.SharedInstance.GetPooledObjectBul();
          bullet.transform.position = shotPoint.transform.position;
          bullet.transform.rotation = shotPoint.transform.rotation;
          bullet.SetActive(true);
          //метод,вызывающий пули из object pool и заставляющий их вылетать из shot point. Проверку на отсутвие пуль не делал, потому что пуль в пуле 12, пуля пропадает за 2.5 секунды, а за 2.5 секунды с перезарядкой в 0.33 секунды максимум можно выстрелить 7.53... пули, значит пули не смогут закончиться
          timeReload = 0.33;
          //время перезарядки, реализующее 3 выстрела в секунду,в соответсвии с ТЗ
        }
      }
      else
      {
        //другая схема управения - клавиатура
        moveInputHorizontal=Input.GetAxis("Horizontal");
        //метод, меняющий коэфицент поворота в зависимости от нажатых клавих AD и боковыъ стрелочек
        if(Input.GetAxis("Vertical")>0)
        {
          if(sounds[1].isPlaying==false)
          {
            sounds[1].Play();
          }
          //проигрывание звука, только если он уже не проигрывается
          moveInputVertical+=aceleration;
          //ускорение при нажатой клавише W или стрелочке вверх
        }
        else
        {
          moveInputVertical=0;
          ////если не нажата кнопка ускорения,то скорость обнуляется
        }
      }
      timeReload-=Time.deltaTime;
      //уменьшение времени перезарядки
      if(Input.GetKeyDown(KeyCode.Space)&timeReload<=0)
      //выстрел на пробел есть в каждой из этих схем упраления, поэтому логично вызвать его вне этих двух методов
      {
        sounds[0].Play();
        //проигрывание звука выстрела
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObjectBul();
        bullet.transform.position = shotPoint.transform.position;
        bullet.transform.rotation = shotPoint.transform.rotation;
        bullet.SetActive(true);
        //метод,вызывающий пули из object pool и заставляющий их вылетать из shot point.
        timeReload = 0.33;
        //время перезарядки, реализующее 3 выстрела в секунду,в соответсвии с ТЗ
      }
    }
    public void FixedUpdate()
    {
      if(moveInputVertical>maxSpeed)
      {
        moveInputVertical=maxSpeed;
        //метод, реализующий максимальную скорость
      }
      rb.AddRelativeForce(new Vector2(0f,moveInputVertical));
      //метод, добавляющий силу к персонажу
      rb.AddTorque(moveInputHorizontal*rotationSpeed*-1f);
      //метод, добавляющий боковую силу, вследствие этого реализуется вращение
    }
    public void Death()
    {
      Destroy(gameObject);
      //смерть персонажа. Решил не запихивать его в object pool, потому что пероснаж появляется и умирает не так уж и часто
    }

}
