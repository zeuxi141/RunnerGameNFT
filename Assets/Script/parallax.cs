using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public float depth = 1;

    Player player;
    //PlayerScript player;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        //player = GameObject.Find("Player").GetComponent<PlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float realVelocity = player.currentSpeed / depth;
        //float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if(pos.x <= -20)
        {
            pos.x = 18;
        } 

        transform.position = pos;
    }
}
