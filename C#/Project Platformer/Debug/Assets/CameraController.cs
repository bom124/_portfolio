using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform player;
  private Vector3 pos;

  private void Awake()
  {
    if (!player)
        player = FindObjectOfType<PlayerController>().transform;
  }
  private void Update()
  {
    pos = player.position;
    pos.z=-20f;
    pos.y=pos.y+2f;
    transform.position = Vector3.Lerp(transform.position,pos,Time.deltaTime);
  }

}
