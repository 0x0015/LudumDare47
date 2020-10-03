using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject up;
    public GameObject left;
    public GameObject right;
    public GameObject down;
    public Vector2 velocity;
    public bool onground;
    public bool collideFromLeft;
    public bool collideFromTop;
    public bool collideFromRight;
    public bool collideFromBottom;
    public int jumptimer = 0;
    public float hit = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (hit <= 0)
        {
            
            onground = collideFromBottom;

            velocity.x = 0;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity.x += 8;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity.x -= 8;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {

            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (onground || jumptimer < 10)
                {
                    velocity.y += 3.1f;
                    jumptimer++;
                    onground = false;
                }
            }
            else
            {
                if (onground == true)
                {
                    jumptimer = 0;
                }
                else
                {
                    jumptimer = 99;
                }
            }
            if(onground == true)
            {
                jumptimer = 0;
                velocity.y = 0;
            }
            if (!onground)
            {
                velocity.y -= 1;
            }
            else
            {
                velocity.y = 0;
            }
            if (collideFromTop && velocity.y > 0)
            {
                velocity.y = 0;
                jumptimer = 99;
            }
            //transform.Translate(new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime);
            if (velocity.y < -20)
                velocity.y = -20;
            if (velocity.y > 20)
                velocity.y = 20;
            rigidBody.velocity = velocity;
        }
        else
        {
            rigidBody.velocity = velocity;
            hit -= Time.deltaTime * 60;
            if(collideFromTop)
            {
                hit = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        collideFromTop = false;
        collideFromLeft = false;
        collideFromRight = false;
        collideFromBottom = false;
        //print("collided");
        int contactCount = collision.GetContactCount();
        for(int i = 0; i < contactCount; i++)
        {
            var side = collision.GetContactSide(i);
            if (side == Collision2DSideType.Bottom && up.GetComponent<DetectBoxCollider>().collided)
                collideFromTop = true;
            if(side == Collision2DSideType.Top && down.GetComponent<DetectBoxCollider>().collided)
            collideFromBottom = true;
            if(side == Collision2DSideType.Right && left.GetComponent<DetectBoxCollider>().collided)
            collideFromLeft = true;
            if(side == Collision2DSideType.Left && right.GetComponent<DetectBoxCollider>().collided)
            collideFromRight = true;
        }
        if (collideFromBottom)
        {
            jumptimer = 0;
            velocity.y = 0;
        }

    }

    void OnCollisionExit2D(Collision2D collider)
    {
        //print("exit");
        collideFromTop = false;
        collideFromLeft = false;
        collideFromRight = false;
        collideFromBottom = false;
    }

}
