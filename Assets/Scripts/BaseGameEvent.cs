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
    {//�¼����� ���캯�����Type ��Sender��ֵ
        get { return this.m_type; }
        set { this.m_type = value; }
    }

    public Object Sender
    {//����
        get { return this.m_Sender; }
        set { this.m_Sender = value; }
    }

    public override string ToString()
    {
        return this.m_type + "[" + ((this.m_Sender == null) ? "null" : this.m_Sender.ToString()) + "]";
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="type"> �¼����� </param>
    /// <param name="sender"> ���� </param>
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

public delegate void GameEventListenerDelegate(BaseGameEvent gameEvent);//����ί�����ڴ��¼�����

public class GameEventDispatcher
{
    static GameEventDispatcher Instance;
    public static GameEventDispatcher GetInstance()//����
    {
        if (Instance == null)
        {
            Instance = new GameEventDispatcher();
        }
        return Instance;
    }

    private Hashtable listeners = new Hashtable(); //�ƿ��������͵�ί���¼�

    public void AddEventListener(GameEventType type, GameEventListenerDelegate listener)
    {
        GameEventListenerDelegate gameEventListenerDelegate = this.listeners[type] as GameEventListenerDelegate;//���֮ǰ������͵�ί�� �����һ�ε���Null 
        gameEventListenerDelegate = (GameEventListenerDelegate)Delegate.Combine(gameEventListenerDelegate, listener);//������ί�еĵ����б�������һ��,��Ϊһ���µ�ί��
        this.listeners[type] = gameEventListenerDelegate;//��ֵ����ϣ���е��������
    }

    public void RemoveEventListener(GameEventType type, GameEventListenerDelegate listener)
    {
        GameEventListenerDelegate gameEventListener = this.listeners[type] as GameEventListenerDelegate;//���֮ǰ������͵�ί�� �����һ�ε���Null
        if (gameEventListener != null)
        {
            gameEventListener = (GameEventListenerDelegate)Delegate.Remove(gameEventListener, listener);//��hEventListener�ĵ����б����Ƴ�listener
            this.listeners[type] = gameEventListener;//��ֵ����ϣ���е��������
        }
    }

    public void DispatchEvent(BaseGameEvent baseGame)
    {
        GameEventListenerDelegate gameEventListener = this.listeners[baseGame.Type] as GameEventListenerDelegate;
        if (gameEventListener != null)
        {
            try
            {
                gameEventListener(baseGame);//ִ��ί��
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
