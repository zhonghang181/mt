
using System;
using System.Collections.Generic;

public class CustomEvent
{
    public string Name;
    public object Data;

    public CustomEvent(string name, params Object[] data) { Name = name; Data = data.Length > 0? data[0] : null; }
}

public delegate void Handler(CustomEvent evt);

public class Notification
{
    private static Notification instance = null;

    private Dictionary<string, List<Handler>> eventListerners = new Dictionary<string, List<Handler>>();

    //Single 
    public static Notification Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Notification();
            }
            return instance;
        }        
    }

    public void On(string evtName, Handler listener)
    {
        if (!eventListerners.ContainsKey(evtName))
        {
            eventListerners.Add(evtName, new List<Handler>());
        }
        eventListerners[evtName].Add(listener);
    }

    public void Off(string evtName, Handler listener)
    {
        if (!eventListerners.ContainsKey(evtName))
        {
            return;
        }
        eventListerners[evtName].Remove(listener);
    }

    public void Emit(string evtName, params Object[] data)
    {
        if (eventListerners.ContainsKey(evtName))
        {
            List<Handler>.Enumerator enumerator = eventListerners[evtName].GetEnumerator();
            while(enumerator.MoveNext())
            {
                enumerator.Current(new CustomEvent(evtName, data));
            }
        }
    }
}