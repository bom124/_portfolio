using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
  public Transform player;

  void Update()
  {
    transform.position = new Vector3(player.position.x+5f,transform.position.y,player.position.z+5f);
    //камера следует за игроком с отдалением по оси х и по оси z
  }
}
