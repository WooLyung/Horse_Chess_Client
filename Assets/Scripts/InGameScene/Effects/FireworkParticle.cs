using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkParticle : MonoBehaviour
{
    private Vector3 rotSpeed;
    private float time = 0;

    public bool isLeft = true;
    public Rigidbody2D rigid;

    private void Start()
    {
        float angle = 0;
        rotSpeed = new Vector3(Random.Range(Mathf.PI, 2 * Mathf.PI), Random.Range(Mathf.PI, 2 * Mathf.PI), Random.Range(Mathf.PI, 2 * Mathf.PI));

        if (isLeft)
            angle = Random.Range(Mathf.PI / 6, Mathf.PI / 3);
        else
            angle = Random.Range(Mathf.PI * 2 / 3, Mathf.PI * 5 / 6);

        rigid.AddForce(new Vector2(Mathf.Cos(angle) * Random.Range(1, 8), Mathf.Sin(angle) * Random.Range(10, 14)));
    }

    private void Update()
    {
        time += Time.deltaTime;
        transform.Rotate(rotSpeed * Time.deltaTime * 3);
    }
}