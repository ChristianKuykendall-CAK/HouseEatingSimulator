using UnityEngine;

public class SuckyObject : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController target;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sucker")) {
            transform.position = Vector3.Lerp(transform.position, target.Head.transform.position, Time.deltaTime * 4.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
