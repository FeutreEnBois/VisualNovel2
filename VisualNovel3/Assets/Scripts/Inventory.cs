using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryEnabled;

    private int allSlots;
    private int enabledSlots;
    private GameObject[] slot;
    
    public GameObject slotHolder;
    // Start is called before the first frame update
    void Start()
    {
        allSlots = 5;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryEnabled = !inventoryEnabled;
        
        if(inventoryEnabled == true) 
            {
                inventory.SetActive(true);

            } else {
                inventory.SetActive(false);
            }
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Item"){
            GameObject itemPickedUp = other.gameObject;
            GameObject item = itemPickedUp.GetComponent<GameObject>();

            AddItem(item);
        }
    }

    public void AddItem(GameObject cool){
        Item item = cool.GetComponent<Item>();
        Debug.Log("Add item");
        for(int i = 0; i < allSlots; i++){
            if(slot[i].GetComponent<Slot>().empty & !item.GetComponent<Item>().pickedUp){
                item.GetComponent<Item>().pickedUp = true;
                
                slot[i].GetComponent<Slot>().icon = item.icon;
                slot[i].GetComponent<Slot>().type = item.type;
                slot[i].GetComponent<Slot>().ID =item.ID;
                slot[i].GetComponent<Slot>().description = item.description;
                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                break;

                



            }
        }

    }
}
