using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiScript : MonoBehaviour
{
	public Sprite[] piSprites;
	public SpriteRenderer piSprite;
	float animCounter = 0f;
    void Update()
    {
		byte spriteFrame = 0;
        animCounter += Time.deltaTime;
		if(animCounter < 4f) {
			spriteFrame = (byte)animCounter;
		}
		else {
			animCounter -= 4f;
		}
		piSprite.sprite = piSprites[spriteFrame];
    }
}
//when the pi
