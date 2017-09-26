# MoonBehavior

Behavior tree system for unity3d.

You can create AI behaviors for your unity games with a great visual editor.

![Screenshot](https://github.com/pedro15/MoonBehavior/raw/master/Images/ScreenShot.PNG)

## Usange:

### Moving nodes:

You can move nodes by two ways:

![MovingNodes](https://media.giphy.com/media/xT9IgpzFgXYsxLHVDO/giphy.gif)

it enables you editing your nodes with ease.

### Saving data:

![Saving data image](https://github.com/pedro15/MoonBehavior/raw/master/Images/SavingData.PNG)

"SaveGraph" : Saves the current graph ( it's like your behavior tree "Proyect" )

"Save BehaviorTree" : Saves the behaviorTree for use in your AI's.

Paths and Names: 

DO NOT EDIT THE FILE NAMES of the saved data.

Graphs are default saved at: "Assets/MoonBehavior/Graphs" 

BehaviorTrees default saved at: "Assets/MoonBehavior/BehaviorTrees"

(You can modify the paths at settings.)

### AI Memory:

![Memory image](https://github.com/pedro15/MoonBehavior/blob/master/Images/MoonAI_Memory.PNG)

The AI's memory are used for saving local data in each AI agent.

To assign and remove elements:

![Memory image2](https://github.com/pedro15/MoonBehavior/blob/master/Images/MoonAI_Memory2.PNG)

The Inspector Supported data are: STRING,FLOAT,INT,BOOLEAN,VECTOR2,VECTOR3,VECTOR4,OBJECT (Every unity assignable type (GameObject,audioclip,scriptableObject,Sprite,Texture2D etc..)),
COLOR and LAYERMASK. 

To edit an element:

![Memory image3](https://github.com/pedro15/MoonBehavior/blob/master/Images/MoonAI_Memory3.PNG)

It needs to have an "key" to identify each element (it is used in code to get and set the value).

You can also asign the other data types via custom actions ( The memory stores the data in object type so it support every type of data.)

getting and set from memory:

```csharp

public MoonAI ai ; // The AI Instance

// It uses generics , so you need to specify the data type between the <> 

float speed = ai.Memory.GetValue<float>("Speed"); 

// To set value:

// It takes the first parameter (string) as the key , and the second parameter (object) as the value 
// the memory doesn't care if the key was assigned previusly or not. (if the element don't exists it creates the element)

ai.Memory.SetValue("Key", 20f);

```

