using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool onGround;
    [SerializeField] float speed;
    private float width;
    private Rigidbody2D myBody;
    [SerializeField] LayerMask engel;
    private static int totalEnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalEnemyCount++;
        Debug.Log(gameObject.name + " oluştu." + "Oyundaki toplam düşman sayısı: " + totalEnemyCount);
        //Sınırların ortasından x'e ulaşıyor.
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * width / 3),Vector2.down,2f,engel);

        if(hit.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Sığması için 3'e böldüm normalde 2'ye bölününce olması lazım bu şekilde düzeltirsin.
        Vector3 EnemyRealPosition = transform.position + (transform.right * width / 3);
        Gizmos.DrawLine(EnemyRealPosition,EnemyRealPosition+new Vector3(0,-2f,0));
    }

    void Flip()
    {
        if (!onGround)
        {
            transform.eulerAngles += new Vector3(0, 180, 0);
        }
        //3f lik birimle ne tarafa bakıyorsa oraya gidiyor.
        myBody.velocity = new Vector2(transform.right.x * speed, 0f);
    }
}
