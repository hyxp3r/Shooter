using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class Enemy : MonoBehaviour
{
    public GameObject floaringDamage;
    private float timeBtwAttack;
    public float startTimeBtwAtack;
    public int health;
    
    public int damage;
    private float stopTime;
    public float startStopTime;
    public float speed;
    private Player player;
    private Animator anim;
    private AddRoom room;
    [HideInInspector] public bool playerNotInRoom;
    private bool stopped;
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y + 2.75f);
        Instantiate(floaringDamage, damagePos, Quaternion.identity);
        floaringDamage.GetComponentInChildren<FloaringDamage>().damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        room = GetComponentInParent<AddRoom>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerNotInRoom)
        {

        
        if(stopTime <= 0)
        {
            stopped = false;
        }
        else
        {
            stopped = true;
            stopTime -= Time.deltaTime;
        }
        }
        else 
        {
            stopped = true;
        }
        if(health <= 0)
        {
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }
        if(player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else 
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (!stopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        }
        
        
    }
     private void OnTriggerStay2D(Collider2D other) 
     {
        if (other.CompareTag("Player"))
        {
        if(timeBtwAttack <= 0)
        {
            anim.SetTrigger("enemyAtack");
            OnEnemyAttack();
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
        }
        
    }
    public void OnEnemyAttack()
    {
        player.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAtack;
    }
}
