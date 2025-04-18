using UnityEngine;

public class SuckyObject : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController target;
    private bool sucking;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sucker")) {
            transform.position = Vector3.Lerp(transform.position, target.Head.transform.position, Time.deltaTime * 4.0f);
            sucking = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (sucking == true && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
