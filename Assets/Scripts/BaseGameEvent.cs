using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MainMenuController;
using Object = UnityEngine.Object;

public class BaseGameEvent
{
    protected Hashtable m_arguments;
    protected GameEventType m_type;
    protected Object m_Sender;

    public IDictionary Params
    {
        get { return this.m_arguments; }
        set { this.m_arguments = (value as Hashtable); }
    }

    public GameEventType Type
    {//事件类型 构造函数会给Type 和Sender赋值
        get { return this.m_type; }
        set { this.m_type = value; }
    }

    public Object Sender
    {//物体
        get { return this.m_Sender; }
        set { this.m_Sender = value; }
    }

    public override string ToString()
    {
        return this.m_type + "[" + ((this.m_Sender == null) ? "null" : this.m_Sender.ToString()) + "]";
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"> 事件类型 </param>
    /// <param name="sender"> 物体 </param>
    public BaseGameEvent(GameEventType type, Object sender)
    {
        this.Type = type;
        Sender = sender;
        if (this.m_arguments == null)
        {
            this.m_arguments = new Hashtable();
            Debug.LogError("this.m_arguments" + this.m_arguments.Count);
        }
    }

    public BaseGameEvent(GameEventType type, Hashtable args, Object sender)
    {
        this.Type = type;
        this.m_arguments = args;
        Sender = sender;

        if (this.m_arguments == null)
        {
            this.m_arguments = new Hashtable();
        }
    }

    public BaseGameEvent Clone()
    {
        return new BaseGameEvent(this.m_type, this.m_arguments, Sender);
    }
}

public delegate void GameEventListenerDelegate(BaseGameEvent gameEvent);//定义委托用于传事件基类

public class GameEventDispatcher
{
    static GameEventDispatcher Instance;
    public static GameEventDispatcher GetInstance()//单利
    {
        if (Instance == null)
        {
            Instance = new GameEventDispatcher();
        }
        return Instance;
    }

    private Hashtable listeners = new Hashtable(); //掌控所有类型的委托事件

    public void AddEventListener(GameEventType type, GameEventListenerDelegate listener)
    {
        GameEventListenerDelegate gameEventListenerDelegate = this.listeners[type] as GameEventListenerDelegate;//获得之前这个类型的委托 如果第一次等于Null 
        gameEventListenerDelegate = (GameEventListenerDelegate)Delegate.Combine(gameEventListenerDelegate, listener);//将两个委托的调用列表连接在一起,成为一个新的委托
        this.listeners[type] = gameEventListenerDelegate;//赋值给哈希表中的这个类型
    }

    public void RemoveEventListener(GameEventType type, GameEventListenerDelegate listener)
    {
        GameEventListenerDelegate gameEventListener = this.listeners[type] as GameEventListenerDelegate;//获得之前这个类型的委托 如果第一次等于Null
        if (gameEventListener != null)
        {
            gameEventListener = (GameEventListenerDelegate)Delegate.Remove(gameEventListener, listener);//从hEventListener的调用列表中移除listener
            this.listeners[type] = gameEventListener;//赋值给哈希表中的这个类型
        }
    }

    public void DispatchEvent(BaseGameEvent baseGame)
    {
        GameEventListenerDelegate gameEventListener = this.listeners[baseGame.Type] as GameEventListenerDelegate;
        if (gameEventListener != null)
        {
            try
            {
                gameEventListener(baseGame);//执行委托
            }
            catch (Exception e)
            {
                throw new System.Exception(string.Concat(new string[] { "Error Dispatch event", baseGame.Type.ToString(), ":", e.Message, " ", e.StackTrace }), e);
            }
        }
    }

    public void RemoveAll()
    {
        this.listeners.Clear();
    }

}
