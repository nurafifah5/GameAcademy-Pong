using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    //Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;
    //besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;
    //titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        //mulai game
        RestartGame();

        trajectoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetBall()
    {
        //reset posisi bola menjadi (0,0)
        transform.position = Vector2.zero;

        //reset kecepatan bola menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;
    }

    void PushBall()
    {
        //tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        //float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);
        float yRandomInitialForce = 10.0f;

        //tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        //jika nilainya dibawah 1, bola bergerak ke kiri. Jika tidak bola bergerak ke kanan
        if(randomDirection < 1.0f)
        {
            //gunakan gaya untuk menggerakkan bola
            rigidBody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce));
        } else
        {
            rigidBody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce));
        }
    }

    void RestartGame()
    {
        //kembalikan bola ke posisi semula
        ResetBall();

        //setelah 2 detik berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
        trajectoryOrigin = transform.position;
    }

    public Vector2 TrajectoryOrigin
    {
        get
        {
            return trajectoryOrigin;
        }
    }
}
