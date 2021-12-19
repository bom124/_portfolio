using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHero : MonoBehaviour
{
  private SpriteRenderer spriteR;

  private void Awake()
  {
    spriteR=GetComponent<SpriteRenderer>();
  }
    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Alpha1))
      {
        spriteR.enabled = false;
      }

    }
}
