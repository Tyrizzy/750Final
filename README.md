# 750Final

Your Name

- Tyare Daniels

Project Title & Description

- Fly-Bi is a side scrolling action game where you have to avoid brick walls in order to make it to the end.

Genre

- Side-Scroller, Action

Your motivation

- I wanted to create something simple so I had time to implement all of the needed requirements. I took inspiration from flappy birds because of the parallax background. I then added my twists to the game with controller input and more.

Key technical aspects

Physics - I created my own PhysicsController script that handled things like linear drag, mass, gravity, objects velocity, and if the object is static. The goal was to have similar features that a Unity Rigidbody would have if applicable for my game.

- Quadtree Collision Detection - I created my own QuadTree class that handled all the logic to create the bounds, keep track of the level, nodes, and to find all of the objects in the scene that has my created BoundingBox script. This script mimics unity's 2D Box colliders using the sprite renderer to obtain the objects corners and draw them. For the actual detection of the collisions my CollsioionDetectionQuadTree2D script handles that; It constantly checks for the active Objects with my BoundingBox on them then if the two objects touch im using SAT to detect if the objects touched/overlap and if they do they will destroy the object, in my case only if the objects tag is "Player". If its not a player it will calculate the mtv and separate the objects.

- Anti-Cheat Detection & Code Structure - As for the anti-cheat i used the website that Prof. Niko gave us. The algorithm i choose 2.4. Patch ntdll!DbgUiRemoteBreakin() because it checks the ntdll!DbgUiRemoteBreakin() which is always called in tandem with kernel32!DebugActiveProcess(). In doing so when the code detects this it will immediately close the game. I also changed the stripping level to high and the scripting backend to IL2CPP to give it some more security.

- UI/UX - For the UI of the game I used my friend Tessla's UI system because it handles really well and fits what i needed it for. I then added my own canvas prefabs and design to it. I also had to create a system that allows you to use your controller to navigate the menus but actually changing the EventSystems first selected menu item.

- 9 Level Debugger - I created my own GameDebugger script that reads command line arguments to find if you wrote "750Final -GD:(1-9)". Depending on the number you put it it'll display those messages inside my custom DebuggerConsole assuming they are actively being called. It will always show you basic info (1) even if you don't start it from cmd. If there does happen to be any warnings or errors the colors of the text will be yellow and red respectively. I've also added buttons for the custom Console so that you can open, close, and clear it.

- Unit Tests - I initially tried to implement this and realized for my game I couldn't think of more than one kind of uni test which would test its performance by spawning a ton of my custom particles (Gameobjects) but I ended up scraping it.

Structure (if applicable)

- NA

Engine used and version#/build (UE, Unity, Cry, Godot, O3DE, etc.)

- Unity Version 2022.3.18f1

Dependencies, and if any packages need to be installed

- NA

Platform Target Build (Win, Linux, OS, iOS, MacOs, Android,etc) & Run Instructions

- Windows. 

- To Run the game download the builded verizon and open the exe.

Features - List the key features of the project. Briefly describe how these features work and their significance

- Controller Input - Used unity’s new input system to handle the controllers input for the game and UI. This allows you to play with either the arrow keys, A & D, and a controller.

- Moving Camera, Walls, WallSpawner, EndPointSpawner - The camera and walls are similar in function. The CameraMove script just translats the camera position by a specified number dictated by the difficulty * Time. The MovePipes script works the same except it uses the camera it find the left edge of the camera's nearClipPlane then when its positions is less then it will be destroyed. The PipeSwaner uses the cameras nearClipPlane to spawn the pipes on the right side of the camera at a random Y. The EndPointSpawner takes in a specified vector3 which is where the finish line will be at. It will also calculate the distance from the player to the finish line and update the UI to show how much further you have to go.

- Easy, Medium, and Hard Mode - EasyMode has the values PipeSpeed = 3, CameraSpeed = 3, SpawnTime = 3, EndPoint = new Vector3(50,0,0). MediumMode has the values PipeSpeed = 10, CameraSpeed = 3, SpawnTime = 1, EndPoint = new Vector3(75,0,0). HardMode has the values PipeSpeed = 20, CameraSpeed = 5, SpawnTime = 1, EndPoint = new Vector3(100,0,0).

Gameplay instructions (Controls) Keyboard, Mouse or other hardware if applicable

- Mouse and keyboard. A/arrow left and D/arrow right to move, Space to jump, Enter to select in menus.

- Controller Gamepad, Left and Right to move, X/A respectively to jump and select.

Known Issues/Notes - Bugs, issues or limitations with potential workarounds

- When switching scenes the menus UI breaks with controller input, however, if you die and restart in the level scene you'll be able to use your controller in the menus. Same happens if you go back to the main menu.

Credits (if you collaborated with another student/if applicable)

- I used fellow classmate Tessla's UI system from previous projects we worked on together.

Art/Design

- Simple shapes and textures. Found free parallax backgrounds.

UX/UI

- Simple UI

Audio

- No audio

Other

- NA

Screenshot or 10 second demo video.

Describe your additional subsystems.

- Particle System manager and implementation - Created my own ParticleSystem script that handles things like the color, mass, size, speed, lifetime, rotation, and a total particle count for my custom debugger. Inside my SpawnParticle function the speed is used to randomly have the particles move, the size controls the size of the gameobject, my physics script takes in the particle’s mass and the particle's velocity. 

- UI for Custom 9 level Debugger - Created my own debug console that will print out all of the basic info by default unless you access it from cmd and specify which number you want to look for. To create the console I used a canvas, slider, and 3 buttons.

- Controller Input - I used unity’s new input system to get the input of the controller.

- Parallax Scroller - I created a parallax script that takes in the background of choice then based on the number you assign it, it will move at that speed. Once the background reaches the end of the camera bounds it will repeat the background.

Post mortem and future work.
