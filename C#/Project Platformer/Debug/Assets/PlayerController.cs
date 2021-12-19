using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float speed_start;
    public float jumpForce;
    public float moveInput;
    public float health;

    public float key;
    public float countjump;
    public float maxcountjump;

    public GameObject healthEffect;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Image bar;
    public GameObject endGame;
    private SpriteRenderer endGameRen;

    public GameObject bossbar;
    private Image bossbarRen;
    public GameObject bossbarback;
    private Image bossbarRenback;

    public GameObject TooEndGame;
    private SpriteRenderer TooEndGameRen;

    public float ShoozeZone;
    public GameObject ulia;
    public GameObject andrey;
    public GameObject sonia;
    private SpriteRenderer uliaRen;
    private SpriteRenderer andreyRen;
    private SpriteRenderer soniaRen;


    private Rigidbody2D rb;
    private Animator anim;

    public bool facintRight = true;

    private void Awake()
    {
      bossbarRenback = bossbarback.GetComponent<Image>();
      bossbarRen = bossbar.GetComponent<Image>();
      TooEndGameRen = TooEndGame.GetComponent<SpriteRenderer>();
      endGameRen = endGame.GetComponent<SpriteRenderer>();
      uliaRen = ulia.GetComponent<SpriteRenderer>();
      andreyRen = andrey.GetComponent<SpriteRenderer>();
      soniaRen = sonia.GetComponent<SpriteRenderer>();
      bossbarRenback.enabled = false;
      bossbarRen.enabled =false;
      TooEndGameRen.enabled = false;
      endGameRen.enabled = false;
      andreyRen.enabled = true;
      soniaRen.enabled = false;
      uliaRen.enabled = false;
    }
    private void Start()
    {
      anim = GetComponent<Animator>();
      rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      bar.fillAmount = health*0.01f;
      moveInput= Input.GetAxis("Horizontal");
      rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
      if(facintRight ==false && moveInput >0)
      {
        Flip();
      }
      else if(facintRight==true && moveInput <0)
      {
        Flip();
      }
      if(moveInput == 0)
      {
         anim.SetBool("isRunning",false);
      }
      else
      {
         anim.SetBool("isRunning",true);
      }
      if(Input.GetMouseButton(0) && moveInput==0)
      {
        anim.SetBool("isShooting",true);
      }
      else
      {
        anim.SetBool("isShooting",false);
      }
    }
    private void Update()
    {
      if(health<=0 && health>-10000)
      {
        endGameRen.enabled = true;
        transform.position = new Vector3(6,100,0);
        health=-10000;
      }
      if(Input.GetKeyDown(KeyCode.R))
      {
        RestartLevel();
      }
      if(ShoozeZone==1)
      {
        Shooze();
      }
      isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius,whatIsGround);
      if (isGrounded == true)
      {
        countjump=maxcountjump;
      }
      if(countjump>0 && Input.GetButtonDown("Jump"))
      {
        countjump-=1;
        rb.velocity = Vector2.up * jumpForce;
      }
    }
  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("bossZone"))
    {
      bossbarRenback.enabled = true;
      bossbarRen.enabled =true;
    }
    if(other.CompareTag("End"))
    {
      TooEndGameRen.enabled = true;
      speed=0;
    }
    if(other.CompareTag("ShoozeZone"))
    {
      ShoozeZone=1;
    }
    if(other.CompareTag("ShoozeZoneEnd"))
    {
      ShoozeZone=0;
    }
    if(other.CompareTag("health"))
    {
      health+=10;
      Instantiate(healthEffect,other.transform.position,Quaternion.identity);
      Destroy(other.gameObject);
    }
    if(other.CompareTag("shipi"))
    {
      health-=10;
    }
    if(other.CompareTag("key"))
    {
      key=1;
      Destroy(other.gameObject);
    }
    if(other.CompareTag("botle"))
    {
      maxcountjump=2;
      Destroy(other.gameObject);

    }
    if(other.CompareTag("keyWall")&&key==1)
    {
      Destroy(other.gameObject);
    }
  }
  public void RestartLevel()
  {
    SceneManager.LoadScene("SampleScene");
  }
  void Shooze()
  {
    if(Input.GetKeyDown(KeyCode.Alpha1))
    {
      andreyRen.enabled = false;
      soniaRen.enabled = false;
      uliaRen.enabled = true;
    }
    if(Input.GetKeyDown(KeyCode.Alpha2))
    {
      andreyRen.enabled = true;
      soniaRen.enabled = false;
      uliaRen.enabled = false;
    }
    if(Input.GetKeyDown(KeyCode.Alpha3))
    {
      andreyRen.enabled = false;
      soniaRen.enabled = true;
      uliaRen.enabled = false;
    }
  }
  private void Flip()
    {
      facintRight = !facintRight;
      Vector3 Scaler = transform.localScale;
      Scaler.x *=-1;
      transform.localScale = Scaler;
    }

}
