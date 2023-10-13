using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public Action<PanelType> PanelStartCloseEvent;
    public Action<PanelType> PanelStartOpenEvent;
    public Action<PanelType> PanelOpenedFinishEvent;
    public Action<PanelType> PanelClosedFinishEvent;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
