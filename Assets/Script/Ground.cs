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
    new BoxCollider2D collider;

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
        if (player.isStart && player.isDead == false)
        {

            Vector2 pos = transform.position;

            // move ground to left
            pos = GroundMovement(pos);

            //lay vi tri cao nhat cua ground
            groundHeight = transform.position.y + (collider.size.y * transform.localScale.y / 2);
            //lay vi tri ben phải và trái của ground
            groundRight = transform.position.x + (collider.size.x * transform.localScale.x / 2);
            groundLeft = transform.position.x - (collider.size.x * transform.localScale.x / 2);

            if (groundRight < screenLeft)
            {
                Destroy(gameObject);
                return;
            }

            if (!didGenerateGround && groundRight < screenRight)
            {                               
                didGenerateGround = true;
                generateGround();             
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
        // Instantiate a new ground
        GameObject go = Instantiate(gameObject);

        // Khai báo đối tượng chứa vị trí cho ground mới tạo
        Vector2 pos;

        // Tạo viên ngọc (gem) trước khi cập nhật vị trí ground để chúng xuất hiện cùng lúc
        GameObject gem = null;
        if (gemPrefab != null)
        {
            gem = Instantiate(gemPrefab);
        }

        // Randomize the sprite of the generated ground
        int randomIndex = Random.Range(0, groundPrefabs.Length);

        // Copy the attributes of the ground prefab to the generated ground
        CopyAtributesGameObj(go, randomIndex);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();

        // Tính toán vị trí khởi tạo nền
        float gravity = Mathf.Abs(Physics2D.gravity.y * player.rb.gravityScale);
        float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        float minY = cameraBottom + goCollider.size.y * transform.localScale.y / 2 + 0.5f;
        float maxY = (player.jumpForce * player.jumpForce) / (2 * gravity);
        float padding = 2.0f;
        float lowestCameraView = Camera.main.transform.position.y - Camera.main.orthographicSize + padding;
        if (minY < lowestCameraView)
        {
            minY = lowestCameraView;
        }

        float actualY = Random.Range(minY, maxY) - goCollider.size.y * transform.localScale.y / 2 + 0.5f;

        if(player.currentSpeed <= 80)
        {
            if(randomIndex == 2)
            {
                pos.x = screenRight + Random.Range(5, 6);
            }
            else
            {
                pos.x = screenRight + Random.Range(10, 12);
            }
        }
        else
        {
            pos.x = screenRight + Random.Range(10, 14);
        }
        // Đặt vị trí của ground và gem ngay lập tức trước khi nó hiển thị trên camera
        pos.y = actualY;
        go.transform.position = pos;

        // Đặt vị trí của gem dựa trên vị trí của ground ngay sau khi ground được khởi tạo
        if (gem != null)
        {
            Vector2 gemPosition = go.transform.position;
            gemPosition.x = Random.Range(groundLeft + 0.2f, groundRight - 0.2f ); // Điều chỉnh tọa độ X của viên ngọc
            gemPosition.y = go.transform.position.y + (goCollider.size.y * go.transform.localScale.y / 2) + 0.5f; // Đặt vị trí y của gem lên trên ground
            gem.transform.position = gemPosition;

            Rigidbody2D gemRb = gem.GetComponent<Rigidbody2D>();
            if (gemRb != null)
            {
                gemRb.gravityScale = 0; // Đặt trọng lực của gem bằng 0 để ngăn nó rơi xuống
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
