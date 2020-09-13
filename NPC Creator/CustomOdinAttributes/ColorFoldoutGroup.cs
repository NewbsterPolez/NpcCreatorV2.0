using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

public class ColorFoldoutGroup : PropertyGroupAttribute
{
    public float R =1 , G = 1, B = 1, A = 1;
    public bool Expanded;

    public ColorFoldoutGroup(string path) : base (path)
    {

    }

    public ColorFoldoutGroup(string path, float r, float g, float b, float a = 1f, bool expanded = false) : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
        this.Expanded = expanded;
    }
}

public class ColorFoldoutGroupDrawer: OdinGroupDrawer<ColorFoldoutGroup>
{
    private bool isOpen;

    protected override void Initialize()
    {
        isOpen = this.Attribute.Expanded;
    }
    protected override void DrawPropertyLayout(GUIContent label)
    {
        GUIHelper.PushColor(new Color(this.Attribute.R, this.Attribute.G, this.Attribute.B, this.Attribute.A));
        SirenixEditorGUI.BeginBox();
        SirenixEditorGUI.BeginBoxHeader();
        GUIHelper.PopColor();

        this.isOpen = SirenixEditorGUI.Foldout(this.isOpen, label);
        SirenixEditorGUI.EndBoxHeader();

        if(SirenixEditorGUI.BeginFadeGroup(this, isOpen))
        {
            for(int i = 0; i < this.Property.Children.Count; i++)
            {
                this.Property.Children[i].Draw();
            }
        }
        SirenixEditorGUI.EndFadeGroup();
        SirenixEditorGUI.EndBox();
    }
}
