using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "BoardSkin", menuName = "ScriptableObjects/BoardSkin", order = 1)]
public class BoardSkin : ScriptableObject {
    public List<Sprite> textures;
}