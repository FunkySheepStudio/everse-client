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

    public void ShowMarkerWindow(GameObject marker)
    {
        
        UI.rootVisualElement.Q<Label>("MarkerId").text = "Marker ID: " + marker.name;
        UI.rootVisualElement.Q<VisualElement>("CreateMarker").visible = true;
    }

    public void HideMarkerWindow(GameObject marker)
    {
        UI.rootVisualElement.Q<VisualElement>("CreateMarker").visible = false;
    }
}
