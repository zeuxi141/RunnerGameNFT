using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float depth = 8;
    public float groundHeight;
    public float groundRight;
    public float screenRight;
    public float screenLeft;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y * transform.localScale.y / 2);
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane)).x;
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
    }
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

        float realVelocity = player.currentSpeed / depth;
        pos.x -= realVelocity * Time.fixedDeltaTime;

        groundRight = transform.position.x + (collider.size.x * transform.localScale.x / 2);

        if (groundRight < screenLeft)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y * transform.localScale.y / 2);

        float gravity = Mathf.Abs(Physics2D.gravity.y * player.rb.gravityScale);
        float maxY = (player.jumpForce * player.jumpForce) / (2 * gravity);
        Debug.Log("Max Jump height: " + maxY);
        float minY = goGround.groundHeight - 10;

        float lowestCameraView = Camera.main.transform.position.y - Camera.main.orthographicSize;
        Debug.Log("LowestCamera: " + lowestCameraView);
        if (minY < lowestCameraView)
        {
            minY = lowestCameraView;
        }
        Debug.Log("Min Jump height: " + minY);
        float actualY = Random.Range(minY, maxY) - goCollider.size.y / 2;

        pos.x = screenRight + Random.Range(10, 15);
        pos.y = actualY;
        go.transform.position = pos;

    }
}