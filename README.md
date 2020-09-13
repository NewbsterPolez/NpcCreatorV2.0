# NpcCreatorV2.0
An Odin Inspector based Npc Editor for Synty's Modular Fantasy Hero asset
--Created and Owned by NewbsterPolez--
===========================================================================

Requirements:
- Odin Inspector
- Synty: Modular Fantasy Characters

---------------------------------------------------------

Set-Up:

1.) Open up your Unity Project and ensure you have both 'Odin Inspector' and 'Synty's Modular Fantasy Characters' assets imported into your project. The tool will not work without these assets so be sure you have them before you add this editor.

2.) Create a new folder in your project with a name of your choosing, then right-click the newly created folder and select 'Show In Explorer' from the pop-up menu.

3.) Drag-and-drop the Commons folder, Editor folder, Odin Custom Attibutes folder and NpcCreatorSelector script into the new folder.

4.) Open the Editor folder in the Unity Project and then open the NpcCreator script. Locate the region ' ==> Editor Extension Methods/Variables <==' and expand it. Within it, find the region ' -- Save Methods -- ', then expand that.

There is a private string NpcSavePath variable there. Adjust the string value to whereever you would like your NPC Prefabs to be saved to. I suggest making a new folder somewhere titled: Npcs, then setting the string to that path. Make sure the path ends with a '/' like so:

Assets/MyProject/Npcs/

Then save the script. Make sure the path already exists, so if folders need to be made, be sure you do that before trying to save an Npc. The script nor Unity won't auto generate the folders for the path.

5.) Open the location where you want your Npcs to save to and drag-and-drop the Template folder into it. This is a modified Prefab of the Modular Fantasy Characters asset so be sure you use this to make any Npcs. The Template folder location is not required to be in any specific folder, so feel free to move it around to a place you will be able to find it easily for yourself.

6.) Open the prefab in Unity and ensure the Base parent GameObject has the componenet 'NpcCreatorSelector'. If it is missing, add it and save the prefab.

There you go, all set up. Drag in the Template NPC prefab into the scene, select it in the hierarchy, locate the drop-down 'Tools' at the top bar in Unity, Go to Synty-> NPC Creator. A new window will dock itself next to your Inspector, switch over to it and Customize your new npcs.

---------------------------------------------------------------------------------------------------------------------

Additional Info:

- The Load Material slot is used to apply a material to every part of the Modular Character. You should find the material for the Modular Characters and duplicate it using Shift+D while selecting it in the Project. I purposefully didn't have the Npc Creator generate materials for you as you may want to use the same material for multiple npcs to cut down on space, like for having guards in a city who share a color palette but just have different armor on.

- You can also change the race enums to suit your needs. Open the Npc Creator script and expand the region '==> Selection Made & Locked In <==', then expand the region 
'--> Appearance Tab <--' and then the region '-> Gender Drop-Down'. In there you will find Public enum RaceEnum. Modify the choices as you wish, ensuring you seperate each selection with a , then save. When you open the Npc Creator, your race options will pop up. Selecting a race will prompt you to create new color palettes for each color options like Skin, Eyes, Hair, Stubble. From there you can edit the palette of each race individually, in the case that your game has pre-defined characteristics for each race.
