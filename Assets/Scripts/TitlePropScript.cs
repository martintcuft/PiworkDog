using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePropScript : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public float lifeTime = 5f;
    void Update() {
		transform.position = new Vector3(transform.position.x-Time.deltaTime*scrollSpeed, transform.position.y, transform.position.z);
        lifeTime -= Time.deltaTime;
		if(lifeTime <= 0f) Destroy(gameObject, 0f);
    }
}
