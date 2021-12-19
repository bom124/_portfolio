using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunChoose : MonoBehaviour
{
  public GameObject alternativa;

  void Update()
  {
    if(alternativa.GetComponent<Gun>().getShoot==1)
    {
      Destroy(gameObject);
    }
  }
}
