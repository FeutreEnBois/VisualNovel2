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
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(item);
        }
    }

    public void AddItem(Item cool){
        for(int i = 0; i < allSlots; i++){
            if(slot[i].GetComponent<Slot>().empty){
                cool.GetComponent<Item>().pickedUp = true;

            
                slot[i].GetComponent<Slot>().icon = cool.GetComponent<Item>().icon;
                slot[i].GetComponent<Slot>().type = cool.GetComponent<Item>().type;
                slot[i].GetComponent<Slot>().ID =cool.GetComponent<Item>().ID;
                slot[i].GetComponent<Slot>().description = cool.GetComponent<Item>().description;
            }
        }

    }
}
