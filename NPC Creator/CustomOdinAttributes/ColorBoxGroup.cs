using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;

public class ColorBoxGroup : PropertyGroupAttribute
{
    public float R =1 , G = 1, B = 1, A = 1;

    public bool ShowLabel;

    public ColorBoxGroup(string path) : base(path)
    {

    }

    public ColorBoxGroup(string path, float r, float g, float b, float a = 1f, bool showLabel = true) : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
        this.ShowLabel = showLabel;
    }

    public class ColorBoxGroupDrawer : OdinGroupDrawer<ColorBoxGroup>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIHelper.PushColor(new Color(this.Attribute.R, this.Attribute.G, this.Attribute.B, this.Attribute.A));
            SirenixEditorGUI.BeginBox();
            if (this.Attribute.ShowLabel == true)
            {
                SirenixEditorGUI.BeginBoxHeader();
                GUIHelper.PopColor();
                GUILayout.Label(label);
                SirenixEditorGUI.EndBoxHeader();
            }
            else 
                GUIHelper.PopColor();

            for (int i = 0; i < this.Property.Children.Count; i++)
            {
                this.Property.Children[i].Draw();
            }
            
            SirenixEditorGUI.EndBox();
        }
    }
}
