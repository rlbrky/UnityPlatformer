using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float minX, maxX;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Burada gameObject yazıp yazmaman fark etmiyor her türlü main camera objesine ulaşabiliyoruz.
        //gameObject.transform

        //Buradaki Clamp fonksiyonu girilen minimum ve maximum değerleri arasında değer döndürülmesini sağlıyor,
        //minimum ve maximum un dışına çıkılırsa en son değeri(minX ve maxX bu durum için.) döndürüyor.
        transform.position = new Vector3(Mathf.Clamp(playerTransform.position.x,minX,maxX), transform.position.y, transform.position.z);
    }
}
