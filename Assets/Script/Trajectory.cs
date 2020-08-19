using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public BallControl ball;
    CircleCollider2D ballCollider;
    Rigidbody2D ballRigidbody;
    //bola bayangan yang akan ditampikan di titik tumbukan
    public GameObject ballAtCollision;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //inisiasi status pantulan lintasan yang hanya akan ditampilkan jika lintasan bertumbukan dengan objek tertentu
        bool drawBallAtCollision = false;
        //titik tumbukan yang digeser untuk menggambar ballAtCollision
        Vector2 offsetHitPoint = new Vector2();

        //tentukan titik tumbukan dengan deteksi pergerakan lingkaran 
        RaycastHit2D[] circleCastHit2DArray = Physics2D.CircleCastAll(ballRigidbody.position, ballCollider.radius, ballRigidbody.velocity.normalized);

        //untuk setiap titik tumbukan
        foreach(RaycastHit2D circleCastHit2D in circleCastHit2DArray)
        {
            //jika terjadi tumbukan dan tumbukan tersebut tidak dengan bola (karena garis lintasan digambar dari titik tengah bola)
            if (circleCastHit2D.collider != null && circleCastHit2D.collider.GetComponent<BallControl>() == null)
            {
                //garis lintasan akan digambar dari titik tengah bola saat ini ke titik tengah bola pada saat tumbukan, yaitu sebuah titik yang di-offset dari titik tumbukan berdasar vektor normal titik tersebut sebesar jari-jari bola
                //tentukan titik tumbukan
                Vector2 hitPoint = circleCastHit2D.point;

                //tentukan normal di titik tumbukan
                Vector2 hitNormal = circleCastHit2D.normal;

                //tentukan titik tengah bola pada saar bertumbukan
                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

                //gambar garis lintasan dari titik tengah bola saat ini ke titik tengah bola saat bertumbukan
                DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);

                //kalau bukan sidewall, gambar pantulannya
                if (circleCastHit2D.collider.GetComponent<SideWall>() == null)
                {
                    //hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                    //hitung vektor keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    //hitung dot product dari outVector dan hitNormal. Digunakan agar garis lintasan tidak digambar ketika terjadi tumbukan
                    float outDot = Vector2.Dot(outVector, hitNormal);
                    if (outDot > -1.0f && outDot < 1.0f)
                    {
                        //gambar lintasan pantulannya
                        DottedLine.DottedLine.Instance.DrawDottedLine(offsetHitPoint, offsetHitPoint + outVector * 10.0f);

                        //menggambar bola bayangan di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }
                //hanya gambar lintasan untuk satu titik tumbukan jadi keluar dari loop
                break;
            }
        }

        if (drawBallAtCollision)
        {
            //if true akan menggambar bola bayangan di prediksi titik tumbukan
            ballAtCollision.transform.position = offsetHitPoint;
            ballAtCollision.SetActive(false);
        }
        else
        {
            //sembunyikan bola bayangan
            ballAtCollision.SetActive(false);
        }
    }
}
