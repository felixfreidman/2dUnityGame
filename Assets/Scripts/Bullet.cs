using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; } }
    public float speed = 10.0F;
    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }
    public Color Color {
        set { spriteRenderer.color = value;}
    }
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
     private void Start() {
        Destroy(gameObject, 1.4F);
    }
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Unit unit = collider.GetComponent<Unit>();
        if(unit && unit.gameObject != parent) {
            unit.ReceivedDamage();
            Destroy(gameObject);
        }
    }
   

}
