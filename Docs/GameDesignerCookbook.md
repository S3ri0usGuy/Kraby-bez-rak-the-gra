# An Ultimate Guide on How to Cook Quests and Dialogues (without using code)

![image](https://github.com/user-attachments/assets/ea64c03f-ae5a-4d6d-a73a-882bc9b5844e)

- [Terminology](#terminology)
- [Clock](#clock)
- [Interactable Objects](#interactable-objects)
- [Dialogues](#dialogues)
  - [Overview](#overview)
  - [Dialogue Nodes Guide](#dialogue-nodes-guide)
    - [Dialogues Folder Structure (IMPORTANT!)](#dialogues-folder-structure-important)
    - [Creating The Nodes](#creating-the-nodes)
      - [Dialogue Groups (skippable for the game ending dialogue)](#1-dialogue-groups-skippable-for-the-game-ending-dialogue)
      - [Dialogue Nodes](#2-dialogue-nodes)
      - [Dialogue Phrases](#3-dialogue-phrases)
      - [Dialogue Answer Options](#4-dialogue-answer-options)
      - [Dialogue Next Node Branching](#5-dialogue-next-node-branching)
    - [Dialogue Conditions](#dialogue-conditions)
      - [Dialogue Condition Creation](#dialogue-condition-creation)
      - [Dialogue Conditions Overview](#dialogue-conditions-overview)
    - [Dialogue Save Events](#dialogue-save-events)
    - [Localized Phrases](#localized-phrases)
      - [Creating a New Localization Table for Dialogue Phrases](#creating-a-new-localization-table-for-dialogue-phrases)
      - [Using an Already Existing Localization Table for Dialogue Phrases](#using-an-already-existing-localization-table-for-dialogue-phrases)
    - [Localized Voice Lines](#localized-voice-lines)
  - [Dialogue NPCs](#dialogue-npcs)
    - [Creating an NPC with Dialogues](#creating-an-npc-with-dialogues)
    - [Dialogue Node Triggers](#dialogue-node-triggers)
- [Quests](#quests)
	- [Quest System](#quest-system)
	- [Quest/Subquest States](#questsubquest-states)
	- [Creating Quests](#creating-quests)
- [Actions](#actions)
  - [List of Actions](#list-of-actions)
- [Triggers](#triggers)

**IMPORTANT:** when you're editing the files (such as quest or dialogues), remember to always press `File -> Save Project` before committing, because Ctrl + S is not enough!

## Terminology

- **Entity** - An active in-game object that is not a player. Examples: NPCs, narration screen.

- **Narration Screen** - a black screen with subtitles that is used to demonstrate the player that something has happened offscreen.

- **Dialogue Node** - A single step in a conversation. Contains one or more phrases, optional answer options, and optional branching logic.

- **Dialogue Phrase** - An individual line of dialogue spoken by a character. Includes speaker index, localized text, optional voice line, duration, and skippability.

- **Dialogue Answer Option** - A choice presented to the player after the last phrase in a node. Each option can lead to another node and be shown conditionally.

- **Dialogue Branching** - Automatic logic that determines the next node. Uses condition-based branches or falls back to a default.

- **Dialogue Condition** - A rule or requirement used in branching or options to decide whether something should be shown or skipped (e.g., "Quest X was Completed").

- **Dialogue Sequence** - A chain of dialogue nodes played in order. It starts at a selected node and continues until a node is reached that has no options — that ends the sequence.

- **Dialogue Group** - A file that is created for each character that contains multiple dialogue nodes. It's populated automatically. It is used by the save system to easily retrieve dialogue nodes depending on the saved data.

- **Trigger** - A component that "fires" in response to some in-game event (e.g., quest is completed, some time has passed). Can be attached to nodes or options. They are usually used with **actions**.

- **Action** - A component that usually has one method - `Perform`. This method is called by other components, especially **triggers**. The actions are meant to interact with in-game objects: play audio, start/complete/fail quests, save state.

- **Interactable Object** - An object that has a trigger collider assigned to it and a component that ends with the `Interactable` suffix. Such objects show a prompt similiar to "Press X to do Y" 

## Clock

The game counts time in minutes. The starting number of minutes can be edited inside the `Player` prefab (use the `t:Prefab Player` search prompt in the Project window):

![image](https://github.com/user-attachments/assets/18815167-e4c5-4a8d-9328-9607cbd82827)

Minutes can be spent using the `SpendTimeAction` which can be triggered by dialogues or quests.

## Interactable Objects

Interactable objects are objects with a trigger collider and a component that ends with the `Interactable` suffix. Such objects show a prompt similiar to "Press X to do Y". Currently, there are two "Interactable" components:

1. `EventInteractable` - a component that was used for the exit trigger in the alpha version. It's the most basic interactable object as it just triggers the "Event" when the player interacts with it:
   ![image](https://github.com/user-attachments/assets/38bc1341-5eab-47d9-80d9-8170a677c6e6)
2. `DialogueInteractable` - a special interactable component that is used to start dialogues. This component is present in the `DialogueNPC` prefab and it's assigned to the `EntryTrigger` component, so all NPCs with dialogues have it. Try to not think about it that much, [the less you know the better](https://www.youtube.com/watch?v=2SUwOgmvzK4). Just remember that it's your responsibility to give this trigger a proper action name, position and collider.

Each interactable object has the "Action Name" which is shown when the player enters the trigger. The action names are stored in the `General` localization table.

Almost all of interactable objects (objects that show you the "Press X to do Y" text) do not work if the number of minutes left is 0. If you want for some interactable object to always work, uncheck this checkbox:

![image](https://github.com/user-attachments/assets/664a0576-7c32-4d18-8256-a3f1220e64c8)

## Dialogues

### Overview

So, the entire dialogue system can be divided into 5 main parts:
1. **Data** - how dialogues are saved.
    - Dialogue groups
        - Nodes
            - Phrases
                - Localized text
                - Voice lines
                - Duration
            - Answer options
                - Localized title
                - [Branching]
            - Answering behavior
            - [Branching]
2. **Branching** - how dialogues are sequenced and which phrases/answers should appear when.
    - Branching
        - Default next node
        - Branches
            - Conditions
            - Next node
3. **Control** - "guts" that you sould not be interested in (how resources are loaded, how they are saved, how dialogue states work).
4. **Triggers** - customizable actions that can be used to interacts with the world (quests, UI, etc.).
5. **View** - user interface, animations and camera view. The animations can be played using the triggers.

The tricky part is that the data and branching are stored in files (scriptable objects), and the triggers and view are defined by the objects on the scene (components). This division may make no sense to you, but it exists for the one main reason: it's more modular and minimizes the amount of interconnected components, which prevents developer errors + it's easier to maintain.

### Dialogue Nodes Guide
Dialogue Nodes are the key elements of the dialogue system. They define what phrases (lines) are said, what dialogue nodes are next and what options the player has when answering. Each object that has a dialogue attached to it must have at least one node.

#### Dialogues Folder Structure (IMPORTANT!)
Dialogue Nodes must be located in the dedicated folder. Your responsibility is to create such folders for each character, or other objects (like the game ending scree, which is also a "dialogue"!). So, the structure looks more or less like this:

```
Assets/
  └── Dialogues/
      ├── Alpha/                        # The game ending dialogue that was used in the alpha version
      ├── Characters/                   # Character-specific dialogues
      │   ├── Main/                     # Main characters' dialogues (e.g. protagonist, core NPCs)
      │   └── Side/                     # Side characters, extras, or optional interactions
      ├── Shared/                       # Reusable assets across dialogues
      │   └── Conditions/               # Global conditions that can be used for any dialogue
      └── Test/                         # Dialogues and extras that were/will be used for testing
```

If you want to create dialogue nodes for a side character, just put them in the `Assets/Dialogues/Characters/Side/[CharacterName]` folder (don't use spaces and non-English letters for the character names, use PascalCase). For the main characters this folder is: `Assets/Dialogues/Characters/Main/[CharacterName]`. If you are creating something more interesting, like the game ending screen just create a new folder in the `Assets/Dialogues`. 

Tip: you can create subfolders for the logical groups of nodes, it can make your life a bit easier.

Following this convention (especially for the characters) is very important, because otherwise some systems may not work properly, and it will be easier for others to modify dialogues.

#### Creating The Nodes
##### 1. **Dialogue Groups** (skippable for the game ending dialogue)

For the NPCs, each dialogue node must belong to the dialogue node group (it's the saving system requirement). So, when making dialogue nodes for characters, in the root character folder (e.g. `Assets/Dialogues/Characters/Main/Clemence`) create the `Dialogue Nodes Group`:

![image](https://github.com/user-attachments/assets/240e56c0-b78e-4b33-a80a-3a5be5421e1d)

And, that's all for the node groups! Your only requirement from now is to you put all your nodes inside this folder (or its subfolders). Do not try to update the
node groups manually, as they populate themselves automatically.

**Important:** Please, give the groups proper names, preferably in this format: `[CharacterName]DialogueGroup`.

##### 2. **Dialogue Nodes**

So you've created the node group, your next step is to create your first node. It's done in the same way as groups, but you just need to choose a different option:

![image](https://github.com/user-attachments/assets/e7f082a4-e2c2-4e6d-a691-0e8566094a64)

Once you've created a node, it should look like this:

![image](https://github.com/user-attachments/assets/fa5b6947-edab-4a1b-87e4-d94d688177d4)

**Important:** Please, give the nodes proper names, preferably in this format: `[CharacterName]_Dlg[SequenceNo]` for the "parent node" of the dialogue sequence and `[CharacterName]_Dlg[SequenceNo]_[EntryName]`. Examples: `Michele_Dlg1`, `Michele_Dlg1_Yes`.

##### 3. **Dialogue Phrases**

**Tip:** hover your mouse on the properties - this will show you tooltips.

In order to add a phrase, just press the "+" button:

![image](https://github.com/user-attachments/assets/5a46e6dc-7eee-45ff-8606-900b0d4677f3)

The phrases will be played at the same order as in this array. Of course, you're free to reorder and remove them however you want.

Now, let's see how an average phrase looks like:

![image](https://github.com/user-attachments/assets/a523c1f3-71df-4535-8f90-678181548f5d)

1. **Speaker Index** - an index of the speaker who says this phrase. Usually, it's 0 for the protagonist, 1 for the NPC, 2 for the narrator (depends on the `DialoguePlayer` settings). This index is required for the systems (subtitles and audio players) to differentiate who says what.
2. **Text** - a localized text of the phrase that is displayed in subtitles. See the [Localized Phrases](#localized-phrases) section for more details.
3. **Default Audio Clip** (optional) - an audio clip that will be played during the phrase if the **Audio Clip** is not assigned.
4. **Audio Clip** (optional) - a localized audio clip that will be played during the phrase. See the [Localized Voice Lines](#localized-voice-lines) section for more details.
5. **Duration** - how long the phrase lasts (in seconds). If the audio clip (either **Default Audio Clip** or **Audio Clip**) is assigned, and this value is zero or negative, the clip's duration is taken instead.
6. **Unskippable** - if checked, the phrase will not be skippable; otherwise the player can press any key to skip the phrase.
7. **Subtitles Profile** - the subtitles profile that will be used for this phrase. Can be left as "None", then the default one will be used.

##### 4. **Dialogue Answer Options**

Now take a look at the answer options.

![image](https://github.com/user-attachments/assets/d153f3f7-8b49-40d9-8dd0-c2048ed76c01)

You can see two options: yes and no. Their parameters are:

1. **Text** - a localized option text that will be displayed to the player. Such string must be put either in the default Dialogue table or in the table that is specific to the character (same principle as with the [Localized Phrases](#localized-phrases)).
2. **Branching** - the branching logic which is quite similiar to the next node branching (see the next section). But there is a main difference: where there is no node available (this means that no node is assigned as the default one and others are locked by conditions) the option will not be shown to the player.

Answering the option leads to the dialogue sequence transitioning to the first available node in the branching (the default one comes last).

If you don't specify any answers or if all answers will be locked behind the branching, the player won't be shown a prompt to answer. This ends the dialogue sequence, which leads to the the current NPC node (the node that will be played next) being updated based on the next node branching (see the next section).

Optionally, you can add a time restriction to the node.

![image](https://github.com/user-attachments/assets/9a48bc03-4bb7-45c7-9999-4f00381101a6)

* **Default** - the player has no time limits to answer.
* **TimedPickFirst** - the player's time to answer is limited (by "Time To Answer" seconds). If time is out, the first option is picked.
* **TimedPickRandom** - the player's time to answer is limited (by "Time To Answer" seconds). If time is out, a random option is picked.

**Important**: the limit of the answer options is 4. If you have more than 4 answer options available, only the first 4 will be displayed (and you'll see a warning in the console).

##### 5. **Dialogue Next Node Branching**

Now, the most complex and interesting stuff - branching. So, each object with dialogues has a one node saved as "current". This node changes either when the player chooses some answer, or when the dialogue sequence ends.

![image](https://github.com/user-attachments/assets/4bc0c2dc-7855-49b0-87e0-5df9d9657341)

- **Default Next Node** - a node that will be selected if there is no branch available.
- **Branches** - a collection of potential next nodes. Each is checked in the same order you put them there. The first one with all conditions met is selected.
    - **Node** - a node in a branch that will be selected if all conditions are satisfied and if it comes first.
    - **Conditions** - a list of conditions that must be satisfied in order for the node to be selected. More info in the [Dialogue Conditions](#dialogue-conditions) section. Note that **ALL** conditions from the list must be satisfied for the node to be selected.

The selected node is set as a current for the dialogue object. If it's not assigned (None), the node doesn't change and remains the same.

**Pro-tip:** if a node is empty (no phrases and answers), the player won't be able to start the dialogue. But, the empty node can still have branching which will be updated each time the player enters the "dialogue start zone"! So, you can use empty nodes to temporary disable dialogues. There is also an already existing empty node that you can use to permanently disable the dialogues: `DialogueEmptyNode` (just don't edit it!).

#### Dialogue Conditions

Dialogue Conditions are files that can be combined in the branches in order to create more complex dialogue graphs. They are located in:
- `Assets/Dialogues/Shared/Conditions` folder. It contains conditions that can be reused by many objects.
- Folders specific to entities (more info [here](#dialogues-folder-structure-important)).

##### Dialogue Condition Creation

In order to create a dialogue condition follow these steps:

1. Make sure that you're located in the appropriate folder (`Assets/Dialogues/Shared/Conditions` or a folder specific for the dialogue entity).
2. Right click and inside the `Create -> Dialogue -> Conditions` choose a condition that you want to add.

![image](https://github.com/user-attachments/assets/3af35f6a-4371-4946-b4f3-003b9c2c0665)

3. Name the condition so it's easily recognizable. If it's a condition that is specific to an entity, put the entity name at the beginning of the name (e.g. `ClemenceRomanceCondition`).
4. You're done! Now use it in the branching however you want.

#### Dialogue Conditions Overview

- **Quest** - conditions related to quests.
    - **Quest State** (`DialogueQuestStateCondition`) - a condition that is satisfied when the specified quest has exactly the same state as defined by the "State" property.
  
    ![image](https://github.com/user-attachments/assets/ed73cd29-d900-4d06-825d-dee2b7d1d7a0)

    - **Subquest State** (`DialogueSubquestStateCondition`) - exactly same as the **Quest State** one, but for subquests.
- **Random** (`DialogueRandomCondition`) - a condition that has a probability to be satisfied. It's completely random and a chance to be satisfied is defined by the "Probability" property (from 0 to 1: 0 meaning always failure, 1 meaning always success). This condition is not recommended to be actually used and was added to test the conditions feature before other conditions existed.
  
  ![image](https://github.com/user-attachments/assets/2cfe7433-4b2c-4e07-9d6f-3009a8b4fc94)
  
- **Save Event** (`DialogueSaveEventCondition`) - a condition that is satisfied if the specified save event (defined by the "Event" property) has the target state (defined by the "Target State" property).
  
  ![image](https://github.com/user-attachments/assets/b6a7c272-32c7-4046-92b2-a1f8d3e2fe6e)
  
- **Time Left** (`DialogueTimeLeftCondition`) - a condition that is satisfied only if the minutes left pass the comparison. It uses this formula to check if it's satisfied: `minutesLeft (Comparison) targetMinutesLeft`.
  
    ![image](https://github.com/user-attachments/assets/37f60a59-ed14-42fd-b03b-d9a5afea80ae)
    
#### Dialogue Save Events

Save Events are basically flags with two states: "exists" and "not exists". By default, all save events have the "not exists" state. Save Events can be created by choosing these options: `Create -> Dialogues -> Save Event`.

To manipulate the state of events use the `DialogueSaveEventAction` component. To make dialogues responsive to the save events, use the `DialogueSaveEventCondition` condition.

#### Localized Phrases

All text phrases are localized. This means, that you must create localization table entries for each phrase. My recommandation is to create a separate localization tables for main characters and other characters that may have huge amount of dialogues. For others, use the default one: `Assets/Localization/Tables/Dialogues/Dialogues` (please, keep phrases of the same character close to each other).

##### **Creating a New Localization Table for Dialogue Phrases**

It's advised to create a new localization table per each main character. For the side characters you can reuse the default one.

1. Open the **Localization Tables** window (`Window -> Asset Management -> Localization Tables`).
   
	![image](https://github.com/user-attachments/assets/0a527b99-5472-458a-8912-ef20d6d91909)

3. Press `+ New Table Collection` (top left corner).
   
	![image](https://github.com/user-attachments/assets/b6627df0-43f1-4d61-9e8b-3c1a521ef99b)

5. Name it in this pattern: `[EntityName]Dialogues`, make sure that all locales are selected and the type is set to "String Table Collection".
   
	![image](https://github.com/user-attachments/assets/83d703b8-4514-4d81-befb-648c95618e74)

7. Smash the `Create` button.
8. Select an appropriate folder. It must follow this convention: `Assets/Dialogues/.../[EntityCharacterName]/Localization/Text` (more about it #dialogues-folder-structure-important). Example: `Assets/Dialogues/Characters/Main/Demo/Localization/Text`.
9. If you see this window then you probably did everything OK so far. Next steps are optional but recommended.
   
	![image](https://github.com/user-attachments/assets/0b922843-b7bf-4c72-b915-2368a60eb816)

7 Editing dialogues in the `Localization Tables` is a next level torture, because this window was programmed by apes. So, in order to make yourself a life easier (not that easier tbh) you can add a CSV extension to the table. Just click the "+" button and select the "Csv Extension":

![image](https://github.com/user-attachments/assets/0c0c2774-f1be-4d8c-bc91-a3a9a1faaf45)

8. Then smash the `Save` button and select the `[Project]/Localization/Dialogues` folder. **IMPORTANT:** this folder is not located in the `Assets` folder, it's directly inside the root project directory.

![image](https://github.com/user-attachments/assets/95baadad-ceeb-4a54-bf13-87c716b8fcb7)

9. Open this CSV file in an editor of your choice (e.g. Libre Office Calc). Note that Excel sucks for this, because it has problems with different delimiters, but it's probably still usable with the right settings (don't even ask me, I don't know how to configure this shit).

10. Enjoy the soulless spreadsheats!

![image](https://github.com/user-attachments/assets/d64abb62-3c1a-418c-a4a4-50f89a16f19c)

##### **Using an Already Existing Localization Table for Dialogue Phrases**

1. Select the localization table file in the `Project` window (it looks like a file explorer). Tip: use `t:StringTableCollection` search prompt to find all localization tables.
2. Select the `Dialogues` file and in the `Inspector` smash the `Open in Table Editor` button and edit phrases there.
   2.1. OR, in your file explorer (not in Unity!) go to the `/Localization` folder (inside the root folder of the project).
   2.2. Find the corresponding CSV file, open it in your favourite editor and edit it there. **Note:** you should not add IDs manually when adding new entries. Instead, just add a new row, then enter the key and translations.
   2.3. After editing, save the file and press the `Import` button.
   
   ![image](https://github.com/user-attachments/assets/9632ea82-676c-4366-8387-aecd849d8562)
   
   2.4 Press the `Export` button if you have added new entries to update IDs.
   
   ![image](https://github.com/user-attachments/assets/5519223f-c716-4cf0-9632-df26e0934c6e)

3. For new entries use this pattern: `[entityName*]_[node]_[speaker]` (* - omit the entityName if the table is unique for it). Examples: `dlg1_yes4_narrator` (`ClemenceDialogues` table, specific for the NPC), `jacques_dlg1_4_jacques` (NPC's name is specified because this phrase is inside the general `Dialogue` table)
      
#### Localized Voice Lines

Voice lines use similiar tables as the text phrases. These tables must be used only if the voice lines use speech, otherwise they are redundant.

Like the text phrases, voice lines have its own default table `VoiceLines` that is located in `Assets/Localization/Voice` folder. Rule of thumb: if the entity has its own dialogues table, you must create a corresponding voice lines table to it.

In order to create a new voice lines table you do the same steps as [here](#creating-a-new-localization-table-for-dialogue-phrases), but in step #3 you must select "Asset Table Collection" instead of "String Table Collection" and in step #5 instead of putting the table it in the `Assets/Dialogues/.../[EntityCharacterName]/Localization/Text` folder, you must put it inside this folder: `Assets/Dialogues/.../[EntityCharacterName]/Localization/Voice`. Also, these tables do not support extensions so ignore other steps after #6.

Editing such tables is pretty straightforward.

### Dialogue NPCs

Okay, you now can create dialogues, but how to add them to the NPCs?

#### Creating an NPC with Dialogues

All NPCs must be prefabs that are placed in the `Assets/Prefabs/NPC` folder. Also, these prefabs must be variants of the `DialogueNPC` prefabs as it makes refactoring waaaay easier.

1. Find the `DialogueNPC` prefab (look in `Assets/Prefabs/NPC` or just use prompt `t:Prefab DialogueNPC`) and drag it to the scene.

    ![image](https://github.com/user-attachments/assets/41954cb2-4cea-4822-89d3-21347b9182bb)

2. Drag and drop this newly created object in the `Assets/Prefabs/NPC/Side` or `Assets/Prefabs/NPC/Main` folder. When asked to create "Prefab or Variant" choose "Prefab Variant".

   ![image](https://github.com/user-attachments/assets/d3ad4674-073e-4576-bfb3-51d3da6a4c42)

3. Rename the file to a character name immediately! Make sure to get rid of the "Variant" thing in the name, because it's cringe. Also, don't forget to rename the object on the scene too.

4. Open this prefab by right clicking it on the scene and choosing `Prefab -> Open in Context`, then edit it in this window and try to not edit the NPC outside of it. If you want to edit this object directly on the scene remember these things:
   - Your changes will be saved only on the scene. If you do so, update the prefab regulary by dragging and droping object from the scene direction onto the prefab.
   - Using objects from the scene is a bad idea, as it breaks all purpose of our components architecture and it would be problematic to test specific NPCs on a separate scenes.

5. Congratulations! You have created your first NPC that has the default test dialogues. Now you need to reassign them. Go to the root object of your prefab and change these things:

    - "First Node" - put the first dialogue node that you want to be played.
    - "Id" - set a unique identifier that will be used as a key in the save file. It's recommended to name it in this format: `[character_name]_actor` (everything in lowercase).
    - "Group" - set a group that you've created during [this step](#1-dialogue-groups-skippable-for-the-game-ending-dialogue).

    ![image](https://github.com/user-attachments/assets/abe7b301-8600-4834-b690-2e6578c3feb6)

6. And if you think that this is all, you're delusional. Now go to the [localized table](#localized-phrases) that you've used for your NPC's dialogues and make an entry for their name there (`[character_name]_name`).
7. Go to the `Speaker` object and in the `DialogueSpeaker` component assign your newly created entry in the `Name` property.

   ![image](https://github.com/user-attachments/assets/b1e27600-e6e2-4a8b-b091-e5388ebc7851)

8. Go to the `EntryTrigger` object. It is the interactable object that defines where the player can start the dialogue. Place it where you like it the most. Feel free to modify the collider properties (like radius, or you can remove it and replace with the box collider if you wish). I just don't recommend modifying the scale in the `Transform`, because it sometimes leads to strange bugs.
9. Go to the `ViewCamera` object and place it where you like. This is the view that the player will see after initiating a conversation. **Pro-tip:** select this object and go to `GameObject -> Align With View`, this will copy a view from the `Scene` window.

10. You're all set! The dialogues must work now. If not, you probably messed up somewhere, try to repeat all the steps. If nothing helps - make sure to note the warnings and/or errors in console if there are any.

#### Dialogue Node Triggers

Okay, now you want for dialogues to have an actual impact on something? Say no more! You have Dialogue Node Triggers for this that can be combined with any [action](#actions).

Open your NPC prefab and go to the `Triggers` object. Add new gameobjects there, I recommend adding one per node and then add triggers as children. 

You can also create separate objects for actions, especially when you have many of them and it's mandatory if you want to use more than one action of the same type. Try to name the triggers the same way you name dialogue nodes, but ommit the NPC name (e.g., for the `Clemence_Dlg1_No` you can name the trigger `Dlg1_No`).

The `DialogueNodeTrigger` in itself may be scary, but it won't be after you use it 5 times or so.

![image](https://github.com/user-attachments/assets/35289710-e590-40d7-8e66-570e5c898633)

- **Target Node** - a node for which this trigger activates. If "None", this trigger will work for all nodes.
- **Phrase Index** - an index of the phrase (starting from 0) that triggers the **Phrase Started** event.
- **Phrase Started** - an event that is called when the phrase with the **Phrase Index** starts. Use this event for cosmetic stuff or to show the narration screen.
- **Ended** - an event that is called when the node (**not a phrase!**) ends playing. Use this event to update quests, time and modify save events. It also can be used to hide the narration screen.

**Warning:** don't use nodes from different NPCs as this obviously won't work.

## Quests

Quests are pretty straightforward in this game. The only thing you need are quest and subquest files, quest/subquest triggers and actions.

Each quest can have its own subquests that can also be explained as "quest objectives" or "quest tasks".

![image](https://github.com/user-attachments/assets/48dccae6-f6e9-44eb-ab84-c12a210e2788)

### Quest/Subquest States

Both quests and subquests can have one of the 4 states:

1. **None** - the quest/subquest is hidden from the player, because it hasn't been started yet.
2. **Active** - the quest/subquest is in progress and shown as a "blank checkbox".

  ![image](https://github.com/user-attachments/assets/f9cee534-47b5-43ed-9bc5-55ea428ec867)

3. **Completed** - the quest/subquest has been completed and marked with a green check mark.

  ![image](https://github.com/user-attachments/assets/4bc868a2-6590-424f-bbb8-f5d05dbcfe8f)

4. **Failed** - the quest/subquest has been failed and marked with a red cross.

  ![image](https://github.com/user-attachments/assets/eeb1c4ba-900e-4aed-8836-ad332cbcc966)

**Note:** changing the quest's state if it is already Completed or Failed is more likely a bad idea, so you'll see warnings in the console when attempted to do so (though it's technically possible).

### Quest System

In order to use quests your scene must contain the `QuestSystem` prefab (find it using this prompt: `t:Prefab QuestSystem`). Also you can put your quest actions, triggers and related objects into this object (don't apply them to the `QuestSystem`, instead make your own prefab variant).

**Warning:** make sure that prefabs don't have dependencies on the scene objects.

### Creating Quests

In order to create a quest follow this steps:

1. Go to the `Assets/Quests` folder. Create a dedicated folder for your quest and name it appropriately.
2. Right click and select `Create -> Quests -> Quest`:

   ![image](https://github.com/user-attachments/assets/f2c436ff-df11-40d6-b79f-a4102f1ef4f9)

3. If you select this quest you will see its parameters

   ![image](https://github.com/user-attachments/assets/2ce01363-4db8-49c6-b93c-ca26ec38d7df)

    * **Name** - a name of the quest that is displayed in the UI. It's a localized string, all such strings must be put in the `Quests` localized table (use `Quests t:StringTableCollection` prompt).
    * **Fail On Time Over** - if checked, the quest and all of its subquests will be marked as failed once the time is over.
    * **Hidden** - if checked, the quest and all of its subquests will be hidden in the UI.
    * **Order** - the order of the quest in the UI. The lesser the value, the higher the quest. Quests with the same order will be sorted based on which quest was revealed first.

4. (Optional) Once you have finished configuring your quest it's time to create subquests for it. Right click and select `Create -> Quests -> Subquest`:

   ![image](https://github.com/user-attachments/assets/12bfaf00-fff8-4986-a80b-8602e88f4b6d)

5. If you select this subquest you will see its parameters

   ![image](https://github.com/user-attachments/assets/d375276f-8fff-4742-bc14-0507136c3027)

   
    * **Description** - a short description of the subquest that is displayed in the UI. It's a localized string, all such strings must be put in the `Quests` localized table (use `Quests t:StringTableCollection` prompt).
    * **Quest** - a quest that is "an owner" of this subquest. If you forget to set this property, you will most likely see warning and errors in the console.
    * **Hidden** - whether this subquest should be hidden in the UI. If the assigned quest is hidden, this setting will be ignored because all subquests of the hidden quest will be hidden by default.
    * **Order** - the order of the subquest in the UI. The lesser the value, the higher the subquest. Subquests with the same order will be sorted based on which subquest was revealed first.
    * **Action On Quest Passed** - what should happen with the subquest when the **Quest** is marked as **Completed**.
    * **Action On Quest Failed** - what should happen with the subquest when the **Quest** is marked as **Failed**.

5. (Optional) If you want for the quest/subquests to reveal themselves when the player launches the game use Quest/Subquest actions with the "Perform On Enable" checked and with the "Action" set to "Reveal":

   ![image](https://github.com/user-attachments/assets/f34c9ee8-1d41-455f-a86a-1e47d7800005)

   Revealing the subquest also reveals its quest.

Now you're ready to use quests in: 
  * [Triggers](#triggers) (`QuestStateTrigger`, `SubquestStateTrigger`).
  * [Actions](#actions) (`QuestAction`, `SubquestAction`).
  * [Dialogue Conditions](#dialogue-conditions) (`DialogueQuestStateCondition`, `DialogueSubquestStateCondition`).
  
## Actions

Actions are components that end with the `Action` suffix. Usually, they have a one important method that is called `Perform` and they are meant to be used together with Unity events (they are provided by the [Triggers](#triggers) and by some of the [Interactable Objects](#interactable-objects)).

### List of Actions

1. `DelayedAction` - an action that triggers a Unity Event after the specified delay in real seconds. It's not a "pure action" and feels more like "a glue". Can be paired with the `DelayedActionSaver` if the action progress should be saved (information about the action has started and how much time is left for it to be executed). Method: `Perform`.
2. `EndGameAction` - an action that was used in the Alpha version of the game. Its only purpose is to end the game. Method: `EndGame`.
3. `NarrationAction` - an action that allows to show/hide the narration screen. Methods: `ShowScreen`, `HideScreen`.
4. `DialogueSaveEventAction` - an action that manipulates the [dialogue save event](dialogue-save-events) state. Method: `Perform`.
5. `PlayerLockAction` - an action that allows to lock/unlock the player input. Methods: `Lock`, `Unlock`.
6. `QuestAction` - an action that changes the quest state. It has an additional flag "Perform On Enable` which if set, performs the action automatically when the object is enabled. Method: `Perform`.
7. `SubquestAction` - same as the `QuestAction` but for the subquests. Method: `Perform`.
8. `SpendTimeAction` - an action that spends the specified number of minutes. Method: `Perform`.

## Triggers

Triggers are components that usually end with the `Trigger` suffix. They provide Unity Event(s) that can be used to perform [Actions](#actions) or to interact with other objects. Some of the triggers need other components to exist in parent/children, but the majority of them can be put anywhere on the scene and still work. Please, put them somewhere appropriate so you'll be able to find them later.

To combine triggers with actions, just add both trigger and action to your game object and then in the trigger add an even listener (use the "+" button). Then, drag and drop your action object there, choose an appropriate component and select the method you're interested in (look the [List of Actions](#list-of-actions) section).

1. `ChainedTrigger` - a trigger that calls the event after all of the assigned to it triggers have called their event. In order work properly with the save system, the `ChainedTriggerSaver` component on the same object is required (you will get a warning if you forget about it). Note that not all triggers can be used with this component. This is used in the alpha version to end the game after the player had failed all subquests of the main quest.
2. `DialogueNodeTrigger` - a special dialogue trigger that is described [here](#dialogue-node-triggers). Cannot be chained.
3. `PlayerTrigger` - a trigger that calls the event when the player enters it. Requires a trigger collider. Can be chained.
4. `QuestStateTrigger` - a trigger that calls the event when the specified quest updates its state. If the `Initial State Check` option is checked, the state will be checked after loading the game. It has a different behaviour depending on the "Trigger Type" property:
    - `StateChanged` - event is called when the state is changed to anything.
    - `Started` - event is called only when the quest changes its state to `Active`.
    - `Completed` - event is called only when the quest changes its state to `Completed`.
    - `Failed` - event is called only when the quest changes its state to `Failed`.
    Can be chained.
5. `SubquestStateTrigger` - same as the `QuestStateTrigger` but for the subquests. Can be chained.
6. `TimeOverTrigger` - a trigger that calls the event after the [time is over](#clock). Additionally, the "Initial State Check" flag defines whether the trigger should call the event if the game has started with 0 minutes left. Can be chained.
