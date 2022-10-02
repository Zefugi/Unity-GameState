using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(fileName = "New Incident", menuName = "GameState/Incident")]
    public class Incident : ScriptableObject
    {
        [SerializeField] bool _logInConsole;
        [SerializeField] Color _color = Color.green;
        private string _consoleFormatString= "<color=#{0}>{1}</color> invoked by <color=#{0}>{2}</color>\n{3}";

        private List<(object Subscriber, IncidentInvokeDelegate Delegate)> _subscribers = new List<(object, IncidentInvokeDelegate)>();
        private int _invocationDepth = 0;
        private List<(object Subscriber, IncidentInvokeDelegate Delegate)> _subscribersToAdd = new List<(object Subscriber, IncidentInvokeDelegate Delegate)>();
        private List<int> _subscribersToRemove = new List<int>();

        public void Subscribe(object subscriber, IncidentInvokeDelegate incidentCallback)
            => (_invocationDepth == 0 ? _subscribers : _subscribersToAdd).Add((subscriber, incidentCallback));

        public void Unsubscribe(object subscriber)
        {
            if (_invocationDepth == 0)
                _subscribers.RemoveAt(GetIndexOfSubscriber(subscriber));
            else
                _subscribersToRemove.Add(GetIndexOfSubscriber(subscriber));
        }

        private int GetIndexOfSubscriber(object subscriber)
        {
            for (int i = 0; i < _subscribers.Count; i++)
                if (_subscribers[i].Subscriber == subscriber)
                    return i;
            return -1;
        }

        private (object Subscriber, IncidentInvokeDelegate Deletate) GetTupleOfSubscriber(object subscriber)
        {
            for (int i = 0; i < _subscribers.Count; i++)
                if (_subscribers[i].Subscriber == subscriber)
                    return _subscribers[i];
            return (null, null);
        }

        public void Invoke(object invoker, object information = null)
        {
            _invocationDepth++;
            if (_logInConsole)
                Debug.LogFormat(_consoleFormatString, ColorUtility.ToHtmlStringRGB(_color), name, invoker, information?.ToString());
            for (int i = 0; i < _subscribers.Count; i++)
                _subscribers[i].Delegate.Invoke(invoker, information);
            _invocationDepth--;

            if (_subscribersToAdd.Count != 0)
            {
                _subscribers.AddRange(_subscribersToAdd);
                _subscribersToAdd.Clear();
            }

            if(_subscribersToRemove.Count != 0)
            {
                for (int i = 0; i < _subscribersToRemove.Count; i++)
                    _subscribers.RemoveAt(_subscribersToRemove[i]);
                _subscribersToRemove.Clear();
            }
        }
    }
}
