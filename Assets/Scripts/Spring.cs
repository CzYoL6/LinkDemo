using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float springForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("spring!") ;
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * springForce, ForceMode2D.Impulse);
            Destroy(gameObject);

        }
    }
}
