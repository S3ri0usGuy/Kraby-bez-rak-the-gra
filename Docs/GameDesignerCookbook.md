# An Ultimate Guide on How to Cook Quests and Dialogues (without using code)

![image](https://github.com/user-attachments/assets/ea64c03f-ae5a-4d6d-a73a-882bc9b5844e)

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
##### 1. **Dialogue Groups** (skippable for the game ending dialogue). 

For the NPCs, each dialogue node must belong to the dialogue node group (it's the saving system requirement). So, when making dialogue nodes for characters, in the root character folder (e.g. `Assets/Dialogues/Characters/Main/Clemence`) create the `Dialogue Nodes Group`:

![image](https://github.com/user-attachments/assets/240e56c0-b78e-4b33-a80a-3a5be5421e1d)

And, that's all for the node groups! Your only requirement from now is to you put all your nodes inside this folder (or its subfolders). Do not try to update the
node groups manually, as they populate themselves automatically.

**Important:** Please, give the groups proper names, preferably in this format: `[CharacterName]DialogueGroup`.

##### 2. **Dialogue Nodes**.

So you've created the node group, your next step is to create your first node. It's done in the same way as groups, but you just need to choose a different option:

![image](https://github.com/user-attachments/assets/e7f082a4-e2c2-4e6d-a691-0e8566094a64)

Once you've created a node, it should look like this:

![image](https://github.com/user-attachments/assets/fa5b6947-edab-4a1b-87e4-d94d688177d4)

**Important:** Please, give the nodes proper names, preferably in this format: `[CharacterName]_Dlg[SequenceNo]` for the "parent node" of the dialogue sequence and `[CharacterName]_Dlg[SequenceNo]_[EntryName]`. Examples: `Michele_Dlg1`, `Michele_Dlg1_Yes`.

##### 3. **Phrases**

**Tip:** hover your mouse on the properties - this will show you tooltips.

In order to add a phrase, just press the "+" button:

![image](https://github.com/user-attachments/assets/5a46e6dc-7eee-45ff-8606-900b0d4677f3)

The phrases will be played at the same order as in this array. Of course, you're free to reorder and remove them however you want.

Now, let's see how an average phrase looks like:

![image](https://github.com/user-attachments/assets/e7e36c2c-ef59-411f-8de6-b83ee7936dbd)

1. **Speaker Index** - an index of the speaker who says this phrase. Usually, it's 0 for the protagonist, 1 for the NPC, 2 for the narrator (depends on the `DialoguePlayer` settings). This index is required for the systems (subtitles and audio players) to differentiate who says what.
2. **Text** - a localized text of the phrase that is displayed in subtitles. See the [Localized Phrases](#localized-phrases) section for more details.
3. **Default Audio Clip** (optional) - an audio clip that will be played during the phrase if the **Audio Clip** is not assigned.
4. **Audio Clip** (optional) - a localized audio clip that will be played during the phrase. See the [Localized Voice Lines](#localized-voice-lines) section for more details.
5. **Duration** - how long the phrase lasts (in seconds). If the audio clip (either **Default Audio Clip** or **Audio Clip**) is assigned, and this value is zero or negative, the clip's duration is taken instead.
6. **Unskippable** - if checked, the phrase will not be skippable; otherwise the player can press any key to skip the phrase.

#### Localized Phrases

All text phrases are localized. This means, that you must create localization table entries for each phrase. My recommandation is to create a separate localization tables for main characters and other characters that may have huge amount of dialogues. For others, use the default one: `Assets/Localization/Tables/Dialogues/Dialogues` (please, keep phrases of the same character close to each other).

##### **Creating a New Localization Table for Dialogue Phrases**

It's advised to create a new localization table per each main character. For the side characters you can reuse the default one.

1. Open the **Localization Tables** window (`Window -> Asset Management -> Localization Tables`).
![image](https://github.com/user-attachments/assets/0a527b99-5472-458a-8912-ef20d6d91909)

2. Press `+ New Table Collection` (top left corner).
![image](https://github.com/user-attachments/assets/b6627df0-43f1-4d61-9e8b-3c1a521ef99b)

3. Name it in this pattern: `[EntityName]Dialogues`, make sure that all locales are selected and the type is set to "String Table Collection".
![image](https://github.com/user-attachments/assets/83d703b8-4514-4d81-befb-648c95618e74)

4. Smash the `Create` button.
5. Select an appropriate folder. It must follow this convention: `Assets/Dialogues/.../[EntityCharacterName]/Localization/Text` (more about it #dialogues-folder-structure-important). Example: `Assets/Dialogues/Characters/Main/Demo/Localization/Text`.
6. If you see this window then you probably did everything OK so far. Next steps are optional but recommended.
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
