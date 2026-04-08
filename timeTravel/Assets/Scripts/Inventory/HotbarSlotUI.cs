using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("Drag children in from the Inspector")]
    [SerializeField] private Image background;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image selectionBorder;
    [SerializeField] private TextMeshProUGUI slotNumberText;

    private int index;
    private HotbarUI hotbarUI;

    public void Init(int slotIndex, HotbarUI parent)
    {
        index    = slotIndex;
        hotbarUI = parent;

        if (slotNumberText != null)
            slotNumberText.text = slotIndex < 9 ? (slotIndex + 1).ToString() : "0";

        Clear();
    }

    public void Show(Item item)
    {
        ItemIcon iconComp = item.GetComponent<ItemIcon>();

        if (itemIcon != null)
        {
            if (iconComp != null && iconComp.icon != null)
            {
                itemIcon.sprite  = iconComp.icon;
                itemIcon.color   = Color.white;
                itemIcon.enabled = true;
            }
            else
            {
                itemIcon.sprite  = null;
                itemIcon.color   = new Color(1f, 1f, 1f, 0.3f);
                itemIcon.enabled = true;
            }
        }
    }

    public void Clear()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite  = null;
            itemIcon.enabled = false;
        }
    }

    public void SetHighlight(bool selected, Color color)
    {
        if (selectionBorder != null)
        {
            selectionBorder.color   = color;
            selectionBorder.enabled = selected;
        }
        transform.localScale = selected ? Vector3.one * 1.07f : Vector3.one;
    }

    public void OnClicked() => hotbarUI.SelectSlot(index);
}