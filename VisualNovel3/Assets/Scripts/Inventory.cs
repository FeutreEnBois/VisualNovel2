using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryEnabled;
    public int preuvesCount = 0;
    public int allSlots;
    private int enabledSlots;
    private GameObject[] slot;
    public Dictionary<string,List<string>> preuves = new Dictionary<string, List<string>>();
    public GameObject slotHolder;

    public List<GameObject> KillerSlot = new List<GameObject>();
    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Tring to create multiple instance of Inventory");
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        allSlots = 4;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ToggleInventory();
        
    }

    public void ToggleInventory()
    {
        inventoryEnabled = !inventoryEnabled;
        inventory.SetActive(inventoryEnabled);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Item"){
            GameObject itemPickedUp = other.gameObject;
            GameObject item = itemPickedUp.GetComponent<GameObject>();

            AddItem(item);
        }
    }

    public bool Contains(int itemID)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if(slot[i].GetComponentInChildren<Slot>().ID == itemID)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(GameObject cool)
    {
        Item item = cool.GetComponent<Item>();
        Debug.Log("Add item");
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponentInChildren<Slot>().empty & !item.GetComponent<Item>().pickedUp)
            {
                item.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponentInChildren<Slot>().icon = item.icon;
                slot[i].GetComponentInChildren<Slot>().type = item.type;
                slot[i].GetComponentInChildren<Slot>().ID = item.ID;
                slot[i].GetComponentInChildren<Slot>().Name = item.Name;
                slot[i].GetComponentInChildren<Slot>().description = item.description;
                slot[i].GetComponentInChildren<Slot>().UpdateSlot();
                slot[i].GetComponentInChildren<Slot>().empty = false;
                break;
            }
        }

    }

    public bool PreuvesContains(string Informateur, string preuve, bool existe = true)
    {
        if (preuves.ContainsKey(Informateur))
        {
            if (preuves[Informateur].Contains(preuve))
            {
                return existe;
            }
        }

        return !existe;
    }
    public int PreuvesNbr(string Informateur)
    {
        if (preuves.ContainsKey(Informateur))
        {
            return preuves[Informateur].Count;
        }

        return 0;
    }

    public void AddPreuve(string Informateur, string preuve, int KillerSlotnbr = -1) // ex : Barman -> clef
    {

        if (!preuves.ContainsKey(Informateur))
        {
            //Debug.Log("Instantiating Preuve from " + Informateur);
            preuves[Informateur] = new List<string>();
        }
        if (preuves[Informateur].Contains(preuve))
        {
            Debug.LogWarning("Preuve alrealdy added : " + preuve);
            return;
        }
        else
        {
            Debug.Log("preuve '" + preuve + "' added to Informateur '" + Informateur + "' with success");
            preuves[Informateur].Add(preuve);
        }
        //if(KillerSlotnbr != -1)
        //{
        //    KillerSlot[KillerSlotnbr].GetComponent<SelectKillerSlotContainer>().addPreuve(preuve);
        //}
        preuvesCount++;
    }

}
