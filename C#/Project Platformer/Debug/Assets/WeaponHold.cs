using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHold : MonoBehaviour
{
  public bool hold;
  public float distance;
  public GameObject bullet;
  public Transform shotPoint;
  public Transform holdPoint;
  public Transform holdPoint_sword;
  public Transform otkuda;
  public float throwObject = 5;
  RaycastHit2D hit;
  public bool facintRight = true;
  private float moveInput;
    private Animator anim;

  void Start()
  {
    anim = GetComponent<Animator>();
  }
  void Update()
  {
    moveInput= Input.GetAxis("Horizontal");
    if(Input.GetKeyDown(KeyCode.F))
    {
      if(!hold)
      {
        Physics2D.queriesStartInColliders = false;
        hit = Physics2D.Raycast(otkuda.position, Vector2.right * otkuda.localScale.x, distance);
        if(hit.collider!=null && hit.collider.tag == "gun")
        {
          hit.collider.gameObject.GetComponent<Gun>().getShoot = 1;
          if(hit.collider.gameObject.GetComponent<Gun>().basketbol==1)
          {
            anim.SetTrigger("bask");
          }
          if(hit.collider.gameObject.GetComponent<Gun>().drobovik==1)
          {
            anim.SetTrigger("drobovik");
          }
          if(hit.collider.gameObject.GetComponent<Gun>().sword==1)
          {
            anim.SetTrigger("sword");
          }
          hold = true;
        }
      }
      else
      {
        hold = false;
        anim.SetTrigger("drop");
        if(hit.collider.gameObject.GetComponent<Rigidbody2D>()!=null)
        {
          hit.collider.gameObject.GetComponent<Gun>().getShoot = 0;
          if(facintRight==false)
          {
            facintRight = !facintRight;
            Vector3 Scaler = hit.collider.gameObject.transform.localScale;
            Scaler.x *=-1;
            hit.collider.gameObject.transform.localScale = Scaler;
          }
          hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x,1) * throwObject;
        }
      }
    }

    if(hold)
    {
      if(hit.collider.gameObject.GetComponent<Gun>().sword==1)
      {
        hit.collider.gameObject.transform.position = holdPoint_sword.position;
      }
      if(hit.collider.gameObject.GetComponent<Gun>().sword==0)
      {
        hit.collider.gameObject.transform.position = holdPoint.position;
      }
      if(facintRight ==false && moveInput >0)
      {
        facintRight = !facintRight;
        Vector3 Scaler = hit.collider.gameObject.transform.localScale;
        Scaler.x *=-1;
        hit.collider.gameObject.transform.localScale = Scaler;
      }
      else if(facintRight==true && moveInput <0)
      {
        facintRight = !facintRight;
        Vector3 Scaler = hit.collider.gameObject.transform.localScale;
        Scaler.x *=-1;
        hit.collider.gameObject.transform.localScale = Scaler;
      }
    }
    Physics2D.queriesStartInColliders = true;
  }
  void ShootBas()
  {
    Instantiate(bullet, shotPoint.position, shotPoint.rotation);
  }
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(otkuda.position, otkuda.position + Vector3.right * otkuda.localScale.x * distance);
  }

}
