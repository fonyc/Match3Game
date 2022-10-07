using Board.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainMenuAnimation 
{
    string Id { get; set; }
    void AppearAnimation(RectTransform rect, float delay);
    void HideAnimation(RectTransform rect);
}
