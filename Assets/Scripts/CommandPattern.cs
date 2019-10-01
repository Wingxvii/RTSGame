using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPattern : MonoBehaviour
{
    #region SingletonCode
    //singleton pattern begins here
    private static readonly CommandPattern instance = new CommandPattern();
    static CommandPattern() { }
    private CommandPattern() { }
    public static CommandPattern Instance { get { return instance; } }
    //single pattern ends here
    #endregion

    //stack of undo and redo commands
    private Stack<ICommand> _Undocommands = new Stack<ICommand>();
    private Stack<ICommand> _Redocommands = new Stack<ICommand>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
