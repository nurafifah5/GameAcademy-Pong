using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    //pemain akan bertambah skornya jika bola menyentuk dinding
    public PlayerControl player;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //akan dipanggil ketika objek lain ber collider(bola bersentuhan dengan dinding)
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        //jika objek tersebut bernama "Ball"
        if(anotherCollider.name == "Ball")
        {
            //tambahkan skor ke pemain
            player.IncrementScore();

            //jika skor pemain belum mencapai skor maksimal
            if(player.Score < gameManager.maxScore)
            {
                //restart game setelah bola mengenai dinding
                anotherCollider.gameObject.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
