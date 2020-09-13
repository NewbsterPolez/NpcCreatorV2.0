using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class NpcCreatorSaveWindow : OdinEditorWindow
{
    private bool NpcExists = false;

    [DisplayAsString(false)]
    [OnInspectorGUI]
    [HideLabel]
    [PropertySpace(0, 5)]
    private string StateInfo;

    private string InitialInfo = "Would you like to save the NPC as a Prefab or save changes to this instance of the NPC?";
    private string NpcExistsInfo = "An NPC Prefab already exists with this name. Would you like to save this NPC as a new variant with the same name or overwrite the existing Prefab?";

    protected override void OnEnable()
    {
        base.OnEnable();
        StateInfo = InitialInfo;
        NpcExists = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        NpcExists = false;
    }

    [OnInspectorGUI]
    [PropertySpace(0, 5)]
    private void Divider()
    {
        SirenixEditorGUI.HorizontalLineSeparator(1);
    }

    [HideIf("NpcExists")]
    [ResponsiveButtonGroup(DefaultButtonSize = ButtonSizes.Large)]
    private void SaveAsPrefab()
    { 
        if(GetWindow<NpcCreator>().CheckForExistingPrefab() == false)
        {
            GetWindow<NpcCreator>().SaveAsNewPrefab();
            this.Close();
        }
        else
        {
            NpcExists = true;
            StateInfo = NpcExistsInfo;
        }
    }

    [ShowIf("NpcExists")]
    [ResponsiveButtonGroup(DefaultButtonSize = ButtonSizes.Large)]
    private void SaveAsNewPrefab()
    {
        GetWindow<NpcCreator>().SaveAsNewPrefab();
        this.Close();
    }

    [ShowIf("NpcExists")]
    [ResponsiveButtonGroup(DefaultButtonSize = ButtonSizes.Large)]
    private void SaveAsPrefabOverwrite()
    {
        GetWindow<NpcCreator>().SaveAsOverwrite();
        this.Close();
    }
    
    [HideIf("NpcExists")]
    [ResponsiveButtonGroup(DefaultButtonSize = ButtonSizes.Large)]
    private void SaveThisInstance()
    {
        GetWindow<NpcCreator>().SaveInstance();
        this.Close();
    }
}
