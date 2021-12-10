using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    public Rigidbody2D mainRigidbody2D;
    public Collider2D goalTrigger;

    public SpriteRenderer headSprite;
    public SpriteRenderer tailSprite;

    public Sprite normalHeadSprite;
    public Sprite normalTailSprite;

    public Sprite deadHeadSprite;
    public Sprite deadTailSprite;

    public Sprite smileHeadSprite;

    public void LookAlive() {
        headSprite.sprite = normalHeadSprite;
        tailSprite.sprite = normalTailSprite;
    }

    public void LookDead() {
        headSprite.sprite = deadHeadSprite;
        tailSprite.sprite = deadTailSprite;
    }

    public void Smile() {
        headSprite.sprite = smileHeadSprite;
        tailSprite.sprite = normalTailSprite;
    }
}
