using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class clickObj : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerDown(PointerEventData pointerEventData) //upuszczenie
    {

    }

    public void OnPointerUp(PointerEventData pointerEventData) //wci�niecie
    {
        //Debug.Log(name);
    }

    public void OnPointerEnter(PointerEventData pointerEventData) //najechanie myszk�
    {
        //Debug.Log(name);
    }

    public void OnPointerExit(PointerEventData pointerEventData) //koniec najechania myszk�
    {

    }
}
