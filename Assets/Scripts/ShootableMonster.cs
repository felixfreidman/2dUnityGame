using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    private Bullet bullet;
    public float rate = 2.0F;
    public Color bulletColor = Color.white;
    protected override void Awake() {
        bullet = Resources.Load<Bullet>("Bullet");
    }
    protected override void Start() {
        InvokeRepeating("Shoot", rate, rate);
    }
    private void Shoot() {
        Vector3 position = transform.position;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        Unit unit = collider.GetComponent<Unit>();
        if(unit && unit is Character) {
            if(Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3F) ReceivedDamage();
            else unit.ReceivedDamage();
        }

    }
}
