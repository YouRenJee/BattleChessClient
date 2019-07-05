using System.Collections;
using System.Collections.Generic;
using SGF.Base;



public class ModuleManager : ServiceModule<ModuleManager>
{
    
    public delegate object MyAction(params object[] arg);
    private Dictionary<string, MyAction> dic = new Dictionary<string, MyAction>();
    private Dictionary<string, object[]> dicPre = new Dictionary<string, object[]>();

    public void AddListener(string eventName, MyAction call)
    {
        if (!dic.ContainsKey(eventName))
        {
            dic.Add(eventName, new MyAction(call));
        }
        else
        {
            dic[eventName] += call;
        }
       
        if (dicPre.ContainsKey(eventName))
        {
            call(dicPre[eventName]);
        }
    }

    public object Invoke(string eventName, params object[] arg)
    {
        
        if (dicPre.ContainsKey(eventName))
        {
            dicPre[eventName] = arg;
        }
        else
        {
            dicPre.Add(eventName, arg);
        }
        if (dic.ContainsKey(eventName))
        {
            return dic[eventName].Invoke(arg);
        }
        return null;
    }

    public void RemoveListener(string eventName, MyAction call)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName] -= call;
        }
    }

}
