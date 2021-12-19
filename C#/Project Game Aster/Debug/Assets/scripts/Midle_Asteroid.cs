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
    //���� �������� ������������ � ����� ������, �������� �������������, ����� �������� ����
    {
      DestroyAsteroid(count_small_asteroids_lose * 0, reward);
      speedAsteroidPosle = Random.Range(speedAsteroidGranicaNiz, speedAsteroidGranicaVerh);
      //������� ��������� ��������
      GameObject asteroid = ObjectPool.SharedInstance.GetPooledObjectAsSmall();
      //��������� ����������� ���������
      asteroid.transform.position = transform.position;
      asteroid.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 45);
      //�������� �������� �������������� �� 45 �������� � ������ �������
      asteroid.GetComponent<asteroid>().speed = speedAsteroidPosle;
      //������� ��������� �������� ��������� ��������
      asteroid.SetActive(true);
      asteroid = ObjectPool.SharedInstance.GetPooledObjectAsSmall();
      //��������� ����������� ���������
      asteroid.transform.position = transform.position;
      asteroid.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 45);
      //�������� �������� �������������� �� 45 �������� � ������ �������
      asteroid.GetComponent<asteroid>().speed = speedAsteroidPosle;
      //������� ��� �� ��������� �������� ��������� ��������
      asteroid.SetActive(true);
      //����������� �����
      
    }
  }
}
