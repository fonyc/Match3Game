using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDB : MonoBehaviour
{
    private static SpriteDB _instance;
    [SerializeField] Sprite[] spriteList;

    public static SpriteDB Instance { get => _instance; set => _instance = value; }
    public Sprite[] SpriteList { get => spriteList; set => spriteList = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


}
