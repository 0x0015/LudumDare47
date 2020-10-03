using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
    public bool hit = false;

    Vector2 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (hit == false)
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

            if (!onground)
            {
                velocity.y -= 1;
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


            if (onground == true)
            {
                jumptimer = 0;
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Respawn(startingPosition);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<Spike>())
        {
            hit = true;
        }
        if (collision.collider.gameObject.GetComponent<Flag>())
        {
            Win();
        }
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
    void Respawn(Vector2 position)
    {
        hit = false;
        velocity = Vector2.zero;
        jumptimer = 0;
        //transform.Translate(startingPosition.x, startingPosition.y, 0, Space.World);
        transform.position = new Vector3(startingPosition.x, startingPosition.y, 0);
    }
    void Win()
    {
        int level = int.Parse(SceneManager.GetActiveScene().name.Last().ToString());
        level++;
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
