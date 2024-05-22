using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform toFollow;//el cuerpo a seguir
	public Camera cam;
    void Start()
    {
        toFollow = GameObject.Find("Epi").transform;
    }
    void Update()
	{
        if(toFollow != null) {//si el cuerpo no es nulo
			//posición vertical respecto al jugador
			float ytrans = toFollow.position.y + 1.5f;
			//desde -1.0 hasta 3.5
			if(ytrans > 3.75f) ytrans = 3.75f;
			if(ytrans < -1f) ytrans = -1f;
			
			if(ytrans > -1.75f) {
				cam.orthographicSize = 5+((ytrans+1f)/9f);//desde 5.0 hasta 5.5
			}
			else cam.orthographicSize = 5.0f;
			
			transform.position = new Vector3(transform.position.x, transform.position.y + (ytrans-transform.position.y)*Time.deltaTime, transform.position.z);
		}
    }
}
