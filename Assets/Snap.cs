
using UnityEngine;
using UnityEngine.UIElements;

public class Snap : MonoBehaviour
{
    private bool hasCollided = false; //to ensure the game object gets parented ONE TIME

    public GameObject childObject; //reference to the child that will be snapped
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Pickable"))
            {
                SnapObjectOnTop(collision.gameObject);
            }
        }
    }

    private void SnapObjectOnTop(GameObject childObject)
    {

        //get the collider of this object
        Collider parentCollider = GetComponent<Collider>();

        if (parentCollider != null)
        {
            //get the collider of the child object
            Collider childCollider = childObject.GetComponent<Collider>();

            if (childCollider != null)
            {
                float parentTopY = parentCollider.bounds.max.y;     //get top of parent
                float childHeight = childCollider.bounds.extents.y; //get half the height of child ?

                //set the child object's position to be on top of the parent
                Vector3 newPosition = new Vector3(transform.position.x, parentTopY + childHeight, transform.position.z);

                //set the new position of the child object
                childObject.transform.position = newPosition;

            }
            else
            {
                //DEBUG
                Debug.LogError("Child object does not have a Collider");
            }
        }
        else
        {
            Debug.LogError("Parent object does not have a Collider");
        }
    }
}
