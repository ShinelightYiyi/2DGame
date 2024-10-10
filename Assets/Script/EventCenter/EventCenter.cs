using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork
{
    interface IEventInfo
    {

    }

    class EventInfo<T> :IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }            
    }


    class EventInfo :IEventInfo
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }



    class EventCenter
    {
        private static EventCenter instance;
        public static EventCenter Instance {get => instance ?? (instance = new EventCenter());}  

        private static Dictionary<string ,IEventInfo> eventDic = new Dictionary<string ,IEventInfo>();


        /// <summary>
        /// ��ӷ����¼�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        public void AddEventListener<T>(string eventName,UnityAction<T> action)
        {
            if(eventDic.ContainsKey(eventName))
            {
                (eventDic[eventName] as EventInfo<T>).actions += action;
                Debug.Log("�¼�" + eventName + "����ί��");
            }
            else
            {
                eventDic.Add(eventName,new EventInfo<T>(action));    
                Debug.Log("�½��¼�" +  eventName);
            }
        }


        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        public void AddEventListener(string eventName,UnityAction action)
        {
            if(eventDic.ContainsKey(eventName))
            {
                (eventDic[eventName] as EventInfo).actions += action;
                Debug.Log("�¼�" + eventName + "����ί��");
            }
            else
            {
                eventDic.Add(eventName,new EventInfo(action));
                Debug.Log("�½��¼�" + eventName);
            }
        }


        /// <summary>
        /// ��������ί��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="info"></param>
        public void EventTrigger<T>(string eventName,T info)
        {
            if(!eventDic.ContainsKey(eventName))
            {
                Debug.LogWarning("�¼�" + eventName + "������");
                return;
            }

            (eventDic[eventName] as EventInfo<T>).actions.Invoke(info);
        }



        /// <summary>
        /// ����ί��
        /// </summary>
        /// <param name="eventName"></param>
        public void EventTrigger(string eventName)
        {
            if (!eventDic.ContainsKey(eventName))
            {
                Debug.LogWarning("�¼�" + eventName + "������");
                return;
            }

            (eventDic[eventName] as EventInfo).actions.Invoke();
        }

        /// <summary>
        /// ����¼�,�л�������
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
        }


    }




}
