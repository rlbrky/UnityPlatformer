using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float mySpeedX;
    [SerializeField] float speed; //private ancak unity üzerinden değer verilebiliyor.
    [SerializeField] float jumpPower;
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround; //Zeminde olup olmadığını kontrol ediyoruz.
    private bool canDoubleJump;
    [SerializeField] GameObject arrow;
    [SerializeField] bool attacked;
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    private Animator myAnimator;
    [SerializeField] int arrowCount;
    [SerializeField] Text arrowCountText;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] GameObject winPanel, losePanel;
    // Start is called before the first frame update
    void Start()
    {
        attacked = false;
        myBody = GetComponent<Rigidbody2D>();
        defaultLocalScale = transform.localScale;
        myAnimator = GetComponent<Animator>();
        arrowCountText.text = arrowCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Yatay axiste sağa ve sola gittiğimizde oluşan değerleri alıp oyuncumuzun hızına işliyoruz.
        //-1 ile 1 arasında sağ ve sol tuşa basma süresine bağlı olarak gelecek değerler.
        mySpeedX = Input.GetAxis("Yatay");
        myBody.velocity = new Vector2(mySpeedX * speed, myBody.velocity.y);

        //Speed parametresinin değerini buna kuruyoruz ve böylece animasyonlar arasında geçiş parametremiz oluşuyor.
        myAnimator.SetFloat("Speed",Mathf.Abs(mySpeedX));

        #region Player'ın sağ ve sol hareket yönüne göre yüzünün dönmesi.
        if (mySpeedX > 0)
        {
            //Sayı kullanmak mantıklı değil eğer karakterinin scale'ini değiştirirsen buradaki sayıya geri döneceğinden resimde
            //kaymalar oluşacaktır. Bu şekilde dinamik hale getiriyoruz böylece düzgün çalışacak.
            transform.localScale = new Vector3(defaultLocalScale.x,defaultLocalScale.y,defaultLocalScale.z);
        }
        else if (mySpeedX < 0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        #endregion
        #region Double Jump / Jump kontrolleri
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Boşluk tuşuna basıldı.");
            if (onGround == true)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                canDoubleJump = true;
                myAnimator.SetTrigger("Jump");
            }
            else
            {
                if (canDoubleJump == true)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                    canDoubleJump = false;
                }
            }
        }
        #endregion

        #region Ok atma kontrolleri
        if (Input.GetMouseButtonDown(0) && arrowCount>0)
        {
            if (attacked == false) {
                attacked = true;
                myAnimator.SetTrigger("Attack");
                //Fire();
                StartCoroutine(Fire(0.5f)); //Çağırma yöntemi. Invoke'dan daha iyi.
                //Invoke("Fire", 0.5f); --> delaylemek için yöntem voidde iyi çalışıyor gibi
                // Daha önce Fire fonksiyonunu update methodunda oluşturduğundan kullanamıyordun şuan kullanabiliyorsun.
            }
        }
        
        #endregion

        #region Saldırı hızı ayarları.
        if (attacked == true)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }

        if(currentAttackTimer <= 0)
        {
            attacked = false;
        }
        #endregion

    

}
    //Invoke metodu kullanılmıyor, o yüzden coroutine metodu kullandım.
    IEnumerator Fire(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); //Coroutine ile parametrenin döndürülmesi.
        //Okun oluşturulması.
        GameObject leArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        //Oluşturduğumuz hiyerarşide arrows un altında oluşturuyor ok objelerini.
        leArrow.transform.parent = GameObject.Find("Arrows").transform;

        if (transform.localScale.x > 0)
        {
            leArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(7f, 0f);
        }
        else
        {
            Vector3 leArrowScale = leArrow.transform.localScale;
            leArrow.transform.localScale = new Vector3(-leArrowScale.x, leArrowScale.y, leArrowScale.z);
            leArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-7f, 0f);
        }

        arrowCount--;
        arrowCountText.text = arrowCount.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log(collision.gameObject.name);
       if(collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<TimeControl>().enabled = false;
            Die();
        }
       else if (collision.gameObject.CompareTag("Finish"))
        {
            //winPanel.SetActive(true);
            Time.timeScale = 0;
            Destroy(collision.gameObject);
            StartCoroutine(Wait(true));
        }
    }

    public void Die()
    {
        GameObject.Find("Sound Controller").GetComponent<AudioSource>().clip = null;
        GameObject.Find("Sound Controller").GetComponent<AudioSource>().PlayOneShot(DeathSound);
        myAnimator.SetFloat("Speed", 0);
        myAnimator.SetTrigger("Die");
        //Hareketleri durduruyoruz.
        //myBody.constraints = RigidbodyConstraints2D.FreezePosition; //Pozisyon durdurma.
        myBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
        //losePanel.SetActive(true); //losePanel.active = true; --> Wait fonksiyonunda hallettik.
        //Time.timeScale = 0;
        StartCoroutine(Wait(false));
    }

    IEnumerator Wait(bool win)
    {
        yield return new WaitForSecondsRealtime(2f);

        if (win)
        {
            winPanel.SetActive(true); //winpanel.active = true; aynısı
        }
        else
        {
            losePanel.SetActive(true);
        }
    }
}
