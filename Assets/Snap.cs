using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    private bool hasCollided = false; //to ensure the game object gets parented ONE TIME
    public Vector3 offset = new Vector3(0, 1, 0);
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Pickable"))
            {
                //var newParent = new GameObject();
                //newParent.layer = LayerMask.NameToLayer("Snapping"); //set parent to new layer as to not collide with floor
                collision.transform.SetParent(transform); //set parent to the object

                hasCollided = true;

                if (transform.parent != null)
                {
                    //set child's position to the parent's position
                    collision.gameObject.transform.position = transform.position + offset;
                }
            }
        }
    }

}
