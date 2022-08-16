using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    UIDocument UI;

    public FunkySheep.Events.SimpleEvent markersAdd;
    public FunkySheep.Events.SimpleEvent markersCreate;
    private void Awake()
    {
        UI = GetComponent<UIDocument>();
        UI.rootVisualElement.Q<Button>("markers-btn-add").clicked += markersAdd.Raise;
        UI.rootVisualElement.Q<Button>("markers-btn-create").clicked += markersCreate.Raise;
    }

    public void ShowMarkerWindow()
    {
        UI.rootVisualElement.Q<VisualElement>("CreateMarker").visible = true;
    }

    public void HideMarkerWindow()
    {
        UI.rootVisualElement.Q<VisualElement>("CreateMarker").visible = false;
    }
}
