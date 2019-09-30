using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FindSquares : EditorWindow
{
    
    [MenuItem("Window/FindSquares")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindSquares));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("find squares and make them sliced baby"))
        {
            FindInSelected();
        }
    }

    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        foreach (GameObject g in go)
        {
            if(g.GetComponent<Image>() != null && g.GetComponent<Image>().sprite != null && g.GetComponent<Image>().sprite.name == "square")
            {
                g.GetComponent<Image>().type = Image.Type.Sliced;
                g.GetComponent<Image>().Rebuild(CanvasUpdate.PostLayout);
                g.GetComponent<Image>().Rebuild(CanvasUpdate.LatePreRender);
                g.GetComponent<Image>().Rebuild(CanvasUpdate.MaxUpdateValue);
                g.GetComponent<Image>().Rebuild(CanvasUpdate.Prelayout);

            }
        }
    }
}
