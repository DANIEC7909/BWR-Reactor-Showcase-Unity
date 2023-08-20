using UnityEditor;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{

    public static T Instance {get; private set;}
    protected void Init(object o)
    {
        Instance = (T)o;
        Object[] go = FindObjectsOfType(typeof(T));
        if (go.Length > 1)
        {
#if UNITY_EDITOR

            EditorGUIUtility.PingObject(go[0]);
            Debug.LogError("This Object have to be destroyed because in Game already Exists this object");
           /* foreach (Object ob in go)
            {
                foreach (Object g in go)
                {
                    if ((typeof(T)ob).gameObject*//* ((typeof(T))g).gameObject*//*)
                    {
                        Destroy(ob);
                        Debug.LogError("One GameObject contins two instances of the same script!");
                    }
                    return;
                }
            }*/
#endif
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
}
