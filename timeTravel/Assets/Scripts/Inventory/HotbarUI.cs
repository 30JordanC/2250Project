using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public static HotbarUI Instance { get; private set; }

    [Header("Drag these in from the Inspector")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform hotbarContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int totalSlots = 10;

    [Header("Colors")]
    [SerializeField] private Color normalColor   = new Color(0.1f, 0.1f, 0.2f, 0.85f);
    [SerializeField] private Color selectedColor = new Color(0f, 0.8f, 1f, 0.85f);

    private List<HotbarSlotUI> slotUIs = new List<HotbarSlotUI>();
    private int selectedIndex = 0;

   void Awake()
{
    if (Instance != null && Instance != this) { Destroy(gameObject); return; }
    Instance = this;
    gameObject.SetActive(true); 
}

    void Start()
    {
        BuildSlots();
    }

    void Update()
    {
        HandleInput();
        RefreshDisplay();
    }

    private void BuildSlots()
    {
        foreach (Transform child in hotbarContainer) Destroy(child.gameObject);
        slotUIs.Clear();

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject go = Instantiate(slotPrefab, hotbarContainer);
            go.name = "Slot_" + i;
            HotbarSlotUI slot = go.GetComponent<HotbarSlotUI>();
            slot.Init(i, this);
            slotUIs.Add(slot);
        }

        SelectSlot(0);
    }

    private void RefreshDisplay()
    {
        if (inventory == null) return;
        List<Item> items = inventory.Items;

        for (int i = 0; i < totalSlots; i++)
        {
            if (i < items.Count && items[i] != null)
                slotUIs[i].Show(items[i]);
            else
                slotUIs[i].Clear();
        }
    }

    public void ForceRefresh() => RefreshDisplay();

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= totalSlots) return;
        slotUIs[selectedIndex].SetHighlight(false, normalColor);
        selectedIndex = index;
        slotUIs[selectedIndex].SetHighlight(true, selectedColor);
    }

    public Item GetSelectedItem()
    {
        List<Item> items = inventory.Items;
        return (selectedIndex < items.Count) ? items[selectedIndex] : null;
    }

    private void HandleInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SelectSlot((selectedIndex - 1 + totalSlots) % totalSlots);
        if (scroll < 0f) SelectSlot((selectedIndex + 1) % totalSlots);

        for (int i = 0; i < 10; i++)
        {
            KeyCode key = i < 9 ? (KeyCode)((int)KeyCode.Alpha1 + i) : KeyCode.Alpha0;
            if (Input.GetKeyDown(key)) SelectSlot(i);
        }
    }
}