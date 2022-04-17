using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;
    [Header("Controls")]
    public float speed;
    public ControlType controlType;
    public Joystick joystick;
    public enum ControlType{PC, Android};
    [Header("Weapons")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    [Header("Health")]
    public float health;
    public Text healthDisplay;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator anim;
    private bool facingRight = true;
    [Header("Key")]
    public GameObject keyIcon;
    public GameObject wallEffect;

    private bool keyButtonPushed;


    // Start is called before the first frame update
    void Start()
    {
        if (controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);

        }
        anim = GetComponent<Animator>();
 
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
       if(controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
        }    
        else if(controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }    
        moveVelocity = moveInput.normalized * speed;

        if(moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
        if(!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();
        }
        if (health <=0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
        
    }
    
     void FixedUpdate() {
         rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        
    }
    private void Flip()
    {
      facingRight = !facingRight;
      Vector3 Scaler = transform.localScale;
      Scaler.x *= -1;
      transform.localScale = Scaler;  
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Potion")) 
        {
            ChangeHealth(5);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Shield"))
        {
            if(!shield.activeInHierarchy)
            {
            shield.SetActive(true);
            shieldTimer.gameObject.SetActive(true);
            shieldTimer.isCooldown = true;
            Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetTimer();
                Destroy(other.gameObject);
            }
        }
        else if(other.CompareTag("Weapon"))
        {
            for (int i = 0; i < allWeapons.Length; i++)
            {
                if(other.name == allWeapons[i].name)
                {
                    unlockedWeapons.Add(allWeapons[i]);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Key"))
        {
            keyIcon.SetActive(true);
            Destroy(other.gameObject);

        }
    }
    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;

    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Door") && keyButtonPushed && keyIcon.activeInHierarchy)
        {
            keyIcon.SetActive(false);
            other.gameObject.SetActive(false);
            keyButtonPushed = false;


        }
        
    }
    public void ChangeHealth(int healthValue)
    {
        if(!shield.activeInHierarchy || shield.activeInHierarchy && healthValue >0 )
        {
            health += healthValue; 
            healthDisplay.text = "HP: " + health;
        }
        else if(shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceTime(healthValue);

        }
    }
    public void SwitchWeapon()
    {
        for (int i = 0; i<unlockedWeapons.Count; i++)
        {
            if(unlockedWeapons[i].activeInHierarchy)
            {
                unlockedWeapons[i].SetActive(false);
                if(i != 0)
                {
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;                   

                }
                weaponIcon.SetNativeSize();
                break;
            }
        }

    }
}
