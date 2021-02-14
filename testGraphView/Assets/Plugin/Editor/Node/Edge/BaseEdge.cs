using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


public class BaseEdge : Node
{
    VisualElement input;
    VisualElement output;
    VisualElement main;
    VisualElement content;

    public BaseEdge()
    {
        input = inputContainer;
        output = outputContainer;
        main = mainContainer;
        content = contentContainer;
    }
    
    public VisualElement GetInputContainer()
    {
        return input;
    }

    public VisualElement GetOutputContainer()
    {
        return output;
    }

    public VisualElement GetMainContainer()
    {
        return main;
    }

    public VisualElement GetContentContainer()
    {
        return content;
    }
}
