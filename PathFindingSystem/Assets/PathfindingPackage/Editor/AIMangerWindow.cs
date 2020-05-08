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

        //select the new player
        Selection.activeGameObject = basicPlayer.gameObject;

    }
    //create a basic AI
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
        
        //select the new ai
        Selection.activeGameObject = basicAI.gameObject;

    }

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

        //select the new ai
        Selection.activeGameObject = chaseAI.gameObject;
    }

    void detectAndChasePlayer() {

        //create the new advanced ai
        GameObject advancedAI = new GameObject("advancedAI " + AIHolder.childCount, typeof(WaypointNavigator));
        advancedAI.transform.SetParent(AIHolder, false);

        //add the navmesh agent component
        advancedAI.AddComponent<NavMeshAgent>();
        advancedAI.GetComponent<NavMeshAgent>().baseOffset = 1.0f;
        //add the detect script
        advancedAI.AddComponent<Detect>();

        //add in a sphere collider
        advancedAI.AddComponent<SphereCollider>().radius = 5.0f;
        advancedAI.GetComponent<SphereCollider>().isTrigger = true;

        //create a basic capsule
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.SetParent(advancedAI.transform, false);

        //select the new ai
        Selection.activeGameObject = advancedAI.gameObject;
    }
}