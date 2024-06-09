using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalItem : Item
{
    public SpriteRenderer ViewRenderer { get; private set; }
    public enum eNormalType
    {
        TYPE_ONE,
        TYPE_TWO,
        TYPE_THREE,
        TYPE_FOUR,
        TYPE_FIVE,
        TYPE_SIX,
        TYPE_SEVEN
    }

    public eNormalType ItemType;

    public BoardSkin BoardSkin;

    public NormalItem(BoardSkin boardSkin) {
        BoardSkin = boardSkin;
    }

    public void SetType(eNormalType type)
    {
        ItemType = type;
    }

    public override void SetView()
    {
        if (!View) {
            View = GameObject.Instantiate(Resources.Load<GameObject>(Constants.PREFAB_NORMAL_TYPE_ONE)).transform;
            ViewRenderer = View.GetComponent<SpriteRenderer>();
        }
        ViewRenderer.sprite = BoardSkin.textures[(int)ItemType];
    }

    internal override bool IsSameType(Item other)
    {
        NormalItem it = other as NormalItem;

        return it != null && it.ItemType == this.ItemType;
    }
}
