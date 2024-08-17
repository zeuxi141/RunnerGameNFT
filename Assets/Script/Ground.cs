using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;
    public GameObject gemPrefab;
    public GameObject[] groundPrefabs;


    public float depth = 8;
    public float groundHeight;
    public float groundRight;
    public float groundLeft;
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
        if (player.isStart)
        {

            Vector2 pos = transform.position;

            // move ground to left
            pos = GroundMovement(pos);

            groundHeight = transform.position.y + (collider.size.y * transform.localScale.y / 2);
            groundRight = transform.position.x + (collider.size.x * transform.localScale.x / 2);
            groundLeft = transform.position.x - (collider.size.x * transform.localScale.x / 2);

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
    }

    private Vector2 GroundMovement(Vector2 pos)
    {
        float realVelocity = player.currentSpeed / depth;
        pos.x -= realVelocity * Time.fixedDeltaTime;
        return pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        Vector2 pos;
        BoxCollider2D goCollider;

        // Randomize the sprite of the generated ground
        int randomIndex = Random.Range(0, groundPrefabs.Length);

        CopyAtributesGameObj(go, randomIndex);
        goCollider = go.GetComponent<BoxCollider2D>();


        // caculate ground height
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y * transform.localScale.y / 2);

        // caculate min max
        float gravity = Mathf.Abs(Physics2D.gravity.y * player.rb.gravityScale);
        float maxY = (player.jumpForce * player.jumpForce) / (2 * gravity);
        float minY = goGround.groundHeight - 10;

        float lowestCameraView = Camera.main.transform.position.y - Camera.main.orthographicSize;

        if (minY < lowestCameraView)
        {
            minY = lowestCameraView;
        }

        float actualY = Random.Range(minY, maxY) - goCollider.size.y * transform.localScale.y / 2 + 0.5f;

        pos.x = screenRight + Random.Range(10, 14);
        pos.y = actualY;
        go.transform.position = pos;

        // generate gem
        if (gemPrefab != null)
        {
            GameObject gem = Instantiate(gemPrefab);
            gem.transform.position = go.transform.position;
            Vector2 gemPosition = transform.position;
            gemPosition.x = Random.Range(groundLeft, groundRight);
            gemPosition.y = (groundHeight + Random.Range(1, maxY));
            gem.transform.position = gemPosition;

            Rigidbody2D gemRb = gem.GetComponent<Rigidbody2D>();
            if (gemRb != null)
            {
                gemRb.gravityScale = 0;
            }
        }
    }

    private void CopyAtributesGameObj(GameObject go, int randomIndex)
    {
        go.GetComponent<SpriteRenderer>().sprite = groundPrefabs[randomIndex].GetComponent<SpriteRenderer>().sprite;
        go.GetComponent<SpriteRenderer>().color = groundPrefabs[randomIndex].GetComponent<SpriteRenderer>().color;
        go.transform.localScale = groundPrefabs[randomIndex].transform.localScale;
        go.GetComponent<BoxCollider2D>().size = groundPrefabs[randomIndex].GetComponent<BoxCollider2D>().size;
        go.GetComponent<BoxCollider2D>().offset = groundPrefabs[randomIndex].GetComponent<BoxCollider2D>().offset;
    }
}
