using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiScript : MonoBehaviour
{
	public Sprite[] piSprites;
	public SpriteRenderer piSprite;
	float animCounter = 0f;
	byte spriteFrame = 0;
	
	[SerializeField] ParticleSystem pi_dust;
	
    void Update() {
		byte oldFrame = spriteFrame;
        animCounter += Time.deltaTime;
		if(animCounter < 4f) {
			spriteFrame = (byte)animCounter;
		}
		else {
			animCounter -= 4f;
		}
		
		piSprite.sprite = piSprites[spriteFrame];
		if(oldFrame != spriteFrame) pi_dust.Play();
    }
}
//when the pi
