using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    // Initial value
    public int Initvalue;   

    // List of stat modifiers
    private List<int> statModifiers = new List<int>();

    // Add all modifiers together and return the result
    public int GetStat()
    {
        int retValue = Initvalue;
        statModifiers.ForEach(x => retValue += x);
        return retValue;
    }

    // Add a new modifier to the list
    public void AddStatModifier(int modifier)
    {
        if (modifier != 0)
            statModifiers.Add(modifier);
    }

    // Remove a modifier from the list
    public void RemoveStatModifier(int modifier)
    {
        if (modifier != 0)
            statModifiers.Remove(modifier);
    }

}