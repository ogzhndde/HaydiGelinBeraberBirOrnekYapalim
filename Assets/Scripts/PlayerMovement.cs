using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    public GameObject joystickBackground;
    public Rigidbody rb;
    public float speed;
    public Animator playerAnim;
    public GameObject box;
    public GameObject bluePoint, greenPoint, redPoint;
    public List<GameObject> collectedBox = new List<GameObject>();


    void Start()
    {
        //YUKARIDA TANIMLADIGIMIZ DEGISKENLERI SURUKLE BIRAK YERINE DIREK KODDAN ATAMA YAPIYORUZ
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //JOYSTICK ILE KARAKTERIN HAREKET ETTIRILDIGI YER
        if (joystickBackground.activeInHierarchy)
        {
            playerAnim.SetBool("isRunning", true);

            Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;

            // rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);
        }
        else
        {
            playerAnim.SetBool("isRunning", false);
        }

        //POZISYONUN SINIRLANDIRILDIGI YER
        Vector3 tempPosition = transform.position;
        tempPosition.x = Mathf.Clamp(transform.position.x, -5, 5);
        tempPosition.z = Mathf.Clamp(transform.position.z, -10, 10);
        transform.position = tempPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "BlueSpawn":
                TriggerControl(Color.blue, "Blue");
                break;
            case "GreenSpawn":
                TriggerControl(Color.green, "Green");
                break;
            case "RedSpawn":
                TriggerControl(Color.red, "Red");
                break;

            case "Point":
                //FOR DONGUSUYLE LISTEMIZDEKI TUM OBJELERIN TAG'LERINE GORE ESLESTIRIP KONUMLARINI GUNCELLIYORUZ
                for (int i = 0; i < collectedBox.Count; i++)
                {
                    if (collectedBox[i].tag == "Blue")
                    {
                        collectedBox[i].transform.SetParent(bluePoint.transform);
                        collectedBox[i].transform.position = bluePoint.transform.position + new Vector3(0, 2, 0);
                    }
                    if (collectedBox[i].tag == "Red")
                    {
                        collectedBox[i].transform.SetParent(redPoint.transform);
                        collectedBox[i].transform.position = redPoint.transform.position + new Vector3(0, 2, 0);
                    }
                    if (collectedBox[i].tag == "Green")
                    {
                        collectedBox[i].transform.SetParent(greenPoint.transform);
                        collectedBox[i].transform.position = greenPoint.transform.position + new Vector3(0, 2, 0);
                    }

                }
                collectedBox.Clear();

                break;
        }
    }

    public void TriggerControl(Color boxColor, string tag)
    {
        //KAFAMIZIN USTUNDE BIRIKTIRECEGIMIZ BOSLARIN SPAWNLANDIGI YER
        GameObject spawnedBox = Instantiate(box, transform.position, Quaternion.identity);

        //INSTANTIATE ETTIGIMIZ OBJEYI LISTEYE EKLIYORUZ
        collectedBox.Add(spawnedBox);

        //LISTENIN KONUMUNU GUNCELLIYORUZ
        spawnedBox.transform.position = (transform.position + new Vector3(0, 2, 0)) + (new Vector3(0, 0.5f, 0) * collectedBox.Count);
        // spawnedBox.transform.position =        (ilk konum)                        +            (ekleme yapilan kisim)           ;

        //OBJEYI LISTEYE EKLIYORUZ VE RENK-TAG AYARLARINI YAPIYORUZ
        spawnedBox.transform.SetParent(transform);
        spawnedBox.GetComponent<MeshRenderer>().material.color = boxColor;
        spawnedBox.tag = tag;
    }


}
