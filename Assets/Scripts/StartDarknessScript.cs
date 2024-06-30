using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDarknessScript : MonoBehaviour
{
    public float biggenToScale = 4f;
    public float biggening = 0.01f;
    void Update() {
        Invoke("BigUntilOffscreen", 0.5f);
    }
	private void BigUntilOffscreen() {
		transform.localScale += Vector3.one * biggening * Time.deltaTime;
		if(transform.localScale.y >= biggenToScale) Destroy(gameObject, 0f);
	}
}
