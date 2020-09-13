using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

public class NpcCreator : OdinEditorWindow
{
    #region ==> Show Window Method <==

    [MenuItem("Tools/Synty Tools/NPC Creator")]
    private static void OpenWindow()
    {
        var editorAsm = typeof(Editor).Assembly;
        var inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow");
        GetWindow<NpcCreator>(inspWndType).Show();
    }

    #endregion

    #region ==> Editor Variables <==

    private enum Options
    {
        Gender,
        Head,
        Eyebrows,
        FacialHair,
        Helmet,
        Cowl,
        Face_Guard,
        Hat,
        Chest,
        Sleeve_Right,
        Sleeve_Left,
        Wrist_Right,
        Wrist_Left,
        Glove_Right,
        Glove_Left,
        Waist,
        Boot_Right,
        Boot_Left,
        Ears,
        Hair,
        FrontHead_Accessory,
        SideHead_Accessory,
        BackHead_Accessory,
        Cape,
        Shoulder_Right,
        Shoulder_Left,
        Elbow_Right,
        Elbow_Left,
        Belt_Accessory,
        Knee_Right,
        Knee_Left,
    }

    #region == Option Lists ==
    //List of all Npc part options
    private List<GameObject> HeadList = new List<GameObject>();
    private List<GameObject> EyebrowList = new List<GameObject>();
    private List<GameObject> HairList = new List<GameObject>();
    private List<GameObject> FacialHairList = new List<GameObject>();
    private List<GameObject> EarsList = new List<GameObject>();
    private List<GameObject> HelmetList = new List<GameObject>();
    private List<GameObject> CowlList = new List<GameObject>();
    private List<GameObject> FaceGuardList = new List<GameObject>();
    private List<GameObject> HatList = new List<GameObject>();
    private List<GameObject> TorsoList = new List<GameObject>();
    private List<GameObject> WaistList = new List<GameObject>();
    private List<GameObject> SleeveRightList = new List<GameObject>();
    private List<GameObject> SleeveLeftList = new List<GameObject>();
    private List<GameObject> WristRightList = new List<GameObject>();
    private List<GameObject> WristLeftList = new List<GameObject>();
    private List<GameObject> GloveRightList = new List<GameObject>();
    private List<GameObject> GloveLeftList = new List<GameObject>();
    private List<GameObject> BootRightList = new List<GameObject>();
    private List<GameObject> BootLeftList = new List<GameObject>();
    private List<GameObject> FrontHeadAccessoryList = new List<GameObject>();
    private List<GameObject> SideHeadAccessoryList = new List<GameObject>();
    private List<GameObject> BackHeadAccessoryList = new List<GameObject>();
    private List<GameObject> CapeList = new List<GameObject>();
    private List<GameObject> BeltAccessoryList = new List<GameObject>();
    private List<GameObject> KneeRightList = new List<GameObject>();
    private List<GameObject> KneeLeftList = new List<GameObject>();
    private List<GameObject> ElbowRightList = new List<GameObject>();
    private List<GameObject> ElbowLeftList = new List<GameObject>();
    private List<GameObject> ShoulderRightList = new List<GameObject>();
    private List<GameObject> ShoulderLeftList = new List<GameObject>();
    #endregion

    #region == Current Selection Objs ==
    //Containers to Reference the selected option's Obj to be used for disabling said Obj once a change is made
    private GameObject GenderOBJ;
    private GameObject HeadOBJ;
    private GameObject EyebrowsOBJ;
    private GameObject FacialHairOBJ;
    private GameObject HairOBJ;
    private GameObject EarOBJ;
    private GameObject HelmetOBJ;
    private GameObject CowlOBJ;
    private GameObject FaecGuardOBJ;
    private GameObject HatOBJ;
    private GameObject ChestOBJ;
    private GameObject WaistOBJ;
    private GameObject ShoulderLeftOBJ;
    private GameObject ShoulderRightOBJ;
    private GameObject SleeveLeftOBJ;
    private GameObject SleeveRightOBJ;
    private GameObject ElbowpadLeftOBJ;
    private GameObject ElbowpadRightOBJ;
    private GameObject WristRightOBJ;
    private GameObject WristLeftOBJ;
    private GameObject GloveLeftOBJ;
    private GameObject GloveRightOBJ;
    private GameObject KneepadLeftOBJ;
    private GameObject KneepadRightOBJ;
    private GameObject BootLeftOBJ;
    private GameObject BootRightOBJ;
    private GameObject CapeOBJ;
    private GameObject BeltAccessoryOBJ;
    private GameObject FrontHeadAccessoryOBJ;
    private GameObject BackHeadAccessoryOBJ;
    private GameObject SideHeadAccessoryOBJ;
    #endregion

    #region == Npc Selection Variables ==
    private NpcCreatorSelector selection = null;
    private bool IsSelectionLocked = false;
    #endregion

    #region == GUI IMG Variables ==
    private Texture2D unlockNPCimg;
    private Texture2D editorBanner;
    private EditorIcon leftArrow = EditorIcons.TriangleLeft;
    private EditorIcon rightArrow = EditorIcons.TriangleRight;
    private Rect arrowPadding = new Rect(0, -2, 0, 0);
    #endregion

    #region == Containers to reference the Base Parts of a NPC ==
    private GameObject MaleParts;
    private GameObject FemaleParts;
    private GameObject AllParts;
    #endregion

    #endregion

    #region ==> Draw Editor Banner <==
    [OnInspectorGUI]
    [PropertyOrder(-2)]
    private void DrawBanner()
    {
        GUILayoutUtility.GetRect(1, 3, GUILayout.ExpandWidth(false));
        Rect space = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(editorBanner.height));
        float width = space.width;

        space.xMin = (width - editorBanner.width * 1.2f) / 2;
        if (space.xMin < 0) space.xMin = 0;

        space.width = editorBanner.width * 1.2f;
        space.height = editorBanner.height * 1.2f;
        GUI.DrawTexture(space, editorBanner, ScaleMode.ScaleToFit, true, 0);

