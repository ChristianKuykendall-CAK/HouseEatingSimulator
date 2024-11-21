using System;
using Unity.VisualScripting;
using UnityEngine;

//this class is for the recipe book obvi
public class RecipeBookManager : MonoBehaviour
{
    public Transform Head;
    public float itemPickupDistance;
    public InventoryUIManager inventoryUI;
    public Boolean canRead;

    //debug
    bool DEBUG = true;

    //canvas
    public Canvas recipeBookCanvas;

    private void Update()
    {
        // picking objects
        RaycastHit hit;
        bool cast = Physics.Raycast(Head.position, Head.forward, out hit, itemPickupDistance);
        if (cast)
        {
            //if object is book
            if (hit.transform.CompareTag("Book"))
            {
                if(DEBUG) Debug.Log("BOOK");
                canRead = true;

                //if looking at book and E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (DEBUG) Debug.Log("E was pressed");
                    //recipeBookCanvas enabled when E is hit
                    if (recipeBookCanvas.enabled)
                    {
                        recipeBookCanvas.enabled = false;
                    }
                    else
                    {
                        recipeBookCanvas.enabled = true;
                    }
                }
            }
        }
        else
        {
            if (DEBUG) Debug.Log("not book");
            canRead = false;
        }

        //print E
        inventoryUI.UpdateBookText(canRead);
    }

    public void Start()
    {
        //set recipe book canvas to disabled
        recipeBookCanvas.enabled = false;
    }

}
