using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//include UnityEditor to let the window be created
using UnityEditor;
using UnityEngine.AI;

//the window which lets the user create waypoints in an easier fashion
public class AIManagerWindow : EditorWindow
{
    //create the ability to open the window
    [MenuItem("Tools/AI Manager")]
    static void Open()
    {
        //create the window
        GetWindow<AIManagerWindow>();
    }

    //get the parent that the waypoints will be put under
    public Transform AIHolder;

    //used for navmesh generation
    //NavMeshSurface is part of the high level Unity NavmeshComponents
    //Can be accessed from https://github.com/Unity-Technologies/NavMeshComponents
    //the Unity Github Page
    private NavMeshSurface navMeshSurface;
    private GameObject navMesh;

    //GUI
    private void OnGUI()
    {
        //create a reference to the menu object
        SerializedObject obj = new SerializedObject(this);

        //need a parent object for the AI gameobjects
        EditorGUILayout.PropertyField(obj.FindProperty("AIHolder"));

        //if there is no gameobject parent for the AI parent
        if (AIHolder == null)
        {
            //let the user know that there is no parent in which the AI will be children of
            EditorGUILayout.HelpBox("Root transform must be selected", MessageType.Warning);
        }
        //draw the buttons
        //start the button box
        EditorGUILayout.BeginVertical("box");
        //call function draw button
        DrawButtons();
        //end the button box
        EditorGUILayout.EndVertical();
        //apply any modifications to the menu
        obj.ApplyModifiedProperties();
    }

    //how to draw the buttons
    void DrawButtons()
    {
        if (AIHolder == null) {

            //Create Parent Button
            if (GUILayout.Button("Create AI Parent"))
            {
                createParent();
            }
        }

        if (AIHolder != null)
        {
            //creates a navmesh
            if (GUILayout.Button("Create Navmesh"))
            {
                createNavmesh();
            }
            //checks to see if there is a navmesh surface to rebake
            if (navMeshSurface != null)
            {
                //rebakes the navmesh
                if (GUILayout.Button("Rebake Navmesh"))
                {
                    bakeNavmesh();
                }
            }
            //basic player button
            if (GUILayout.Button("Basic Player"))
            {
                basicPlayer();
            }
            //basic AI button
            if (GUILayout.Button("Basic AI"))
            {
                basicAI();
            }
            //chase player AI Button
            if (GUILayout.Button("Chase Player AI"))
            {
                chasePlayer();
            }
            //advanced AI Button
            if (GUILayout.Button("AdvancedAI"))
            {
                detectAndChasePlayer();
            }
        }
    }

    //create a parent for the ai to be stored in
    void createParent() {
        //create an empty gameobject
        GameObject AIParent = new GameObject("AI Root");
        //make the AIholder the new gameobject
        AIHolder = AIParent.transform;
    }

    //create the navmesh
    void createNavmesh() {

        //create the navmesh
        navMesh = new GameObject("NavMesh", typeof(NavMeshSurface));
        navMesh.transform.SetParent(AIHolder, false);
        //build the navmesh
        navMeshSurface = navMesh.GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
        //select the navmesh Object
        Selection.activeGameObject = navMesh.gameObject;
    }

    //bakes the navmesh
    void bakeNavmesh() {
        //bake the navmesh
        navMeshSurface.BuildNavMesh();
        //select the game object
        Selection.activeGameObject = navMesh.gameObject;
    }

    //create a basic player
    void basicPlayer() {

        //create the new basic player
        GameObject basicPlayer = new GameObject("BasicPlayer " + AIHolder.childCount, typeof(PlayerMove));
        basicPlayer.transform.SetParent(AIHolder, false);

        //add the character controller component
        basicPlayer.AddComponent<CharacterController>();
        //give it the player tag
        basicPlayer.tag = "Player";

        //add in a trigger capsule
        basicPlayer.AddComponent<CapsuleCollider>().isTrigger = true;
        //add in a collider capsule
        basicPlayer.AddComponent<CapsuleCollider>();

  

        //create a basic capsule
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.SetParent(basicPlayer.transform, false);

        //ignore the player when baking the navmesh
        basicPlayer.AddComponent<NavMeshModifier>().ignoreFromBuild = true;
        cube.AddComponent<NavMeshModifier>().ignoreFromBuild = true;

        //select the new player
        Selection.activeGameObject = basicPlayer.gameObject;

    }
    //create a basic AI that follows a path
    void basicAI() {

        //create the new basic ai
        GameObject basicAI = new GameObject("BasicAI " + AIHolder.childCount, typeof(WaypointNavigator));
        basicAI.transform.SetParent(AIHolder, false);

        //add the navmesh agent component
        basicAI.AddComponent<NavMeshAgent>();
        basicAI.GetComponent<NavMeshAgent>().baseOffset = 1.0f;

        //create a basic capsule
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.SetParent(basicAI.transform, false);

        //ignore the AI when baking the navmesh
        basicAI.AddComponent<NavMeshModifier>().ignoreFromBuild = true;
        cube.AddComponent<NavMeshModifier>().ignoreFromBuild = true;

        //select the new ai
        Selection.activeGameObject = basicAI.gameObject;

    }

    //create an AI that will only chase the player
    void chasePlayer() {

        //create the new chase ai
        GameObject chaseAI = new GameObject("ChaseAI " + AIHolder.childCount, typeof(WaypointNavigator));
        chaseAI.transform.SetParent(AIHolder, false);

        //add the navmesh agent component
        chaseAI.AddComponent<NavMeshAgent>();
        chaseAI.GetComponent<NavMeshAgent>().baseOffset = 1.0f;
        //makes it so the ai will always chase the player
        chaseAI.GetComponent<WaypointNavigator>().playerNotSeen = false;

        //create a basic capsule
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.SetParent(chaseAI.transform, false);

        //ignore the AI when baking the navmesh
        chaseAI.AddComponent<NavMeshModifier>().ignoreFromBuild = true;
        cube.AddComponent<NavMeshModifier>().ignoreFromBuild = true;

        //select the new ai
        Selection.activeGameObject = chaseAI.gameObject;
    }

    //create a more advanced AI
    void detectAndChasePlayer() {

        //create the new advanced ai
        GameObject advancedAI = new GameObject("advancedAI " + AIHolder.childCount, typeof(WaypointNavigator));
        advancedAI.transform.SetParent(AIHolder, false);

        //add the navmesh agent component
        advancedAI.AddComponent<NavMeshAgent>();
        advancedAI.GetComponent<NavMeshAgent>().baseOffset = 1.0f;
        //add the detect script
        advancedAI.AddComponent<Detect>();
        //set it so it is an advanced AI
        advancedAI.GetComponent<Detect>().isadvancedAI = true;
        
        //create a basic capsule
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.SetParent(advancedAI.transform, false);

        //ignore the AI when baking the navmesh
        advancedAI.AddComponent<NavMeshModifier>().ignoreFromBuild = true;
        cube.AddComponent<NavMeshModifier>().ignoreFromBuild = true;

        //select the new ai
        Selection.activeGameObject = advancedAI.gameObject;
    }
}