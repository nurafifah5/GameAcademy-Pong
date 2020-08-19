using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //pemain 1
    public PlayerControl player1;
    private Rigidbody2D player1Rigidbody;

    //pemain 2
    public PlayerControl player2;
    private Rigidbody2D player2Rigidbody;

    //Bola
    public BallControl ball;
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;

    //skor maksimal
    public int maxScore;

    //apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;

    //objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;

    // Start is called before the first frame update
    void Start()
    {
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();    
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //untuk menampilkan GUI
    void OnGUI()
    {
        //tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);

        //tombol restart untuk memulai game dari awal
        if(GUI.Button(new Rect(Screen.width / 2 - 60,35,120,53), "RESTART"))
        {
            //ketika tombol restart ditekan, reset skor kedua pemain
            player1.ResetScore();
            player2.ResetScore();

            //..dan restart game
            ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        //jika pemain 1 menang (skornya maks)
        if(player1.Score == maxScore)
        {
            //tampilkan teks di bagian kiri layar
            GUI.Label(new Rect(Screen.width / 2 -150, Screen.height / 2 - 10, 2000, 1000), "PLAYER 1 WINS");

            //..dan kembalikan bola ke tengah
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        } else if (player2.Score == maxScore)
        {
            //sebaliknya jika pemain 2 menang (skornya maks)
            //menampilkan teks di bagian kanan layar
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER 2 WINS");

            //mengembalikan bola ke tengah
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        //jika isDebugWindowShown == true, tampilkan text area untuk debug window
        if (isDebugWindowShown)
        {
            //simpan nilai warna lama GUI
            Color oldColor = GUI.backgroundColor;

            //beri warna baru
            GUI.backgroundColor = Color.red;

            //variabel-variabel fisika
            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            //tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

            //tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            //kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }

        //toggle nilai debug window ketika pemain mengklik tombol ini
        if(GUI.Button(new Rect(Screen.width/2 - 60, Screen.height-73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }

    }
}
