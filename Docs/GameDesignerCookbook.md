# An Ultimate Guide on How to Cook Quests and Dialogues (without using code)

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

### Dialogue Nodes