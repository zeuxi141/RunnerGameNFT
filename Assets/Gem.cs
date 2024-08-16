using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Player player;
    public float depth = 8;
    private int maxGem = 3;
    public float gemRight;
    public float screenRight;
    public float screenLeft;

    BoxCollider2D collider;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane)).x;
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        gemRight = transform.position.x + (collider.size.x * transform.localScale.x / 2);

        // move gem
        pos = GemMovement(pos);

        if (gemRight < screenLeft)
        {
            Destroy(gameObject);
            return;
        }

        //if (!didGenerateGround)
        //{
        //    if (groundRight < screenRight)
        //    {
        //        didGenerateGround = true;
        //        generateGround();
        //    }
        //}

        transform.position = pos;
    }

    private Vector2 GemMovement(Vector2 pos)
    {
        float realVelocity = player.currentSpeed / depth;
        pos.x -= realVelocity * Time.fixedDeltaTime;
        return pos;
    }
}
