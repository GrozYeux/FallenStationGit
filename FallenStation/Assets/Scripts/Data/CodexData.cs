using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class CodexData
{
    public HashSet<string> notes;
    public CodexData(CodexManager codex)
    {
        notes = codex.notes;
    }
}
