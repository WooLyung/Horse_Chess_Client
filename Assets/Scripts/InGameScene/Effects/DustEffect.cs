using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    private float speed = 1;
    private float nowSpeed = 1;
    private float angle = 0;
    private float time = 0;

    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        nowSpeed = speed = Random.Range(0.8f, 1.2f);
        angle = Random.Range(0, 2 * Mathf.PI);
    }

    private void Update()
    {
        time += Time.deltaTime * 1.5f;

        if (time >= 1)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, (1 - time) * 0.25f);
            nowSpeed = speed * (1 - time);

            transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * nowSpeed * Time.deltaTime;
        }
	}
}
