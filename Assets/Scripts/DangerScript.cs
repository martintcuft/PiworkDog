using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScript : MonoBehaviour
{
	public byte dangerID = 0;//0 = kill block, 1 = cat
	public Collider2D collider;
	Rigidbody2D rb2D;
	public float moveSpeed = 1f;
	
	//sprite things
	public SpriteRenderer sprite;
	public byte spriteFrame = 0;
	[SerializeField] float animCounter = 0f;
	public bool otherAnim = false;
	public Sprite[] dangerSprites;
	
    void Start() {
		rb2D = GetComponent<Rigidbody2D>();
        switch(dangerID) {
			case 0: sprite.sprite = dangerSprites[0]; break;
			case 1: sprite.sprite = dangerSprites[1]; break;
		}
    }
	
    void Update() {
		switch(dangerID) {
			case 1:
				if(!otherAnim) rb2D.velocity = new Vector2((!sprite.flipX ? -moveSpeed : moveSpeed), rb2D.velocity.y);
			break;
		}
		AnimateDanger();
    }
	/*private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 3) {
			//*on hit player animation*
			otherAnim = true;
            collision.gameObject.GetComponent<PlayerScript>().HitAndRestartSequence();
        }
    }*/
	private void OnTriggerEnter2D(Collider2D collision) {
		//detect collision with despawn point
		if (collision.gameObject.name == "Cat_Exit") {
			//*despawn*
			Debug.Log("Catdestruction");
			Destroy(gameObject, 0.25f);
        }
	}
	
	public void JustHitPlayer() {
		otherAnim = true;
		animCounter = 0f;
		rb2D.velocity = Vector2.zero;
	}
	
	public void AnimateDanger() {
		float adv = Time.deltaTime*10f;
		switch(dangerID) {
			case 0: break;
			case 1: //cat animations
				if(!otherAnim) {
					animCounter += adv*0.75f;
					spriteFrame = (byte)(1+animCounter);
					if(spriteFrame >= 4) spriteFrame = 4;
					if(animCounter >= 4f) animCounter -= 4f;
				}
				else {
					animCounter += adv*0.5f;
					spriteFrame = (byte)(5+animCounter);
					if(spriteFrame >= 8) {
						spriteFrame = 8;
						animCounter = 0f;
						otherAnim = false;
					}
				}
			break;
		}
		if(spriteFrame > dangerSprites.Length-1) spriteFrame = (byte)(dangerSprites.Length-1);
		sprite.sprite = dangerSprites[spriteFrame];
	}
}
