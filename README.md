# K-Syndicate Unity Game Template

The main idea of the template is managed game loading. This is achieved by using a state machine for game phases (Bootstrap, Loading, Game Menu, Gameplay Levels) and avoiding the use of Awake in MonoBehaviour scripts.

This concept is proposed by the developers at Knowledge Syndicate. The implementation is based on this approach and utilizes Zenject, Addressables, and UniTask.

The template includes a basic UI for level selection, game settings, and authorization, as well as a basic UI for gameplay scenes. The UI is implemented using the MVP Passive View pattern.

**A foundation is prepared for:**

- Integrating analytics (e.g., GameAnalytics)
- Implementing ads
- Localization system
- Authorization integration
- Music management and volume control
- Progress saving
- Managing in-game currency and player statistics
- And more

**Dependencies:**

- Zenject
- Addressables
- UniTask
- TextMesh Pro
- Odin Inspector
- Odin Validator

**Design Patterns Used:**

- Dependency Injection (DI)
- State
- EventBus
- Factory
- Model View Presenter Passive View
- Facade
- Template Method
- Proxy
- and more

## Flow Description
![Flow](/images/flow-scheme.png)

**Entry Point Scene**

This is the entry point of the game.
The purpose of this scene is simply to load the Bootstrap scene, which is placed in Addressables. 
The goal is to prevent asset duplication from Addressables groups. 
Therefore, this scene does not contain any assets from the project and only loads the Bootstrap scene.

**Bootstrap Scene**

It creates the game's state machine and sequentially initializes all services to ensure their correct operation.

**Loading Scene**

This scene loads the game state before showing the player the main menu. If the user is authorized, their account information is loaded. Then, the game transitions to the state of loading the player's saved progress. Any other state that needs to be executed before showing the player the main game menu can be inserted here.

**Game Hub Scene**

The main game menu. The scene's state begins with Bootstrap, where the necessary services for the game menu scene can be initialized. After the Bootstrap scene state, the Main scene state is activated, and the game menu appears. In this menu, the player can select a level to play, access settings, and perform authorization. If authorization is successful, the state transitions to Loading, the Loading Scene is loaded, and the game reloads data considering the player's authorization. If authorization fails, the game state does not change.

**Gameplay Scene**

The Gameplay state loads the level, which creates the level's state machine. The level state starts with the Bootstrap state, where all the services needed solely for gameplay can be initialized.

Next, the state transitions to Start. Here, for example, the player can be shown an advertisement, and a message can be sent to analytics about the start of the level. After this, the scene transitions to the Play state, where the level is shown to the player, and the game can begin. There is also a Pause state, which the player enters by pressing the pause button on the screen. From the Pause state, the player can either return to the game or exit it via the UI.

Upon exiting, the game transitions to the Finish state. Here, as well, an advertisement can be shown to the player, and data can be sent to analytics about the end of the game. The Finish state transitions to the Game Hub state, and the main menu scene is loaded.
