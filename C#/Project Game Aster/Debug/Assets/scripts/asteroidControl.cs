using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class asteroidControl : MonoBehaviour
//класс-контроллер
{
  public int countAs;
  //число оставшихся МАЛЕНЬКИХ астероидов
  private int maxCountAs;
  //максимальное количество больших астероидов в этой волне. После уничтожения всех астероидов увеличивается на 1
  private float timeBtwAst;
  //время между волнами астероидов после уничтожения другой волны
  public int score;
  //счёт, отображаемый на экране игрока
  public Text scoreDisplay;
  //текст счёта отображаемый на экране
  public Text controlDisplay;
  //текст, отображающий текущую схему управления в меню и в паузе
  private int health;
  //оставшееся здоровье
  public Image health1;
  //картинка первого контейнера здоровья
  public Image health2;
  //картинка второго контейнера здоровья
  public Image health3;
  //картинка третьего контейнера здоровья
  public Image backgroundMenu;
  //фон, отображающийся только при запуске игры
  public GameObject player;
  public GameObject NLO;
  public bool die;
  //переменная, отображабщая смерть игрока
  private PlayerController PlayerController;
  //класс-контроллер игрока
  public const int timeRespawnNLO_const = 30;
  public float timeRespawnNLO;
  //время респауна НЛО
  public bool NLOdie;
  //переменная, отображабщая смерть НЛО
  public float x_NLO_spawn;
  float y_NLO_spawn;
  const float border_fly_NLO=0.8f;
  public const float size_rate = 1;
  private bool keybordControl;
  //переменная, отображабщая выбранную схему управления
  private static bool GameIsPaused = false;
  //переменная, отображабщая состояние паузы
  public GameObject PauseMenuUi;
  //панель паузы
  private bool gameEnabled;
  //переменная, отображабщая начало игры
  private AudioSource[] explosion;
  //массив со звуками
  public void Start()
  {
    timeRespawnNLO = timeRespawnNLO_const;
    keybordControl = true;
    die=true;
    Time.timeScale = 0;
    //обозначаем стартовые характеристики
    explosion = GetComponents<AudioSource>();
  }
  public void Update()
  {
    if(Input.GetKeyDown(KeyCode.Escape))
    {
      if(GameIsPaused)
      {
        Resume();
      }
      else
      {
        PausedMenu();
      }
      //метод,вызывающий паузу
    }
    if(timeRespawnNLO<=0&NLOdie==true)
    {
      x_NLO_spawn= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + size_rate;
      y_NLO_spawn = (Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y) * Random.Range(-border_fly_NLO, border_fly_NLO);
      if (Random.Range(-1,1)<0)
      {
        Instantiate(NLO, new Vector2(x_NLO_spawn, y_NLO_spawn), transform.rotation);
      }
      else
      {
        Instantiate(NLO, new Vector2(-x_NLO_spawn, y_NLO_spawn), transform.rotation);
      }
      NLOdie=false;
      //метод, создающий НЛО в одной из двух сторон и на случайной высоте, не ближе, чем на 20% к границам экрана
    }
    else
    {
      //в противном случае уменьшает время до создание НЛО
      timeRespawnNLO-=Time.deltaTime;
    }
    if(health!=0 & die==true & Input.GetAxis("Jump")!=0)
    {
      die=false;
      Instantiate(player, transform.position, transform.rotation);
      //метод воскрешения персонажа при смерти и оставщихся контейнерах здоровья
      PlayerController = FindObjectOfType<PlayerController>();
      PlayerController.keybordControl = keybordControl;
      //персонажу передаётся текущая схема управления
    }
    scoreDisplay.text = score.ToString();
    //обновление счёта
    if(countAs==0)
    //если все маленькие астероиды уничтожены
    {
      if(timeBtwAst<=0)
      //если время передышки(2секунды по ТЗ) прошло
      {
        maxCountAs+=1;
        //максимальное количество асстероидов увеличивается на 1
        countAs=maxCountAs*4;
        //задаётся следующее необходимое число уничтожения маленьких астероидов
        for(int i=0;i<maxCountAs;i++)
        {
          GameObject asteroid = ObjectPool.SharedInstance.GetPooledObjectAs();
          if(asteroid!=null)
          {
            asteroid.transform.position = new Vector3(Random.Range(-10,10),-6,transform.position.z);
            //выбрал оптимальные х для спауна астероидов
            asteroid.transform.rotation = Quaternion.Euler(0,0,Random.Range(40,250)*Random.Range(-1,1));
            //задаётся случайный поворот, но такой, чтобы астероид не оказался не в зоне досягаемости
            //умножение на коэфицент просходит для создание астероидов, летащих  в разные направлениях 
            asteroid.SetActive(true);
            //создаётся соответсвующее количество больших астероидов
            timeBtwAst = 2;
            //время передышки возвращается к прежним значениям
          }
        }
      }
      else
      {
        timeBtwAst-=Time.deltaTime;
        //таймер передышки уменьшается
      }
    }
  }
  public void Death()
  //метод, срабатывающий при смерти игрока
  {
    PlayerController = FindObjectOfType<PlayerController>();
    PlayerController.Death();
    die=true;
    health-=1;
    switch (health)
    {
      case 2:
        health1.enabled = false;
        break;
       case 1:
        health2.enabled = false;
        break;
       case 0:
        health3.enabled = false;
        break;
      default:
        break;
    }
  }
  public void Resume()
  //метод возобновления игры во время паузы
  {
    if(gameEnabled==true)
    {
      PauseMenuUi.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused =false;
    }
  }
  public void PausedMenu()
  //метод паузы
  {
    PauseMenuUi.SetActive(true);
    Time.timeScale = 0;
    GameIsPaused =true;
  }
  public void Control()
  //метод, меняющий схему управления
  {
    if(keybordControl==false)
    {
      controlDisplay.text = "Управление: клавиатура";
      //смена текста в меню паузы
      keybordControl = true;
    }
    else
    {
      controlDisplay.text = "Управление: клавиатура+мышь";
      //смена текста в меню паузы
      keybordControl = false;
    }
    if(die==false)
    {
      PlayerController = FindObjectOfType<PlayerController>();
      PlayerController.keybordControl = keybordControl;
      //если персонажжив, то ему автоматически передаётся новая схема управления
    }
  }
  public void soundExplosion(int i)
  //метод вызывающий звуки уничтожения астероидов. Решил вызывать звуки отсюда, потому что астероиды после уничтожения уходят в ин-актив
  {
     explosion[i].Play();
  }
  public void NewGame()
  //метод начала новой игры
  {
    gameEnabled=true;
    Time.timeScale = 1f;
    backgroundMenu.enabled = false;
    //выключет стартовую картинку меню
    PauseMenuUi.SetActive(false);
    score=0;
    //обнуляет счёт
    GameObject[] bulletsNLO =  GameObject.FindGameObjectsWithTag("BulletEnemy");
    for(int i=0;i<bulletsNLO.Length;i++)
    {
      bulletsNLO[i].SetActive(false);
    }
    //выключет все пули НЛО, если игра уже запущена
    GameObject[] bullets =  GameObject.FindGameObjectsWithTag("BulletPlayer");
    for(int i=0;i<bullets.Length;i++)
    {
      bullets[i].SetActive(false);
    }
    //выключет все пули игрока, если игра уже запущена
    GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Assteroid");
    for(int i=0;i<asteroids.Length;i++)
    {
      asteroids[i].SetActive(false);
    }
    //выключет все астероиды, если игра уже запущена
    if(!NLOdie)
    {
      GameObject.FindGameObjectWithTag("NLO").GetComponent<NLO>().Death();
    }
    //уничтожает НЛО, если игра запущена и они живы
    timeRespawnNLO = timeRespawnNLO;
    //задаёт новое время респауна
    countAs = 0;
    maxCountAs=1;
    //обнуляет максимальное кол-во астероидов
    health=3;
    scoreDisplay.enabled = true;
    health1.enabled = true;
    health2.enabled = true;
    health3.enabled = true;
    //включает все контейнеры здоровья
    if(die==false)
    {
      GameObject playerr = GameObject.FindGameObjectWithTag("player");
      Destroy(playerr);
    }
    //если игра запущена и игрок жив - уничтожает игрока
    die=false;
    Instantiate(player, transform.position, transform.rotation);
    PlayerController = FindObjectOfType<PlayerController>();
    PlayerController.keybordControl = keybordControl;
    //создаёт игрока и задаёт ему выбранную схему управления
  }
  public void Exit()
  //метод, закрывающий игру
  {
    Application.Quit();
  }
}
