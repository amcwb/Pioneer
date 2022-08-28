using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveEntity : MonoBehaviour
{
    Color BoxColor = Color.white;
    BoxCollider m_Renderer;
    bool IsMouseOver = false;
    public Rect windowRect = new Rect(Screen.width / 2 - 120, Screen.height / 2 - 40, 120, 40);

    void Start()
    {
        m_Renderer = GetComponent<BoxCollider>();
    }

    void OnGUI()
    {
        if (IsMouseOver) DrawBoxAround();
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
    }

    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
        {
            print("Got a click");
        }

        if (GUI.Button(new Rect(10, 40, 100, 30), "Testing!"))
        {
            print("Got a click2");
        }
    }

    void OnMouseOver()
    {
        IsMouseOver = true;
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        // m_Renderer.material.color = m_OriginalColor;
        IsMouseOver = false;
    }

    void DrawBoxAround()
    {
        // Draw a box around this GameObject.
        // top left point of rectangle
        Vector3 topLeftWorld = new Vector3(m_Renderer.bounds.center.x - m_Renderer.bounds.extents.x, m_Renderer.bounds.center.y - m_Renderer.bounds.extents.y, 0);
        //bottom right point of rectangle
        Vector3 bottomRightWorld = new Vector3(m_Renderer.bounds.center.x + m_Renderer.bounds.extents.x, m_Renderer.bounds.center.y + m_Renderer.bounds.extents.y, 0);

        Vector3 topLeftScreen = Camera.main.WorldToScreenPoint(topLeftWorld);
        Vector3 bottomRightScreen = Camera.main.WorldToScreenPoint(bottomRightWorld);

        float width = bottomRightScreen.x - topLeftScreen.x;
        float height = topLeftScreen.y - bottomRightScreen.y;


        GUI.Box(new Rect(topLeftScreen.x, Screen.height - topLeftScreen.y, width, height), "");

    }
}
