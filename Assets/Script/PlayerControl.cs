using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Tombol untuk menggerakkan ke atas
    public KeyCode upButton = KeyCode.W;
    //Tombol untuk menggerakkan ke bawah
    public KeyCode downButton = KeyCode.S;
    //Kecepatan Gerak
    public float speed = 10.0f;
    //Batas atas dan bawah game scene (Batas bawah pakai minus - )
    public float yBoundary = 9.0f;
    //Rigidbody 2D Raket
    private Rigidbody2D rigidBody2D;
    //Skor pemain
    private int score;
    //titik tumbukan terakhir dengan bola, untuk menampilkan variabel-variabel fisika terkait tumbukan tersebut
    private ContactPoint2D lastContactPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mendapatkan kecepatan raket saat ini
        Vector2 velocity = rigidBody2D.velocity;

        //jika pemain menekan tombol keatas beri kecepatan positif ke komponen y (ke atas)
        if (Input.GetKey(upButton))
        {
            velocity.y = speed;
        } else if (Input.GetKey(downButton)) {
            //jika pemain menekan tombol kebawah, beri kecepatan negatif ke komponen y (ke bawah)
            velocity.y = -speed;
        } else
        {
            //jika pemain tidak menekan tombol apa-apa, kecepatannya nol
            velocity.y = 0.0f;
        }
        //masukkan kembali kecepatannya ke rigidbody2d
        rigidBody2D.velocity = velocity;

        //Dapatkan posisi raket sekarang
        Vector3 position = transform.position;
        //Jika posisi raket melewati batas atas (yBoundary), kembalikan ke batas atas tersebut
        if (position.y > yBoundary)
        {
            position.y = yBoundary;
        } else if (position.y < -yBoundary)
        {
            //Jika posisi raket melewati batas bawah (-yBoundary), kembalikan ke batas atas tersebut
            position.y = -yBoundary;
        }
        //masukkan kembali posisinya ke transform
        transform.position = position;
    }

    public void IncrementScore()
    {
        //menaikkan skor sebanyak 1 poin
        score++;
    }

    public void ResetScore()
    {
        //mengembalikan skor menjadi 0
        score = 0;
    }

    public int Score
    {
        //mendapatkan nilai skor
        get
        { return score; }
    }

    public ContactPoint2D LastContactPoint
    {
        //untuk mengakses infotmasi titik kontak dari kelas lain
        get
        {
            return lastContactPoint;
        }
    }

    void OnCollisionEnter2D(Collision2D Collision)
    {
        //ketika terjadi tumbukan dengan bola, rekam titik kontaknya
        if (Collision.gameObject.name.Equals("Ball")){
            lastContactPoint = Collision.GetContact(0);
        }
    }
}