        GUILayout.Space(15);
    }

    #endregion

    #region ==> No Selection Error Message <==

    [PropertySpace(SpaceBefore = 10)]
    [OnInspectorGUI]
    [ShowIf("@selection == null && IsSelectionLocked == false")]
    [InfoBox("No Npc Selected. Please target a GameObject in the hierarchy with a 'NpcCreatorSelector' component attached to it to continue.", InfoMessageType.Error)]
    [GUIColor(1f, 0.5f, 0.5f, 1)]
    [PropertyOrder(-1)]
    public void ShowSelectionError() { }

    #endregion

    #region ==> Selection Made & Npc Selection Not Locked <==

    [ShowIf("@selection != null && IsSelectionLocked == false")]
    [DetailedInfoBox("Attach a Material into the 'Load Material' slot to apply a Material to be used for editing the Npc.", "Attach a Material into the 'Load Material' slot to apply a Material to be used for editing the Npc. If left blank, the Material currently on the Npc will be used.")]
    [PropertyOrder(-1)]
    [Title("Load Material")]
    [HideLabel]
    public Material LoadMaterial;

    [ShowIf("@selection != null && IsSelectionLocked == false")]
    [Button(ButtonSizes.Large, Name = "Customize NPC")]
    [GUIColor(0f, .75f, 0f, 1)]
    [PropertySpace]
    private void LockSelection()
    {
        ClearCurrentOptionsObjs();
        FindPartRoots();
        DisableAllParts();
        ChangeGender();
        FetchOptions();
        ApplySavedOptions();
        FetchLoadMaterial();
        ApplyMaterialToParts();
        BreakFromPrefab();

        NpcName = "";
        ChangeGender();
        IsSelectionLocked = true;
    }

    #endregion

    #region ==> Selection Made & Locked In <==

    [ShowIfGroup("IsSelectionLocked")]
    [VerticalGroup("IsSelectionLocked/Main")]

    #region --> Controls <--
    [HorizontalGroup("IsSelectionLocked/Main/Controls", Width = .1f, Order = -2)]
    [OnInspectorGUI]
    [GUIColor(1f, 0.35f, 0.35f, 1)]
    private void UnlockNPC()
    {
        if (GUILayout.Button(unlockNPCimg, GUILayout.Height(25), GUILayout.Width(25)))
        {
            selection = null;
            LoadMaterial = null;
            IsSelectionLocked = false;
        }
    }

    [HorizontalGroup("IsSelectionLocked/Main/Controls")]
    [GUIColor(0f, .75f, 0f, 1)]
    [Button(ButtonHeight = 25)]
    [LabelText("Clear Selections")]
    private void ClearOptionSelections()
    {
        Face = 1;
        ChangeFace();
        Ears = 0;
        ChangeEars();
        Hair = 0;
        ChangeHair();
        Eyebrows = 0;
        ChangeEyebrows();
        FacialHair = 0;
        ChangeFacialHair();
        Helmet = 0;
        ChangeHelmet();
        Cowl = 0;
        ChangeCowl();
        Hat = 0;
        ChangeHat();
        FaceGuard = 0;
        ChangeFaceGuard();
        FrontHeadAccessory = 0;
        ChangeFrontHeadAccessory();
        SideHeadAccessory = 0;
        ChangeSideHeadAccessory();
        BackHeadAccessory = 0;
        ChangeBackHeadAccessory();
        BothShoulder = 0;
        ChangeShoulderBoth();
        Chest = 0;
        ChangeChest();
        Cape = 0;
        ChangeCape();
        Waist = 0;
        ChangeWaist();
        BeltAccessory = 0;
        ChangeBeltAccessory();
        BothSleeve = 0;
        ChangeSleeveBoth();
        BothElbowpads = 0;
        ChangeElbowpadBoth();
        BothWrists = 0;
        ChangeWristBoth();
        BothGloves = 0;
        ChangeGloveBoth();
        BothKneepads = 0;
        ChangeKneepadBoth();
        BothBoots = 0;
        ChangeBootBoth();

    }
    #endregion

    #region -- Spacer Between Controls and Tabs --
    [VerticalGroup("IsSelectionLocked/Main/Spacer", Order = -1)]
    [OnInspectorGUI]
    private void Spacer() { GUILayout.Space(15); }
    #endregion

    [HorizontalGroup("IsSelectionLocked/Main/Options")]

    #region --> Appearance Tab <--
    [TabGroup("IsSelectionLocked/Main/Options/Tabs", "Appearance")]

    #region -- Spacer Between Tabs and Option Drop-Downs --
    [VerticalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Spacer", Order = -2)]
    [OnInspectorGUI]
    private void SpacerAppearanceTab() { GUILayout.Space(5); }
    #endregion

    #region -> Facial Features Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures", 1, 0, 0)]

    [DisableIf("HasHelmet")]
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Face", .1f, 0f, 0f, ShowLabel = false)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Face/Info")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeFace")]
    [PropertyRange(1, "@HeadCount()")]
    public int Face;

    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures")]
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Ears", .1f, 0f, 0f, ShowLabel = false)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Ears/Info")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeEars")]
    [PropertyRange(0, "@EarCount()")]
    public int Ears;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Colors", 1f, 0.25f, 0.25f, ShowLabel = false)]
    [ColorPalette("@getRace(skintext)")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    [OnValueChanged("ChangeSkinColor")]
    public Color SkinColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Colors")]
    [ColorPalette("@getRace(eyetext)")]
    [GUIColor(1f, .8f, 0.8f, 1)]
    [OnValueChanged("ChangeEyeColor")]
    public Color EyeColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Colors")]
    [ColorPalette("@getRace(scartext)")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    [OnValueChanged("ChangeScarColor")]
    public Color ScarColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Colors")]
    [ColorPalette("Body Art Colors")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    [OnValueChanged("ChangeBodyArtColor")]
    public Color BodyArtColor;

    #region -- Increment/Decrement Arrows
    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Face/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FaceDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Face > 1)
            {
                Face--;
                ChangeFace();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Face/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FaceIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Face < HeadCount())
            {
                Face++;
                ChangeFace();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Ears/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void EarsDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Ears > 0)
            {
                Ears--;
                ChangeEars();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/FacialFeatures/Ears/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void EarsIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Ears < EarCount())
            {
                Ears++;
                ChangeEars();
            }
        }
    }

    #endregion

    #endregion

    #region -> Gender Drop-Down <-
    public enum RaceEnum
    {
        Human,
        Goblin,
        Fae,
        Elf,
        Undead,
        HalfElf
    }

    private int Gender;

    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Gender", 0, 1f, 0f, Order = -1)]

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Gender/Choices", Order = -1)]
    [Button(ButtonSizes.Large, Name = "Male")]
    [GUIColor(.4f, .8f, 1, 1)]
    private void MaleChoice()
    {
        Gender = 0;
        ChangeGender();
    }

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Gender/Choices")]
    [Button(ButtonSizes.Large, Name = "Female")]
    [GUIColor(.95f, .6f, .85f, 1)]
    private void FenaleChoice()
    {
        Gender = 1;
        ChangeGender();
    }

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Gender/Race", 0, 0.1f, 0, ShowLabel = false)]
    [EnumPaging]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public RaceEnum Race;
    #endregion

    #region -> Hair Drop-Down <-

    [DisableIf("HasHelmet")]
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair", 0, 0, 1)]
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/HeadHair", 0, 0, 0.1f, ShowLabel = false)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/HeadHair/Info")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeHair")]
    [PropertyRange(0, "@HairCount()")]
    public int Hair;

    [DisableIf("@IsMale() == false || HasHelmet() == true")]
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/FacialHair", 0, 0, 0.1f, ShowLabel = false)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/FacialHair/Info")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeFacialHair")]
    [PropertyRange(0, "@FacialHairCount()")]
    public int FacialHair;

    [DisableIf("HasHelmet")]
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Eyebrows", 0, 0, 0.1f, ShowLabel = false)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Eyebrows/Info")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeEyebrows")]
    [PropertyRange(0, "@EyebrowCount()")]
    public int Eyebrows;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Colors", 0.25f, 0.25f, 1, ShowLabel = false)]
    [ColorPalette("@getRace(hairtext)")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    [OnValueChanged("ChangeHairColor")]
    public Color HairColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Colors")]
    [ColorPalette("@getRace(hairtext)")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    [OnValueChanged("ChangeStubbleColor")]
    public Color StubbleColor;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Colors/Split", Width = 0.45f)]
    [OnInspectorGUI]
    private void Split() { }

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Colors/Split")]
    [GUIColor(0.8f, 0.8f, 1)]
    [Button(Name = "None")]
    private void NoStubbleColor()
    {
        StubbleColor = SkinColor;
        ChangeStubbleColor();
    }

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Colors/Split")]
    [GUIColor(0.8f, 0.8f, 1)]
    [Button(Name = "Match Hair")]
    private void StubbleMatchHairColor()
    {
        StubbleColor = HairColor;
        ChangeStubbleColor();
    }

    #region ~~ Increment/Decrement Arrows
    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/HeadHair/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HairDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Hair > 0)
            {
                Hair--;
                ChangeHair();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/HeadHair/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HairIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Hair < HairCount())
            {
                Hair++;
                ChangeHair();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("@IsMale() == false || HasHelmet() == true")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/FacialHair/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FacialHairDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (FacialHair > 0)
            {
                FacialHair--;
                ChangeFacialHair();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("@IsMale() == false || HasHelmet() == true")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/FacialHair/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FacialHairIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (FacialHair < FacialHairCount())
            {
                FacialHair++;
                ChangeFacialHair();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Eyebrows/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void EyebrowsDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Eyebrows > 0)
            {
                Eyebrows--;
                ChangeEyebrows();
            }
        }
    }

    [OnInspectorGUI]
    [DisableIf("HasHelmet")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Appearance/Hair/Eyebrows/Info", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void EyebrowsIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Eyebrows < EyebrowCount())
            {
                Eyebrows++;
                ChangeEyebrows();
            }
        }
    }
    #endregion

    #endregion

    #endregion

    #region --> Gear Tab <--
    [TabGroup("IsSelectionLocked/Main/Options/Tabs", "Gear")]

    #region -- Spacer Between Tabs and Option Drop-Downs --
    [VerticalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Spacer", Order = -2)]
    [OnInspectorGUI]
    private void SpacerGearTab() { GUILayout.Space(5); }
    #endregion 

    #region -> Headwear Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head", 0, 0, 1)]

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear", 0, 0, .1f)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HelmetInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeHelmet")]
    [PropertyRange(0, "HelmetCount")]
    public int Helmet;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/CowlInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeCowl")]
    [PropertyRange(0, "CowlCount")]
    public int Cowl;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HatInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeHat")]
    [PropertyRange(0, "HatCount")]
    public int Hat;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/FaceGuardInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeFaceGuard")]
    [PropertyRange(0, "FaceGuardCount")]
    public int FaceGuard;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory", 0, 0, 0.1f)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/FrontInfo")]
    [LabelText("Front")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeFrontHeadAccessory")]
    [PropertyRange(0, "@FrontHeadAccessoryCount()")]
    public int FrontHeadAccessory;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/SideInfo")]
    [LabelText("Side")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeSideHeadAccessory")]
    [PropertyRange(0, "@SideHeadAccessoryCount()")]
    public int SideHeadAccessory;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/BackInfo")]
    [LabelText("Back")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeBackHeadAccessory")]
    [PropertyRange(0, "BackHeadAccessoryCount")]
    public int BackHeadAccessory;

    #region Increment/Decrement Arrows
    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HelmetInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HelmetDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Helmet > 0)
            {
                Helmet--;
                ChangeHelmet();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HelmetInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HelmetIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Helmet < HelmetCount())
            {
                Helmet++;
                ChangeHelmet();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/CowlInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void CowlDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Cowl > 0)
            {
                Cowl--;
                ChangeCowl();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/CowlInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void CowlIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Cowl < CowlCount())
            {
                Cowl++;
                ChangeCowl();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HatInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HatDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Hat > 0)
            {
                Hat--;
                ChangeHat();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/HatInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void HatIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Hat < HatCount())
            {
                Hat++;
                ChangeHat();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/FaceGuardInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FaceGuardDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (FaceGuard > 0)
            {
                FaceGuard--;
                ChangeFaceGuard();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/Headwear/FaceGuardInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FaceGuardIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (FaceGuard < FaceGuardCount())
            {
                FaceGuard++;
                ChangeFaceGuard();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/FrontInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FrontHeadAccessoryDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (FrontHeadAccessory > 0)
            {
                FrontHeadAccessory--;
                ChangeFrontHeadAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/FrontInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void FrontHeadAccessoryIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (FrontHeadAccessory < FrontHeadAccessoryCount())
            {
                FrontHeadAccessory++;
                ChangeFrontHeadAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/SideInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SideHeadAccessoryDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (SideHeadAccessory > 0)
            {
                SideHeadAccessory--;
                ChangeSideHeadAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/SideInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SideHeadAccessoryIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (SideHeadAccessory < SideHeadAccessoryCount())
            {
                SideHeadAccessory++;
                ChangeSideHeadAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/BackInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BackHeadAccessoryDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BackHeadAccessory > 0)
            {
                BackHeadAccessory--;
                ChangeBackHeadAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Head/HeadAccessory/BackInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BackHeadAccessoryIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BackHeadAccessory < BackHeadAccessoryCount())
            {
                BackHeadAccessory++;
                ChangeBackHeadAccessory();
            }
        }
    }
    #endregion

    #endregion

    #region -> Upper Body Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body", 1, 0.5f, 0)]

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads", .1f, 0.05f, 0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleShoulderBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchShoulders = true;

    [ShowIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeShoulderBoth")]
    [LabelText("Both")]
    [PropertyRange(0, "ShoulderCount")]
    public int BothShoulder;

    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeShoulderLeft")]
    [LabelText("Left")]
    [PropertyRange(0, "ShoulderCount")]
    public int LeftShoulder;

    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeShoulderRight")]
    [LabelText("Right")]
    [PropertyRange(0, "ShoulderCount")]
    public int RightShoulder;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody", .1f, 0.05f, 0)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/ChestInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeChest")]
    [PropertyRange(0, "ChestCount")]
    public int Chest;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/CapeInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeCape")]
    [PropertyRange(0, "CapeCount")]
    public int Cape;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody", .1f, 0.05f, 0)]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/WaistInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeWaist")]
    [PropertyRange(0, "WaistCount")]
    public int Waist;

    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/BeltAccessoryInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [OnValueChanged("ChangeBeltAccessory")]
    [PropertyRange(0, "BeltAccessoryCount")]
    public int BeltAccessory;

    #region Increment/Decrement Arrows

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/ChestInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ChestDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Chest > 0)
            {
                Chest--;
                ChangeChest();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/ChestInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ChestIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Chest < ChestCount())
            {
                Chest++;
                ChangeChest();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/CapeInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void CapeDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Cape > 0)
            {
                Cape--;
                ChangeCape();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/UpperBody/CapeInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void CapeIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Cape < CapeCount())
            {
                Cape++;
                ChangeCape();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/WaistInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WaistDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (Waist > 0)
            {
                Waist--;
                ChangeWaist();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/WaistInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WaistIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (Waist < WaistCount())
            {
                Waist++;
                ChangeWaist();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/BeltAccessoryInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BeltAccessoryDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BeltAccessory > 0)
            {
                BeltAccessory--;
                ChangeBeltAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/LowerBody/BeltAccessoryInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BeltAccessoryIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BeltAccessory < BeltAccessoryCount())
            {
                BeltAccessory++;
                ChangeBeltAccessory();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftShoulder > 0)
            {
                LeftShoulder--;
                ChangeShoulderLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftShoulder < ShoulderCount())
            {
                LeftShoulder++;
                ChangeShoulderLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightShoulder > 0)
            {
                RightShoulder--;
                ChangeShoulderRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightShoulder < ShoulderCount())
            {
                RightShoulder++;
                ChangeShoulderRight();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothShoulder > 0)
            {
                BothShoulder--;
                ChangeShoulderBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchShoulders")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Body/Shoulderpads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ShoulderBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothShoulder < ShoulderCount())
            {
                BothShoulder++;
                ChangeShoulderBoth();
            }
        }
    }
    #endregion

    #endregion

    #region -> Arms Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms", 0, 1, 0)]

    #region ~~ Sleeves ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves",0,.1f,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleSleeveBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchSleeves = true;

    [ShowIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeSleeveBoth")]
    [PropertyRange(0, "SleeveCount")]
    public int BothSleeve;

    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeSleeveLeft")]
    [PropertyRange(0, "SleeveCount")]
    public int LeftSleeve;

    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeSleeveRight")]
    [PropertyRange(0, "SleeveCount")]
    public int RightSleeve;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothSleeve > 0)
            {
                BothSleeve--;
                ChangeSleeveBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothSleeve < SleeveCount())
            {
                BothSleeve++;
                ChangeSleeveBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftSleeve > 0)
            {
                LeftSleeve--;
                ChangeSleeveLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftSleeve < SleeveCount())
            {
                LeftSleeve++;
                ChangeSleeveLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightSleeve > 0)
            {
                RightSleeve--;
                ChangeSleeveRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchSleeves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Sleeves/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void SleeveRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightSleeve < SleeveCount())
            {
                RightSleeve++;
                ChangeSleeveRight();
            }
        }
    }
    #endregion

    #endregion

    #region ~~ Elbowpads ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads",0,.1f,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleElbowpadBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchElbowpads = true;

    [ShowIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeElbowpadBoth")]
    [PropertyRange(0, "ElbowpadCount")]
    public int BothElbowpads;

    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeElbowpadLeft")]
    [PropertyRange(0, "ElbowpadCount")]
    public int LeftElbowpad;

    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeElbowpadRight")]
    [PropertyRange(0, "ElbowpadCount")]
    public int RightElbowpad;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothElbowpads > 0)
            {
                BothElbowpads--;
                ChangeElbowpadBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothElbowpads < ElbowpadCount())
            {
                BothElbowpads++;
                ChangeElbowpadBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftElbowpad > 0)
            {
                LeftElbowpad--;
                ChangeElbowpadLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftElbowpad < ElbowpadCount())
            {
                LeftElbowpad++;
                ChangeElbowpadLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightElbowpad > 0)
            {
                RightElbowpad--;
                ChangeElbowpadRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchElbowpads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Elbowpads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void ElbowpadRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightElbowpad < ElbowpadCount())
            {
                RightElbowpad++;
                ChangeElbowpadRight();
            }
        }
    }
    #endregion

    #endregion

    #region ~~ Wrists ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists",0,0.1f,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleWristsBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchWrists = true;

    [ShowIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeWristBoth")]
    [PropertyRange(0, "WristCount")]
    public int BothWrists;

    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeWristLeft")]
    [PropertyRange(0, "WristCount")]
    public int LeftWrist;

    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeWristRight")]
    [PropertyRange(0, "WristCount")]
    public int RightWrist;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothWrists > 0)
            {
                BothWrists--;
                ChangeWristBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothWrists < WristCount())
            {
                BothWrists++;
                ChangeWristBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftWrist > 0)
            {
                LeftWrist--;
                ChangeWristLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftWrist < WristCount())
            {
                LeftWrist++;
                ChangeWristLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightWrist > 0)
            {
                RightWrist--;
                ChangeWristRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchWrists")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Wrists/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void WristRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightWrist < WristCount())
            {
                RightWrist++;
                ChangeWristRight();
            }
        }
    }
    #endregion

    #endregion

    #region ~~ Gloves ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves",0,0.1f,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleGloveBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchGloves = true;

    [ShowIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeGloveBoth")]
    [PropertyRange(0, "GloveCount")]
    public int BothGloves;

    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeGloveLeft")]
    [PropertyRange(0, "GloveCount")]
    public int LeftGlove;

    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeGloveRight")]
    [PropertyRange(0, "GloveCount")]
    public int RightGlove;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothGloves > 0)
            {
                BothGloves--;
                ChangeGloveBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothGloves < GloveCount())
            {
                BothGloves++;
                ChangeGloveBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftGlove > 0)
            {
                LeftGlove--;
                ChangeGloveLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftGlove < GloveCount())
            {
                LeftGlove++;
                ChangeGloveLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightGlove > 0)
            {
                RightGlove--;
                ChangeGloveRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchGloves")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Arms/Gloves/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void GloveRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightGlove < GloveCount())
            {
                RightGlove++;
                ChangeGloveRight();
            }
        }
    }
    #endregion

    #endregion

    #endregion

    #region -> Legs Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs", 1, 0, 0)]

    #region ~~ Kneepads ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads",0.1f,0,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleKneepadBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchKneepads = true;

    [ShowIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeKneepadBoth")]
    [PropertyRange(0, "KneepadCount")]
    public int BothKneepads;

    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeKneepadLeft")]
    [PropertyRange(0, "KneepadCount")]
    public int LeftKneepad;

    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeKneepadRight")]
    [PropertyRange(0, "KneepadCount")]
    public int RightKneepad;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothKneepads > 0)
            {
                BothKneepads--;
                ChangeKneepadBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothKneepads < KneepadCount())
            {
                BothKneepads++;
                ChangeKneepadBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftKneepad > 0)
            {
                LeftKneepad--;
                ChangeKneepadLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftKneepad < KneepadCount())
            {
                LeftKneepad++;
                ChangeKneepadLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightKneepad > 0)
            {
                RightKneepad--;
                ChangeKneepadRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchKneepads")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Kneepads/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void KneepadRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightKneepad < KneepadCount())
            {
                RightKneepad++;
                ChangeKneepadRight();
            }
        }
    }
    #endregion

    #endregion

    #region ~~ Boots ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots",0.1f,0,0)]
    [LabelText("Match")]
    [OnValueChanged("ToggleBootBoth")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    public bool MatchBoots = true;

    [ShowIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/BothInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Both")]
    [OnValueChanged("ChangeBootBoth")]
    [PropertyRange(0, "BootCount")]
    public int BothBoots;

    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/LeftInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Left")]
    [OnValueChanged("ChangeBootLeft")]
    [PropertyRange(0, "BootCount")]
    public int LeftBoot;

    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/RightInfo")]
    [GUIColor(1f, 1f, 0.5f, 1)]
    [LabelText("Right")]
    [OnValueChanged("ChangeBootRight")]
    [PropertyRange(0, "BootCount")]
    public int RightBoot;

    #region ~~ Increment/Decrement Arrows ~~
    [OnInspectorGUI]
    [ShowIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootBothDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (BothBoots > 0)
            {
                BothBoots--;
                ChangeBootBoth();
            }
        }
    }

    [OnInspectorGUI]
    [ShowIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/BothInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootBothIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (BothBoots < BootCount())
            {
                BothBoots++;
                ChangeBootBoth();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootLeftDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (LeftBoot > 0)
            {
                LeftBoot--;
                ChangeBootLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/LeftInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootLeftIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (LeftBoot < BootCount())
            {
                LeftBoot++;
                ChangeBootLeft();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootRightDecrement()
    {
        if (SirenixEditorGUI.IconButton(leftArrow, GUI.skin.button, width: 22))
        {
            if (RightBoot > 0)
            {
                RightBoot--;
                ChangeBootRight();
            }
        }
    }

    [OnInspectorGUI]
    [HideIf("MatchBoots")]
    [HorizontalGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Legs/Boots/RightInfo", Width = 18)]
    [GUIColor(1f, 1f, 0.5f, 1)]
    private void BootRightIncrement()
    {
        if (SirenixEditorGUI.IconButton(rightArrow, GUI.skin.button, width: 22))
        {
            if (RightBoot < BootCount())
            {
                RightBoot++;
                ChangeBootRight();
            }
        }
    }
    #endregion

    #endregion

    #endregion

    #region -> Pigments Drop-Down <-
    [ColorFoldoutGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments",0.4f,0.4f,0.4f)]

    #region ~~ Cloth Colors ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Cloth",0.1f,0.1f,0.1f)]
    [LabelText("Primary")]
    [ColorPalette("Human Skin Colors")]
    [OnValueChanged("ChangePrimaryClothColor")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color PrimaryClothColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Cloth")]
    [LabelText("Secondary")]
    [OnValueChanged("ChangeSecondaryClothColor")]
    [ColorPalette("Human Skin Colors")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color SecondaryClothColor;
    #endregion

    #region ~~ Leather Colors ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Leather", 0.1f, 0.1f, 0.1f)]
    [LabelText("Primary")]
    [OnValueChanged("ChangePrimaryLeatherColor")]
    [ColorPalette("Human Skin Colors")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color PrimaryLeatherColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Leather")]
    [LabelText("Secondary")]
    [ColorPalette("Human Skin Colors")]
    [OnValueChanged("ChangeSecondaryLeatherColor")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color SecondaryLeatherColor;
    #endregion

    #region ~~ Metal Colors ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Metal", 0.1f, 0.1f, 0.1f)]
    [LabelText("Primary")]
    [ColorPalette("Human Skin Colors")]
    [OnValueChanged("ChangePrimaryMetalColor")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color PrimaryMetalColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Metal")]
    [LabelText("Secondary")]
    [ColorPalette("Human Skin Colors")]
    [OnValueChanged("ChangeSecondaryMetalColor")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color SecondaryMetalColor;

    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Metal")]
    [LabelText("Dark")]
    [ColorPalette("Human Skin Colors")]
    [OnValueChanged("ChangeDarkMetalColor")]
    [GUIColor(1f, .8f, 0.7f, 1)]
    public Color DarkMetalColor;
    #endregion

    #region ~~ Shader Sliders ~~
    [ColorBoxGroup("IsSelectionLocked/Main/Options/Tabs/Gear/Pigments/Emission", 1, 1, 1,ShowLabel = false)]
    [OnValueChanged("ChangeEmission")]
    [Range(0, 1)]
    public float Emission;

    #endregion

    #endregion

    #endregion

    #region --> Finalize Tab <--
    [TabGroup("IsSelectionLocked/Main/Options/Tabs", "Finalize")]

    #region -- Spacer Between Tabs and Option Drop-Downs --
    [VerticalGroup("IsSelectionLocked/Main/Options/Tabs/Finalize/Spacer", Order = -2)]
    [OnInspectorGUI]
    private void SpacerFinalizeTab() { GUILayout.Space(5); }
    #endregion
    
    [BoxGroup("IsSelectionLocked/Main/Options/Tabs/Finalize/Name", ShowLabel = false)]
    [InfoBox("NPC Name cannot be left blank",InfoMessageType.Error, "NpcNameIsBlank")]
    public string NpcName;

    [DisableIf("NpcNameIsBlank")]
    [TabGroup("IsSelectionLocked/Main/Options/Tabs", "Finalize")]
    [GUIColor("SaveButtonState")]
    [Button(ButtonSizes.Large, Name = "Save NPC")]
    private void SaveNpcPopUp()
    {
        GetWindow<NpcCreatorSaveWindow>(utility: true).Show();
    }


    #endregion

    #endregion

    #region ==> Selection Controller <==
    //Controller that updates the NPC Selection when a GameObject is selected in the Hierarchy
    //Changes the editor based on if the GameObject is an Npc that can be edited
    [OnInspectorGUI]
    private void OnSelectionChange()
    {
        if (Selection.activeGameObject && IsSelectionLocked == false)
        {
            if (Selection.activeGameObject.GetComponent<NpcCreatorSelector>())
            {
                selection = Selection.activeGameObject.GetComponent<NpcCreatorSelector>();
            }
            else
            {
                selection = null;
            }
        }
        else if (!Selection.activeGameObject && IsSelectionLocked == false)
        {
            selection = null;
        }
    }
    #endregion

    #region  ==> Editor Extension Methods/Variables <==

    //Variables used for input on getRace to change color choices
    private string skintext = "Skin Colors";
    private string eyetext = "Eye Colors";
    private string scartext = "Scar Colors";
    private string hairtext = "Hair Colors";

    //Bool Method used to hide options based on Gender
    private bool IsMale()
    {
        if (Gender == 0)
            return true;
        else return false;
    }

    //Bool Method used to change if the player has a helmet option on
    private bool HasHelmet()
    {
        if (Helmet != 0)
        {
            return true;
        }
        else return false;
    }

    //Bool Method used to check if the NPC Name on Finalize Tab is empty
    private bool NpcNameIsBlank()
    {
        if (NpcName == "")
        {
            return true;
        }
        else return false;
    }

    //Method to change the color choices on Appearance based on the Race option
    private string getRace(string input)
    {
        return Race.ToString() + " " + input;
    }

    private Color SaveButtonState()
    {
        if(NpcNameIsBlank() == false)
        {
            return new Color(0, 1, 0.25f, 1);
        }
        else
        {
            return Color.white;
        }
    }

    //Method used to break the NPC from prefab upon locking in the Selection
    private void BreakFromPrefab()
    {
        if (PrefabUtility.GetCorrespondingObjectFromSource(selection.gameObject) != null)
        {
            PrefabUtility.UnpackPrefabInstance(selection.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }
    }

    //Method to find a reference to the material currently on the Npc should Load Material be left empty
    //Also updates the color swatches in the editor window to the colors on the Material
    private void FetchLoadMaterial()
    {
        if(LoadMaterial == null)
        {
            LoadMaterial = AllParts.transform.Find("All_01_Hair").GetChild(1).GetComponent<Renderer>().sharedMaterial;
        }

        SkinColor = LoadMaterial.GetColor("_Color_Skin");
        HairColor = LoadMaterial.GetColor("_Color_Hair");
        EyeColor = LoadMaterial.GetColor("_Color_Eyes");
        StubbleColor = LoadMaterial.GetColor("_Color_Stubble");
        ScarColor = LoadMaterial.GetColor("_Color_Scar");
        BodyArtColor = LoadMaterial.GetColor("_Color_BodyArt");
        PrimaryClothColor = LoadMaterial.GetColor("_Color_Primary");
        SecondaryClothColor = LoadMaterial.GetColor("_Color_Secondary");
        PrimaryLeatherColor = LoadMaterial.GetColor("_Color_Leather_Primary");
        SecondaryLeatherColor = LoadMaterial.GetColor("_Color_Leather_Secondary");
        PrimaryMetalColor = LoadMaterial.GetColor("_Color_Metal_Primary");
        SecondaryMetalColor = LoadMaterial.GetColor("_Color_Metal_Secondary");
        DarkMetalColor = LoadMaterial.GetColor("_Color_Metal_Dark");
    }

    private void DisableAllParts()
    {
        foreach (Transform child in FemaleParts.transform)
        {
            if (child.childCount <= 0)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform child1 in child)
                {
                    if (child1.childCount <= 0)
                    {
                        child1.gameObject.SetActive(false);
                    }
                    else
                    {
                        foreach (Transform child2 in child1)
                            if (child2.childCount <= 0)
                            {
                                child2.gameObject.SetActive(false);
                            }
                            else
                            {
                            }
                    }

                }
            }
        }

        foreach (Transform child in MaleParts.transform)
        {
            if (child.childCount <= 0)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform child1 in child)
                {
                    if (child1.childCount <= 0)
                    {
                        child1.gameObject.SetActive(false);
                    }
                    else
                    {
                        foreach (Transform child2 in child1)
                            if (child2.childCount <= 0)
                            {
                                child2.gameObject.SetActive(false);
                            }
                            else
                            {
                            }
                    }

                }
            }
        }

        foreach (Transform child in AllParts.transform)
        {
            if (child.childCount <= 0)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform child1 in child)
                {
                    if (child1.childCount <= 0)
                    {
                        child1.gameObject.SetActive(false);
                    }
                    else
                    {
                        foreach (Transform child2 in child1)
                            if (child2.childCount <= 0)
                            {
                                child2.gameObject.SetActive(false);
                            }
                            else
                            {
                            }
                    }

                }
            }
        }
    }

    //Method used by to apply the Load Material to all parts on the NPC
    private void ApplyMaterialToParts()
    {
        if (LoadMaterial != null)
        {
            foreach (Transform child in FemaleParts.transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    child.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                }
                else
                {
                    foreach (Transform child1 in child)
                    {
                        if (child1.GetComponent<Renderer>())
                        {
                            child1.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                        }
                        else
                        {
                            foreach (Transform child2 in child1)
                                if (child2.GetComponent<Renderer>())
                                {
                                    child2.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                                }
                                else
                                {
                                }
                        }

                    }
                }
            }

            foreach (Transform child in MaleParts.transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    child.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                }
                else
                {
                    foreach (Transform child1 in child)
                    {
                        if (child1.GetComponent<Renderer>())
                        {
                            child1.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                        }
                        else
                        {
                            foreach (Transform child2 in child1)
                                if (child2.GetComponent<Renderer>())
                                {
                                    child2.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                                }
                                else
                                {
                                }
                        }

                    }
                }
            }

            foreach (Transform child in AllParts.transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    child.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                }
                else
                {
                    foreach (Transform child1 in child)
                    {
                        if (child1.GetComponent<Renderer>())
                        {
                            child1.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                        }
                        else
                        {
                            foreach (Transform child2 in child1)
                                if (child2.GetComponent<Renderer>())
                                {
                                    child2.gameObject.GetComponent<Renderer>().sharedMaterial = LoadMaterial;
                                }
                                else
                                {
                                }
                        }

                    }
                }
            }
        }
    }

    //Method to find the base GameObjects for use in generating list options
    private void FindPartRoots()
    {
        FemaleParts = selection.transform.Find("Modular_Characters").Find("Female_Parts").gameObject;
        MaleParts = selection.transform.Find("Modular_Characters").Find("Male_Parts").gameObject;
        AllParts = selection.transform.Find("Modular_Characters").Find("All_Gender_Parts").gameObject;
    }

    //Method to clear all the OptionOBJs
    private void ClearCurrentOptionsObjs()
    {
        HeadOBJ = null;
        EyebrowsOBJ = null;
        HairOBJ = null;
        FacialHairOBJ = null;
        GenderOBJ = null;
        HelmetOBJ = null;
        CowlOBJ = null;
        FaecGuardOBJ = null;
        HatOBJ = null;
        ChestOBJ = null;
        WaistOBJ = null;
        ShoulderLeftOBJ = null;
        ShoulderRightOBJ = null;
        SleeveLeftOBJ = null;
        SleeveRightOBJ = null;
        ElbowpadLeftOBJ = null;
        ElbowpadRightOBJ = null;
        WristRightOBJ = null;
        WristLeftOBJ = null;
        GloveLeftOBJ = null;
        GloveRightOBJ = null;
        KneepadLeftOBJ = null;
        KneepadRightOBJ = null;
        BootLeftOBJ = null;
        BootRightOBJ = null;
        CapeOBJ = null;
        BeltAccessoryOBJ = null;
        FrontHeadAccessoryOBJ = null;
        BackHeadAccessoryOBJ = null;
        SideHeadAccessoryOBJ = null;
}

    //Method to fetch the Options and populate the appropriate lists with them
    private void FetchOptions()
    {
        #region Clear Lists
            HeadList.Clear();
            EyebrowList.Clear();
            HairList.Clear();
            FacialHairList.Clear();
            HelmetList.Clear();
            CowlList.Clear();
            FaceGuardList.Clear();
            HatList.Clear();
            TorsoList.Clear();
            EarsList.Clear();
            WaistList.Clear();
            SleeveRightList.Clear();
            SleeveLeftList.Clear();
            WristRightList.Clear();
            WristLeftList.Clear();
            GloveLeftList.Clear();
            GloveRightList.Clear();
            BootLeftList.Clear();
            BootRightList.Clear();
            FrontHeadAccessoryList.Clear();
            SideHeadAccessoryList.Clear();
            BackHeadAccessoryList.Clear();
            CapeList.Clear();
            BeltAccessoryList.Clear();
            ElbowRightList.Clear();
            ElbowLeftList.Clear();
            KneeRightList.Clear();
            KneeLeftList.Clear();
            ShoulderLeftList.Clear();
            ShoulderRightList.Clear();
        #endregion

        foreach (Transform hairstyle in AllParts.transform.GetChild(1))
        {
            HairList.Add(hairstyle.gameObject);
        }

        foreach (Transform face in GenderOBJ.transform.GetChild(0).GetChild(0))
        {
            HeadList.Add(face.gameObject);
        }

        foreach (Transform eyebrow in GenderOBJ.transform.GetChild(1))
        {
            EyebrowList.Add(eyebrow.gameObject);
        }

        foreach (Transform facialHair in GenderOBJ.transform.GetChild(2))
        {
            FacialHairList.Add(facialHair.gameObject);
        }

        foreach (Transform ears in AllParts.transform.GetChild(12).GetChild(0))
        {
            EarsList.Add(ears.gameObject);
        }

        foreach (Transform helmet in GenderOBJ.transform.GetChild(0).GetChild(1))
        {
            HelmetList.Add(helmet.gameObject);
        }

        foreach (Transform hat in AllParts.transform.GetChild(0).GetChild(0))
        {
            HatList.Add(hat.gameObject);
        }

        foreach (Transform faceGuard in AllParts.transform.GetChild(0).GetChild(1))
        {
            FaceGuardList.Add(faceGuard.gameObject);
        }

        foreach (Transform cowl in AllParts.transform.GetChild(0).GetChild(2))
        {
            CowlList.Add(cowl.gameObject);
        }

        foreach (Transform torso in GenderOBJ.transform.GetChild(3))
        {
            TorsoList.Add(torso.gameObject);
        }

        foreach (Transform waist in GenderOBJ.transform.GetChild(10))
        {
            WaistList.Add(waist.gameObject);
        }

        foreach (Transform sleeve in GenderOBJ.transform.GetChild(4))
        {
            SleeveRightList.Add(sleeve.gameObject);
        }

        foreach (Transform sleeve in GenderOBJ.transform.GetChild(5))
        {
            SleeveLeftList.Add(sleeve.gameObject);
        }

        foreach (Transform wrist in GenderOBJ.transform.GetChild(6))
        {
            WristRightList.Add(wrist.gameObject);
        }

        foreach (Transform wrist in GenderOBJ.transform.GetChild(7))
        {
            WristLeftList.Add(wrist.gameObject);
        }

        foreach (Transform glove in GenderOBJ.transform.GetChild(8))
        {
            GloveRightList.Add(glove.gameObject);
        }

        foreach (Transform glove in GenderOBJ.transform.GetChild(9))
        {
            GloveLeftList.Add(glove.gameObject);
        }

        foreach (Transform boot in GenderOBJ.transform.GetChild(11))
        {
            BootRightList.Add(boot.gameObject);
        }

        foreach (Transform boot in GenderOBJ.transform.GetChild(12))
        {
            BootLeftList.Add(boot.gameObject);
        }

        foreach (Transform headAccessory in AllParts.transform.GetChild(2).GetChild(1))
        {
            FrontHeadAccessoryList.Add(headAccessory.gameObject);
        }

        foreach (Transform headAccessory in AllParts.transform.GetChild(2).GetChild(2))
        {
            SideHeadAccessoryList.Add(headAccessory.gameObject);
        }

        foreach (Transform headAccessory in AllParts.transform.GetChild(2).GetChild(3))
        {
            BackHeadAccessoryList.Add(headAccessory.gameObject);
        }

        foreach (Transform cape in AllParts.transform.GetChild(4))
        {
            CapeList.Add(cape.gameObject);
        }

        foreach (Transform shoulderPad in AllParts.transform.GetChild(5))
        {
            ShoulderRightList.Add(shoulderPad.gameObject);
        }

        foreach (Transform shoulderPad in AllParts.transform.GetChild(6))
        {
            ShoulderLeftList.Add(shoulderPad.gameObject);
        }

        foreach (Transform elbowPad in AllParts.transform.GetChild(7))
        {
            ElbowRightList.Add(elbowPad.gameObject);
        }

        foreach (Transform elbowPad in AllParts.transform.GetChild(8))
        {
            ElbowLeftList.Add(elbowPad.gameObject);
        }

        foreach (Transform beltAccessory in AllParts.transform.GetChild(9))
        {
            BeltAccessoryList.Add(beltAccessory.gameObject);
        }

        foreach (Transform kneePad in AllParts.transform.GetChild(10))
        {
            KneeRightList.Add(kneePad.gameObject);
        }

        foreach (Transform kneePad in AllParts.transform.GetChild(11))
        {
            KneeLeftList.Add(kneePad.gameObject);
        }
    }

    //Method to apply option changes based on an Enum of Option Type and an ID to select from a list of options
    private void ApplyOptions(Options type, int id)
    {
        switch(type)
        {
            case Options.Gender:
                {
                    if (Gender == 0)
                    {
                        GenderOBJ = MaleParts;
                    }
                    else
                    {
                        GenderOBJ = FemaleParts;
                    }

                    if (IsSelectionLocked == true)
                    {
                        FetchOptions();

                        ChangeFace();
                        ChangeEyebrows();
                        ChangeChest();
                        ChangeSleeveLeft();
                        ChangeSleeveRight();
                        ChangeWristLeft();
                        ChangeWristRight();
                        ChangeGloveLeft();
                        ChangeGloveRight();
                        ChangeWaist();
                        ChangeBootLeft();
                        ChangeBootRight();
                        if (Gender == 0)
                        {
                            ChangeFacialHair();
                        }
                        else
                        {
                            ApplyOptions(Options.FacialHair, 0);
                        }

                        selection.SavedGenderIndex = id;
                    }
                    break;
                }

            case Options.Hair:
                {
                    if (HairOBJ != null)
                    {
                        HairOBJ.SetActive(false);
                    }

                    HairOBJ = HairList[id];
                    HairOBJ.SetActive(true);

                    selection.SavedHairIndex = id;
                    break;
                }

            case Options.Head:
                {
                    if (HeadOBJ != null)
                    {
                        HeadOBJ.SetActive(false);
                    }
                    
                    if ( Face > HeadList.Count - 1)
                    {
                        Face = HeadList.Count - 1;
                        id = Face;
                    }

                    HeadOBJ = HeadList[id];
                    HeadOBJ.SetActive(true);

                    selection.SavedHeadIndex = id;
                    break;
                }

            case Options.Eyebrows:
                {
                    if (EyebrowsOBJ != null)
                    {
                        EyebrowsOBJ.SetActive(false);
                    }
                    
                    if (Eyebrows > EyebrowList.Count - 1)
                    {
                        Eyebrows = EyebrowList.Count - 1;
                    }
                    
                    EyebrowsOBJ = EyebrowList[id];
                    EyebrowsOBJ.SetActive(true);

                    selection.SavedEyebrowsIndex = id;
                    break;
                }

            case Options.FacialHair:
                {
                    if (FacialHairOBJ != null)
                    {
                        FacialHairOBJ.SetActive(false);
                    }

                    if (FacialHairList.Count == 0)
                    {
                        FacialHair = 0;
                    }
                    
                    FacialHairOBJ = FacialHairList[id];
                    FacialHairOBJ.SetActive(true);

                    selection.SavedFacialHairIndex = id;
                    break;
                }

            case Options.Ears:
                {
                    if (EarOBJ != null)
                    {
                        EarOBJ.SetActive(false);
                    }
                    
                    EarOBJ = EarsList[id];
                    EarOBJ.SetActive(true);

                    selection.SavedEarsIndex = id;
                    break;
                }

            case Options.Helmet:
                {
                    if(Helmet != 0)
                    {
                        ApplyOptions(Options.Head, 0);
                        ApplyOptions(Options.Eyebrows, 0);
                        ApplyOptions(Options.Hair, 0);
                        ApplyOptions(Options.FacialHair, 0);
                    }
                    else
                    {
                        CheckHeadwearStatus();
                    }

                    if (HelmetOBJ != null)
                    {
                        HelmetOBJ.SetActive(false);
                    }

                    HelmetOBJ = HelmetList[id];
                    HelmetOBJ.SetActive(true);

                    selection.SavedHelmetIndex = id;
                    break;
                }

            case Options.Cowl:
                {
                    if (Cowl != 0)
                    {
                        ApplyOptions(Options.Hair, 0);
                    }
                    else
                    {
                        CheckHeadwearStatus();
                    }

                    if (CowlOBJ != null)
                    {
                        CowlOBJ.SetActive(false);
                    }

                    CowlOBJ = CowlList[id];
                    CowlOBJ.SetActive(true);

                    selection.SavedNoHairHelmetIndex = id;
                    break;
                }

            case Options.Hat:
                {
                    if (Hat == 1)
                    {
                        ApplyOptions(Options.Hair, 0);
                    }
                    else
                    {
                        CheckHeadwearStatus();
                    }

                    if (HatOBJ != null)
                    {
                        HatOBJ.SetActive(false);
                    }

                    HatOBJ = HatList[id];
                    HatOBJ.SetActive(true);

                    selection.SavedHatIndex = id;
                    break;
                }

            case Options.Face_Guard:
                {
                    if (FaceGuard > 2)
                    {
                        ApplyOptions(Options.FacialHair, 0);
                    }
                    else
                    {
                        CheckHeadwearStatus();
                    }

                    if (FaecGuardOBJ != null)
                    {
                        FaecGuardOBJ.SetActive(false);
                    }

                    FaecGuardOBJ = FaceGuardList[id];
                    FaecGuardOBJ.SetActive(true);

                    selection.SavedNoBeardHelmetIndex = id;
                    break;
                }

            case Options.FrontHead_Accessory:
                {
                    if (FrontHeadAccessoryOBJ != null)
                    {
                        FrontHeadAccessoryOBJ.SetActive(false);
                    }

                    FrontHeadAccessoryOBJ = FrontHeadAccessoryList[id];
                    FrontHeadAccessoryOBJ.SetActive(true);

                    selection.SavedFrontHead_AttachmentIndex = id;
                    break;
                }

            case Options.SideHead_Accessory:
                {
                    if (SideHeadAccessoryOBJ != null)
                    {
                        SideHeadAccessoryOBJ.SetActive(false);
                    }

                    SideHeadAccessoryOBJ = SideHeadAccessoryList[id];
                    SideHeadAccessoryOBJ.SetActive(true);

                    selection.SavedSideHead_AttachmentIndex = id;
                    break;
                }

            case Options.BackHead_Accessory:
                {
                    if (BackHeadAccessoryOBJ != null)
                    {
                        BackHeadAccessoryOBJ.SetActive(false);
                    }

                    BackHeadAccessoryOBJ = BackHeadAccessoryList[id];
                    BackHeadAccessoryOBJ.SetActive(true);

                    selection.SavedBackHead_AttachmentIndex = id;
                    break;
                }

            case Options.Chest:
                {
                    if (ChestOBJ != null)
                    {
                        ChestOBJ.SetActive(false);
                    }

                    ChestOBJ = TorsoList[id];
                    ChestOBJ.SetActive(true);

                    selection.SavedTorsoIndex = id;
                    break;
                }

            case Options.Waist:
                {
                    if (WaistOBJ != null)
                    {
                        WaistOBJ.SetActive(false);
                    }

                    WaistOBJ = WaistList[id];
                    WaistOBJ.SetActive(true);

                    selection.SavedHipsIndex = id;
                    break;
                }

            case Options.Shoulder_Right:
                {
                    if (ShoulderRightOBJ != null)
                    {
                        ShoulderRightOBJ.SetActive(false);
                    }

                    ShoulderRightOBJ = ShoulderRightList[id];
                    ShoulderRightOBJ.SetActive(true);

                    selection.SavedShoulder_RightIndex = id;
                    break;
                }

            case Options.Shoulder_Left:
                {
                    if (ShoulderLeftOBJ != null)
                    {
                        ShoulderLeftOBJ.SetActive(false);
                    }

                    ShoulderLeftOBJ = ShoulderLeftList[id];
                    ShoulderLeftOBJ.SetActive(true);

                    selection.SavedShoulder_LeftIndex = id;
                    break;
                }

            case Options.Wrist_Left:
                {
                    if (WristLeftOBJ != null)
                    {
                        WristLeftOBJ.SetActive(false);
                    }

                    WristLeftOBJ = WristLeftList[id];
                    WristLeftOBJ.SetActive(true);

                    selection.SavedArm_Lower_LeftIndex = id;
                    break;
                }

            case Options.Wrist_Right:
                {
                    if (WristRightOBJ != null)
                    {
                        WristRightOBJ.SetActive(false);
                    }

                    WristRightOBJ = WristRightList[id];
                    WristRightOBJ.SetActive(true);

                    selection.SavedArm_Lower_RightIndex = id;
                    break;
                }

            case Options.Sleeve_Right:
                {
                    if (SleeveRightOBJ != null)
                    {
                        SleeveRightOBJ.SetActive(false);
                    }

                    SleeveRightOBJ = SleeveRightList[id];
                    SleeveRightOBJ.SetActive(true);

                    selection.SavedArm_Upper_RightIndex = id;
                    break;
                }

            case Options.Sleeve_Left:
                {
                    if (SleeveLeftOBJ != null)
                    {
                        SleeveLeftOBJ.SetActive(false);
                    }

                    SleeveLeftOBJ = SleeveLeftList[id];
                    SleeveLeftOBJ.SetActive(true);

                    selection.SavedArm_Upper_LeftIndex = id;
                    break;
                }

            case Options.Glove_Right:
                {
                    if (GloveRightOBJ != null)
                    {
                        GloveRightOBJ.SetActive(false);
                    }

                    GloveRightOBJ = GloveRightList[id];
                    GloveRightOBJ.SetActive(true);

                    selection.SavedHand_RightIndex = id;
                    break;
                }

            case Options.Glove_Left:
                {
                    if (GloveLeftOBJ != null)
                    {
                        GloveLeftOBJ.SetActive(false);
                    }

                    GloveLeftOBJ = GloveLeftList[id];
                    GloveLeftOBJ.SetActive(true);

                    selection.SavedHand_LeftIndex = id;
                    break;
                }

            case Options.Elbow_Left:
                {
                    if (ElbowpadLeftOBJ != null)
                    {
                        ElbowpadLeftOBJ.SetActive(false);
                    }

                    ElbowpadLeftOBJ = ElbowLeftList[id];
                    ElbowpadLeftOBJ.SetActive(true);

                    selection.SavedElbow_LeftIndex = id;
                    break;
                }

            case Options.Elbow_Right:
                {
                    if (ElbowpadRightOBJ != null)
                    {
                        ElbowpadRightOBJ.SetActive(false);
                    }

                    ElbowpadRightOBJ = ElbowRightList[id];
                    ElbowpadRightOBJ.SetActive(true);

                    selection.SavedElbow_RightIndex = id;
                    break;
                }

            case Options.Cape:
                {
                    if (CapeOBJ != null)
                    {
                        CapeOBJ.SetActive(false);
                    }

                    CapeOBJ = CapeList[id];
                    CapeOBJ.SetActive(true);

                    selection.SavedBack_AttachmentIndex = id;
                    break;
                }

            case Options.Belt_Accessory:
                {
                    if (BeltAccessoryOBJ != null)
                    {
                        BeltAccessoryOBJ.SetActive(false);
                    }

                    BeltAccessoryOBJ = BeltAccessoryList[id];
                    BeltAccessoryOBJ.SetActive(true);

                    selection.SavedHip_AttachmentIndex = id;
                    break;
                }

            case Options.Knee_Left:
                {
                    if (KneepadLeftOBJ != null)
                    {
                        KneepadLeftOBJ.SetActive(false);
                    }

                    KneepadLeftOBJ = KneeLeftList[id];
                    KneepadLeftOBJ.SetActive(true);

                    selection.SavedKnee_LeftIndex = id;
                    break;
                }

            case Options.Knee_Right:
                {
                    if (KneepadRightOBJ != null)
                    {
                        KneepadRightOBJ.SetActive(false);
                    }

                    KneepadRightOBJ = KneeRightList[id];
                    KneepadRightOBJ.SetActive(true);

                    selection.SavedKnee_RightIndex = id;
                    break;
                }
                
            case Options.Boot_Left:
                {
                    if (BootLeftOBJ != null)
                    {
                        BootLeftOBJ.SetActive(false);
                    }

                    BootLeftOBJ = BootLeftList[id];
                    BootLeftOBJ.SetActive(true);

                    selection.SavedLeg_LeftIndex = id;
                    break;
                }

            case Options.Boot_Right:
                {
                    if (BootRightOBJ != null)
                    {
                        BootRightOBJ.SetActive(false);
                    }

                    BootRightOBJ = BootRightList[id];
                    BootRightOBJ.SetActive(true);

                    selection.SavedLeg_RightIndex = id;
                    break;
                }
        }
    }

    //Method to get the saved option IDs from NPC Creator Selector script and apply the options to the current Npc
    //Only executes if the NPC has been edited before by the NPC Creator
    private void ApplySavedOptions()
    {
        Hair = selection.SavedHairIndex;
        ApplyOptions(Options.Hair, Hair);

        Face = selection.SavedHeadIndex;
        ApplyOptions(Options.Head, Face);

        Eyebrows = selection.SavedEyebrowsIndex;
        ApplyOptions(Options.Eyebrows, Eyebrows);

        if (Gender == 0)
        {
            FacialHair = selection.SavedFacialHairIndex;
            ApplyOptions(Options.FacialHair, FacialHair);
        }
        else
        {
            FacialHair = 0;
            ApplyOptions(Options.FacialHair, FacialHair);
        }

        Ears = selection.SavedEarsIndex;
        ApplyOptions(Options.Ears, Ears);

        Chest = selection.SavedTorsoIndex;
        ApplyOptions(Options.Chest, Chest);

        Waist = selection.SavedHipsIndex;
        ApplyOptions(Options.Waist, Waist);

        Helmet = selection.SavedHelmetIndex;
        ApplyOptions(Options.Helmet, Helmet);

        Cowl = selection.SavedNoHairHelmetIndex;
        ApplyOptions(Options.Cowl, Cowl);

        Hat = selection.SavedHatIndex;
        ApplyOptions(Options.Hat, Hat);

        FaceGuard = selection.SavedNoBeardHelmetIndex;
        ApplyOptions(Options.Face_Guard, FaceGuard);

        FrontHeadAccessory = selection.SavedFrontHead_AttachmentIndex;
        ApplyOptions(Options.FrontHead_Accessory, FrontHeadAccessory);

        SideHeadAccessory = selection.SavedSideHead_AttachmentIndex;
        ApplyOptions(Options.SideHead_Accessory, SideHeadAccessory);

        BackHeadAccessory = selection.SavedBackHead_AttachmentIndex;
        ApplyOptions(Options.BackHead_Accessory, BackHeadAccessory);

        LeftShoulder = selection.SavedShoulder_LeftIndex;
        ApplyOptions(Options.Shoulder_Left, LeftShoulder);

        RightShoulder = selection.SavedShoulder_RightIndex;
        ApplyOptions(Options.Shoulder_Right, RightShoulder);

        LeftSleeve = selection.SavedArm_Upper_LeftIndex;
        ApplyOptions(Options.Sleeve_Left, LeftSleeve);

        RightSleeve = selection.SavedArm_Upper_RightIndex;
        ApplyOptions(Options.Sleeve_Right, RightSleeve);

        LeftWrist = selection.SavedArm_Lower_LeftIndex;
        ApplyOptions(Options.Wrist_Left, LeftWrist);

        RightWrist = selection.SavedArm_Lower_RightIndex;
        ApplyOptions(Options.Wrist_Right, RightWrist);

        LeftGlove = selection.SavedHand_LeftIndex;
        ApplyOptions(Options.Glove_Left, LeftGlove);

        RightGlove = selection.SavedHand_RightIndex;
        ApplyOptions(Options.Glove_Right, RightGlove);

        LeftBoot = selection.SavedLeg_LeftIndex;
        ApplyOptions(Options.Boot_Left, LeftBoot);

        RightBoot = selection.SavedLeg_RightIndex;
        ApplyOptions(Options.Boot_Right, RightBoot);

        LeftElbowpad = selection.SavedElbow_LeftIndex;
        ApplyOptions(Options.Elbow_Left, LeftElbowpad);

        RightElbowpad = selection.SavedElbow_RightIndex;
        ApplyOptions(Options.Elbow_Right, RightElbowpad);

        Cape = selection.SavedBack_AttachmentIndex;
        ApplyOptions(Options.Cape, Cape);

        BeltAccessory = selection.SavedHip_AttachmentIndex;
        ApplyOptions(Options.Belt_Accessory, BeltAccessory);

        RightKneepad = selection.SavedKnee_RightIndex;
        ApplyOptions(Options.Knee_Right, RightKneepad);

        LeftKneepad = selection.SavedKnee_LeftIndex;
        ApplyOptions(Options.Knee_Left, LeftKneepad);

        LeftBoot = selection.SavedLeg_LeftIndex;
        ApplyOptions(Options.Boot_Left, LeftBoot);

        RightBoot = selection.SavedLeg_RightIndex;
        ApplyOptions(Options.Boot_Right, RightBoot);
    }

    //Method used to revert the Hair, Eyebrows, Hair, and Face options to their original values based on Headwear
    private void CheckHeadwearStatus()
    {
        if(Helmet == 0)
        {
            ChangeFace();
            ChangeEyebrows();
            
        }

        if(Helmet == 0 && Cowl == 0 && Hat != 1)
        {
            ChangeHair();
        }

        if(FaceGuard < 3)
        {
            ChangeFacialHair();
        }
    }

    //Methods used by OnValueChanged to update Options
    #region -- Change Options Methods --

    #region ~~> Appearance Options <~~
    private void ChangeGender() { ApplyOptions(Options.Gender, Gender); }

    private void ChangeFace()
    {
        if(Face == 0)
        {
            Face = 1;
        }
        ApplyOptions(Options.Head, Face);
    }
    private void ChangeEars() { ApplyOptions(Options.Ears, Ears); }

    private void ChangeHair() { ApplyOptions(Options.Hair, Hair); }
    private void ChangeFacialHair() { ApplyOptions(Options.FacialHair, FacialHair); }
    private void ChangeEyebrows() { ApplyOptions(Options.Eyebrows, Eyebrows); }
    
    private void ChangeSkinColor()
    {
        if (StubbleColor == LoadMaterial.GetColor("_Color_Skin"))
        {
            StubbleColor = SkinColor;
            ChangeStubbleColor();
        }
        LoadMaterial.SetColor("_Color_Skin", SkinColor);
    }
    private void ChangeEyeColor() { LoadMaterial.SetColor("_Color_Eyes", EyeColor); }
    private void ChangeScarColor() { LoadMaterial.SetColor("_Color_Scar", ScarColor); }
    private void ChangeBodyArtColor() { LoadMaterial.SetColor("_Color_BodyArt", BodyArtColor); }

    private void ChangeHairColor()
    {
        if(StubbleColor == LoadMaterial.GetColor("_Color_Hair"))
        {
            StubbleColor = HairColor;
            ChangeStubbleColor();
        }
        LoadMaterial.SetColor("_Color_Hair", HairColor);
    }
    private void ChangeStubbleColor() { LoadMaterial.SetColor("_Color_Stubble", StubbleColor); }
    #endregion

    #region ~~> Gear Options <~~
    private void ChangeHelmet() { ApplyOptions(Options.Helmet, Helmet); }
    private void ChangeCowl() { ApplyOptions(Options.Cowl, Cowl); }
    private void ChangeHat() { ApplyOptions(Options.Hat, Hat); }
    private void ChangeFaceGuard() { ApplyOptions(Options.Face_Guard, FaceGuard); }

    private void ChangeFrontHeadAccessory() { ApplyOptions(Options.FrontHead_Accessory, FrontHeadAccessory); }
    private void ChangeSideHeadAccessory() { ApplyOptions(Options.SideHead_Accessory, SideHeadAccessory); }
    private void ChangeBackHeadAccessory() { ApplyOptions(Options.BackHead_Accessory, BackHeadAccessory); }

    private void ChangeChest() { ApplyOptions(Options.Chest, Chest); }
    private void ChangeWaist() { ApplyOptions(Options.Waist, Waist); }
    private void ChangeCape() { ApplyOptions(Options.Cape, Cape); }
    private void ChangeBeltAccessory() { ApplyOptions(Options.Belt_Accessory, BeltAccessory); }

    private void ChangeShoulderLeft() { ApplyOptions(Options.Shoulder_Left, LeftShoulder); }
    private void ChangeShoulderRight() { ApplyOptions(Options.Shoulder_Right, RightShoulder); }
    private void ChangeShoulderBoth()
    {
        LeftShoulder = BothShoulder;
        RightShoulder = BothShoulder;
        ApplyOptions(Options.Shoulder_Right, RightShoulder);
        ApplyOptions(Options.Shoulder_Left, LeftShoulder);
    }

    private void ChangeWristRight() { ApplyOptions(Options.Wrist_Right, RightWrist); }
    private void ChangeWristLeft() { ApplyOptions(Options.Wrist_Left, LeftWrist); }
    private void ChangeWristBoth()
    {
        LeftWrist = BothWrists;
        RightWrist = BothWrists;
        ApplyOptions(Options.Wrist_Left, LeftWrist);
        ApplyOptions(Options.Wrist_Right, RightWrist);
    }

    private void ChangeSleeveLeft() { ApplyOptions(Options.Sleeve_Left, LeftSleeve); }
    private void ChangeSleeveRight() { ApplyOptions(Options.Sleeve_Right, RightSleeve); }
    private void ChangeSleeveBoth()
    {
        LeftSleeve = BothSleeve;
        RightSleeve = BothSleeve;
        ApplyOptions(Options.Sleeve_Right, RightSleeve);
        ApplyOptions(Options.Sleeve_Left, LeftSleeve);
    }

    private void ChangeElbowpadLeft() { ApplyOptions(Options.Elbow_Left, LeftElbowpad); }
    private void ChangeElbowpadRight() { ApplyOptions(Options.Elbow_Right, RightElbowpad); }
    private void ChangeElbowpadBoth()
    {
        LeftElbowpad = BothElbowpads;
        RightElbowpad = BothElbowpads;
        ApplyOptions(Options.Elbow_Left, LeftElbowpad);
        ApplyOptions(Options.Elbow_Right, RightElbowpad);
    }

    private void ChangeGloveLeft() { ApplyOptions(Options.Glove_Left, LeftGlove); }
    private void ChangeGloveRight() { ApplyOptions(Options.Glove_Right, RightGlove); }
    private void ChangeGloveBoth()
    {
        LeftGlove = BothGloves;
        RightGlove = BothGloves;
        ApplyOptions(Options.Glove_Right, RightGlove);
        ApplyOptions(Options.Glove_Left, LeftGlove);
    }

    private void ChangeKneepadLeft() { ApplyOptions(Options.Knee_Left, LeftKneepad); }
    private void ChangeKneepadRight() { ApplyOptions(Options.Knee_Right, RightKneepad); }
    private void ChangeKneepadBoth()
    {
        LeftKneepad = BothKneepads;
        RightKneepad = BothKneepads;
        ApplyOptions(Options.Knee_Right, RightKneepad);
        ApplyOptions(Options.Knee_Left, LeftKneepad);
    }

    private void ChangeBootLeft() { ApplyOptions(Options.Boot_Left, LeftBoot); }
    private void ChangeBootRight() { ApplyOptions(Options.Boot_Right, RightBoot); }
    private void ChangeBootBoth()
    {
        LeftBoot = BothBoots;
        RightBoot = BothBoots;
        ApplyOptions(Options.Boot_Left, LeftBoot);
        ApplyOptions(Options.Boot_Right, RightBoot);
    }

    private void ToggleShoulderBoth()
    {
        if (MatchShoulders == true)
        {
            BothShoulder = LeftShoulder;
            ChangeShoulderBoth();
        }
    }
    private void ToggleWristsBoth()
    {
        if (MatchWrists == true)
        {
            BothWrists = LeftWrist;
            ChangeWristBoth();
        }
    }
    private void ToggleSleeveBoth()
    {
        if(MatchSleeves == true)
        {
            BothSleeve = LeftSleeve;
            ChangeSleeveBoth();
        }
    }
    private void ToggleGloveBoth()
    {
        BothGloves = LeftGlove;
        ChangeGloveBoth();
    }
    private void ToggleElbowpadBoth()
    {
        BothElbowpads = LeftElbowpad;
        ChangeElbowpadBoth();
    }
    private void ToggleKneepadBoth()
    {
        BothKneepads = LeftKneepad;
        ChangeKneepadBoth();
    }
    private void ToggleBootBoth()
    {
        BothBoots = LeftBoot;
        ChangeBootBoth();
    }

    private void ChangePrimaryClothColor() { LoadMaterial.SetColor("_Color_Primary", PrimaryClothColor); }
    private void ChangeSecondaryClothColor() { LoadMaterial.SetColor("_Color_Secondary", SecondaryClothColor); }
    private void ChangePrimaryLeatherColor() { LoadMaterial.SetColor("_Color_Leather_Primary", PrimaryLeatherColor); }
    private void ChangeSecondaryLeatherColor() { LoadMaterial.SetColor("_Color_Leather_Secondary", SecondaryLeatherColor); }
    private void ChangePrimaryMetalColor() { LoadMaterial.SetColor("_Color_Metal_Primary", PrimaryMetalColor); }
    private void ChangeSecondaryMetalColor() { LoadMaterial.SetColor("_Color_Metal_Secondary", SecondaryMetalColor); }
    private void ChangeDarkMetalColor() { LoadMaterial.SetColor("_Color_Metal_Dark", DarkMetalColor); }
    private void ChangeEmission() { LoadMaterial.SetFloat("_Emission",Emission); }
    #endregion

    #endregion

    //Method used by Sliders to get the count of options in lists
    #region -- Grab List Counts Methods --
    private int HairCount() { return HairList.Count - 1; }

    private int HeadCount() { return HeadList.Count - 1; }
    
    private int EyebrowCount()
    {
        if (EyebrowList.Count != 0)
            return EyebrowList.Count - 1;

        else
            return 0;
    }
    
    private int FacialHairCount()
    {
        if (FacialHairList.Count != 0)
            return FacialHairList.Count - 1;

        else
            return 0;
    }

    private int EarCount() { return EarsList.Count - 1; }

    private int HelmetCount() { return HelmetList.Count - 1;  }

    private int CowlCount() { return CowlList.Count - 1; }

    private int HatCount() { return HatList.Count - 1; }

    private int FaceGuardCount() { return FaceGuardList.Count - 1; }

    private int FrontHeadAccessoryCount() { return FrontHeadAccessoryList.Count - 1; }

    private int SideHeadAccessoryCount() { return SideHeadAccessoryList.Count - 1; }

    private int BackHeadAccessoryCount() { return BackHeadAccessoryList.Count - 1; }

    private int ChestCount(){ return TorsoList.Count - 1; }

    private int WaistCount() { return WaistList.Count - 1; }

    private int ShoulderCount() { return ShoulderLeftList.Count - 1; }

    private int SleeveCount() { return SleeveLeftList.Count - 1; }

    private int ElbowpadCount() { return ElbowLeftList.Count - 1; }

    private int WristCount() { return WristLeftList.Count - 1; }

    private int GloveCount() { return GloveLeftList.Count - 1; }

    private int KneepadCount() { return KneeLeftList.Count - 1; }

    private int BootCount() { return BootLeftList.Count - 1; }

    private int CapeCount() { return CapeList.Count - 1; }

    private int BeltAccessoryCount() { return BeltAccessoryList.Count - 1; }

    #endregion

    //Methods used to save the NPC in various ways
    #region -- Save Methods --
    private string NpcSavePath = "Assets/Npcs/";

    public void SaveInstance()
    {
        selection.name = NpcName;
        if (GameObject.Find("NPCS"))
        {
            selection.transform.SetParent(GameObject.Find("NPCS").transform);
        }
        else
        {
            var npcsGo = new GameObject("NPCS");
            selection.transform.SetParent(npcsGo.transform);
        }
        LoadMaterial = null;
        IsSelectionLocked = false;
        selection = null;
    }

    public void SaveAsNewPrefab()
    {
        selection.name = NpcName;
        if (GameObject.Find("NPCS"))
        {
            selection.transform.SetParent(GameObject.Find("NPCS").transform);
        }
        else
        {
            var npcsGo = new GameObject("NPCS");
            selection.transform.SetParent(npcsGo.transform);
        }
        SavePrefab(NpcName);
        LoadMaterial = null;
        IsSelectionLocked = false;
        selection = null;
    }

    public void SaveAsOverwrite()
    {
        selection.name = NpcName;
        if (GameObject.Find("NPCS"))
        {
            selection.transform.SetParent(GameObject.Find("NPCS").transform);
        }
        else
        {
            var npcsGo = new GameObject("NPCS");
            selection.transform.SetParent(npcsGo.transform);
        }
        PrefabUtility.SaveAsPrefabAssetAndConnect(selection.gameObject, NpcSavePath + NpcName + ".prefab", InteractionMode.AutomatedAction);
        LoadMaterial = null;
        IsSelectionLocked = false;
        selection = null;
    }

    private void SavePrefab(string name)
    {
        string savename = name;
        int variantint = 0;

        TryToSavePrefab:

        if (AssetDatabase.LoadAssetAtPath(NpcSavePath + savename + ".prefab", typeof(GameObject)))
        {
            variantint++;
            savename = name + "0" + variantint.ToString();
            goto TryToSavePrefab;
        }
        else
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(selection.gameObject, NpcSavePath + savename + ".prefab", InteractionMode.AutomatedAction);
        }
    }

    public bool CheckForExistingPrefab()
    {
        if (AssetDatabase.LoadAssetAtPath(NpcSavePath + NpcName + ".prefab", typeof(GameObject)))
            return true;
        else return false;
    }
    #endregion

    #endregion

    //Set the Selection to Null and grab the references to GUI Imgs used in the edtior
    protected override void OnEnable()
    {
        base.OnEnable();
        IsSelectionLocked = false;
        LoadMaterial = null;

        unlockNPCimg = Resources.Load("SYNTY_Lock") as Texture2D;
        editorBanner = Resources.Load("SYNTY_NpcCreator_Banner") as Texture2D;
    }
}