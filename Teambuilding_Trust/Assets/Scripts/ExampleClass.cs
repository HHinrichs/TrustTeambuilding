using UnityEngine;              // usings only those which are actually used
using System;                   // Try to group them based on the namespace
using System.Collections;

namespace ExampleProject.ExampleSystem // Use namespaces always
{
    public class ExampleClass : MonoBehaviour, IExampleInterface // Pascal case, nouns
    {
        // General conventions for classes:
        // Keep them as small as possible they shouldn't become giant monolithic classes.
        // 1 - 150      lines = good
        // 150 - 300    lines = danger zone
        // 300+         lines = here is something wrong. This class probably does too much.
        //
        // When you start using regions, this should be a clear sign that your class is too long.

        // Enums
        [System.Serializable]                               // Write [System.Serializable] so it's easier distinguishable from [SerializeField]
        public enum ExampleClassStates                      // Pascal, nouns (Tip: use plural so you can use singular as variable name)
        {
            NotInitialized,
            Initialized
        }

        // Structs
        [System.Serializable]
        public struct ExampleClassData                      // Pascal case, nouns
        {

        }

        // Classes
        [System.Serializable]
        public class ExampleClassSubClass                   // Pascal case, nouns
        {

        }

        // Delegate definition
        delegate void MyDelegate();                         // Pascal case, nouns or adjectives and they should end with Delegate

        // General conventions for naming variables:
        // Avoid abbreviations except in cases where it’s a domain specific common term.
        //      Example: Prefer playerWeapon over pWeap
        //      Exception Example:  Prefer GPU over GraphicalProcessingUnit
        //
        // Avoid single character names except as loop iterators with "i"
        //      If multiple iterators are needed, give them proper names
        //      only this short variables are fine: i, j, x, y, z, u, v, w, id

        // Constants
        const float NUMBER_OF_PI = 3.14159265f;             // All in capital letter with underscores for space replacement

        // Static variables
        public static ExampleClass Instance = null;         // Pascal case, nouns or adjectives
        
        // Public variables
        public bool Visibility = false;                     // Pascal case, nouns or adjectives, be careful with public variables rather make them a serialized field and create a getter and setter. Be as inclosed as possible.
        [SerializeField] float Height = 100.0f;             // Pascal case, nouns or adjectives

        // Private variables
        bool initialized = false;                           // Camel case, nouns or adjectives

        // Events
        public event Action<float> OnHeightChanged;         // Camel case, scheme "OnSomethingHappend"
        public event Action<float> UpdateUIEvent;           // use either On at start of the name or Event at end to mark them as events

        // Getter + Setter
        public bool GetInitialized { get { return initialized; } }  // Pascal case, nouns or adjectives (helpful is the use of Get or Is at the start of the variable name)
        public bool IsInitialized => initialized;                   // alternative to avoid all the braces ({})

        public float SetHeight { set { Height = value; } }

        // Example of a bad getter, because of too many lines, this reduces easy readability
        public float BadGetter
        {
            get
            {
                return Height;
            }
        }
        
        public float SetHeightBad { set { Height = value; UpdateUIEvent?.Invoke(Height); } } // DON'T DO THIS!!! This setter does more than setting the height this should be a function
        // end of example

        // In classes that are not derived from MonoBehaviour here comes the constructor(s) and after them the destructor
        /*
        public ExampleClass()
        {

        }

        public ~ExampleClass()
        {

        }
        */
        // DON'T comment out old code DELETE it, if you don't need it anymore. Aleternative: mark it as obsolete with: [Obsolete("This function is obsolete! Use NEW_FUNCTION_NAME instead.")]

        // Here comes a set of unity functions Awake, Start, OnEnable, OnDisable, OnDestory, Update functions
        // Awake function
        void Awake()
        {
            // Get all you references here and cache as much as possible
        }

        // Start function
        void Start()
        {

        }

        // OnEnable function
        void OnEnable()
        {

        }

        // OnDisable function
        void OnDisable()
        {

        }

        // OnDestroy function
        void OnDestroy()
        {

        }

        // Updates functions
        void Update()
        {

        }

        void FixedUpdate()
        {

        }

        void LateUpdate()
        {

        }

        // Public functions
        public void DoSomething(bool writeErrorToLog = true) // Pascal case and verbs, parameters in functions are in camel case and giving a devault value is nice
        {
            // Here are some guide lines that are of intrest inside of functions
            if (!initialized)               // Always try the early out, when possible
                return;                     // No brackets if only 1 line

            int errorCode = 0;              // Camel case and nouns or adjectives for variables inside functions

            for (int i = 0; i < 5; i++)     // use i as interator index or camel case nouns or adjectives
                Calculate();                // You don't need to use brackets if only 1 line but it's ok to use them in for, foreach, while, do while loops

            for (int i = 0; i < 5; i++)     // The usage with brackets is fine but you are breaking the DRY principle (DON'T REPEAT YOURSELF). Make it a function when you start copying code.
            {
                Calculate();
            }

            errorCode = Height < 0.0f ? 1 : 0;      // This a shorter way for writing an if else
            if (Height < 0.0f)                      // All this
                errorCode = 1;                      // lines are
            else                                    // doing the same as
                errorCode = 0;                      // the singe line before this if else

            if (writeErrorToLog)
                Debug.LogError("[ExampleClass] " + gameObject.name + " DoSomething: Has an error | error code: " + errorCode.ToString());
        }

        public void SetHeightAndUpdateUI(float height) // Good setter with event invoking example
        {
            Height = height;

            UpdateUIEvent?.Invoke(Height);          // The ? is a nice trick for a null check
            if (UpdateUIEvent != null)              // The line above does the same as this
                UpdateUIEvent.Invoke(Height);       // two lines
        }

        // Physics functions
        void OnCollisionEnter(Collision collision)
        {

        }

        void OnCollisionStay(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Tag"))        // Always use compare tag don't use collision.gameObject.tag == "Tag"
                return;                                         // Espacially in Stay functions implement an early out
        }

        void OnCollisionExit(Collision collision)
        {

        }

        void OnTriggerEnter(Collider collider)
        {

        }

        void OnTriggerStay(Collider collider)
        {

        }

        void OnTriggerExit(Collider collider)
        {

        }

        // Interface implementations
        public void InterfaceDoSomething() // look at interface example for more infos
        {
            throw new System.NotImplementedException();
        }

        // Private functions
        void Calculate() // Pascal case and verbs
        {

        }

        // Coroutine functions
        IEnumerator OpenCoroutine() // Pascal case, verbs and are ending with Coroutine to mark them as coroutines
        {
            yield break;
        }
    }
}

// Yes, I know this class is too long.