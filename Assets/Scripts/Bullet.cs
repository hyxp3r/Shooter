using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    [SerializeField]bool enemyBullet;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }

            if (hitInfo.collider.CompareTag("Player") && enemyBullet)
            {
                hitInfo.collider.GetComponent<Player>().ChangeHealth(-damage);
            }
        Destroy(gameObject);
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        
    Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position); 
    if (screenPosition.y > Screen.height || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.x < 0)  
    {
        Destroy(gameObject);
    }      
    }
    
}
