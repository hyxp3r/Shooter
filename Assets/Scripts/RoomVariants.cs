using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    public GameObject key;
    public GameObject gun;
    [HideInInspector] public List<GameObject> rooms;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomSpawner());
        
    }
    IEnumerator RandomSpawner()
    {
        yield return new WaitForSeconds(5f);
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        int rand = Random.Range(0,rooms.Count - 2 );

        Instantiate(key, rooms[rand].transform.position, Quaternion.identity );
        Instantiate(gun, rooms[rand].transform.position, Quaternion.identity );

        lastRoom.door.SetActive(true);
        lastRoom.DestroyWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
