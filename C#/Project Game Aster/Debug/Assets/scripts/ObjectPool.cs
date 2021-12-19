using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
//object pool с пулями игрока, астероидами и пулями НЛО
{
  public static ObjectPool SharedInstance;
  private  List<GameObject> pooledObjects;
  public  GameObject objectToPoolBullet;
  //помещаемые пули игрока
  private int  amountToPoolBullet;
  //количество помещаемых пуль
  private int  amountToPoolAsteroid;
  //количество помещаемых больших астероидов
  public  GameObject objectToPoolAsteroid;
  //помещаемые больште астероиды
  public  GameObject objectToPoolAsteroidMedium;
  //помещаемые средние астероиды
  private int  amountToPoolAsteroidMedium;
  //количество помещаемых средних астероидов
  public  GameObject objectToPoolAsteroidSmall;
  //помещаемые маленькие астероиды
  private int  amountToPoolAsteroidSmall;
  //количество помещаемых маленьких астероидов
  public  GameObject objectToPoolBulletNLO;
  //помещаемые пули НЛО
  private int  amountBulletNLO;
  //количество помещаемых пуль НЛО

  void Awake()
  {
    amountBulletNLO = 10;
    amountToPoolBullet = 12;
    amountToPoolAsteroid = 10;
    //такое количество посчитал подходящим. Больше 10 астероидов больших врятли удастся получить какому-нибудь игроку
    amountToPoolAsteroidMedium = amountToPoolAsteroid*2;
    //из большого атсероида выпадает два средних, из этого следует, что средних астероидов  в два раза больше
    amountToPoolAsteroidSmall = amountToPoolAsteroidMedium*2;
    //из среднего атсероида выпадает два маленьких, из этого следует, что маленьких астероидов  в два раза больше
    SharedInstance = this;
  }

  void Start()
  {
    pooledObjects = new List<GameObject>();
    //задаём pool
    GameObject tmp;
    //задаём помещаемый внутрь pool объект
    for(int i =0;i<amountToPoolBullet;i++)
    //количество раз соответсвует количеству объектов, необходимых нам
    {
      tmp = Instantiate(objectToPoolBullet);
      tmp.SetActive(false);
      pooledObjects.Add(tmp);
    }
    for(int i =0;i<amountToPoolAsteroid;i++)
    {
      tmp = Instantiate(objectToPoolAsteroid);
      tmp.SetActive(false);
      pooledObjects.Add(tmp);
    }
    for(int i =0;i<amountToPoolAsteroidMedium;i++)
    {
      tmp = Instantiate(objectToPoolAsteroidMedium);
      tmp.SetActive(false);
      pooledObjects.Add(tmp);
    }
    for(int i =0;i<amountToPoolAsteroidSmall;i++)
    {
      tmp = Instantiate(objectToPoolAsteroidSmall);
      tmp.SetActive(false);
      pooledObjects.Add(tmp);
    }
    for(int i =0;i<amountBulletNLO;i++)
    {
      tmp = Instantiate(objectToPoolBulletNLO);
      tmp.SetActive(false);
      pooledObjects.Add(tmp);
    }
  }
  //далее идут методы получания обектов для последующего вызова в сторонних классах 
  public GameObject GetPooledObjectBul(){
    for(int i=0;i<amountToPoolBullet;i++)
    {
      if(!pooledObjects[i].activeInHierarchy)
      {
        return pooledObjects[i];
      }
    }
    return null;
  }
  public GameObject GetPooledObjectAs(){
    for(int i=amountToPoolBullet;i<amountToPoolAsteroid+amountToPoolBullet;i++)
    {
      if(!pooledObjects[i].activeInHierarchy)
      {
        return pooledObjects[i];
      }
    }
    return null;
  }
  public GameObject GetPooledObjectAsMedium(){
    for(int i=amountToPoolAsteroid+amountToPoolBullet;i<amountToPoolAsteroid+amountToPoolBullet+amountToPoolAsteroidMedium;i++)
    {
      if(!pooledObjects[i].activeInHierarchy)
      {
        return pooledObjects[i];
      }
    }
    return null;
  }
  public GameObject GetPooledObjectAsSmall(){
    for(int i=amountToPoolAsteroid+amountToPoolBullet+amountToPoolAsteroidMedium;i<amountToPoolAsteroid+amountToPoolBullet+amountToPoolAsteroidMedium+amountToPoolAsteroidSmall;i++)
    {
      if(!pooledObjects[i].activeInHierarchy)
      {
        return pooledObjects[i];
      }
    }
    return null;
  }
  public GameObject GetPooledObjectBulletNLO(){
    for(int i=amountToPoolAsteroid+amountToPoolBullet+amountToPoolAsteroidMedium+amountToPoolAsteroidSmall;i<amountToPoolAsteroid+amountToPoolBullet+amountToPoolAsteroidMedium+amountToPoolAsteroidSmall+amountBulletNLO;i++)
    {
      if(!pooledObjects[i].activeInHierarchy)
      {
        return pooledObjects[i];
      }
    }
    return null;
  }
}
