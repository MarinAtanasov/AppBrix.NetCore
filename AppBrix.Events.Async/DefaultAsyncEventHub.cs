// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppBrix.Events.Async
{
    internal sealed class DefaultAsyncEventHub : IAsyncEventHub, IApplicationLifecycle
    {
        #region IApplicationLifecycle implementation
        public void Initialize(IInitializeContext context)
        {
            this.app = context.App;
        }

        public void Uninitialize()
        {
            this.taskQueues.Keys.ToList().ForEach(this.RemoveTaskQueue);
        }
        #endregion

        #region IAsyncEventHub implementation
        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            this.SubscribeInternal(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            this.UnsubscribeInternal(handler);
        }

        public void Raise<T>(T args) where T : IEvent
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            this.RaiseEvent(args, typeof(T));
            this.RaiseBaseClassesEvents(args);
            this.RaiseInterfacesEvents(args);
        }
        #endregion

        #region Private methods
        private void SubscribeInternal<T>(Action<T> handler) where T : IEvent
        {
            ITaskQueue queueObject;
            if (this.taskQueues.TryGetValue(typeof(T), out queueObject))
            {
                ((TaskQueue<T>)queueObject).Subscribe(handler);
            }
            else
            {
                this.CreateTaskQueue<T>().Subscribe(handler);
            }
        }

        private TaskQueue<T> CreateTaskQueue<T>() where T : IEvent
        {
            var queue = new TaskQueue<T>();
            this.taskQueues[typeof(T)] = queue;
            this.app.GetEventHub().Subscribe<T>(this.RaiseEvent);
            this.taskQueueUnsubscribers[typeof(T)] = () => this.app.GetEventHub().Unsubscribe<T>(this.RaiseEvent);
            return queue;
        }

        private void UnsubscribeInternal<T>(Action<T> handler) where T : IEvent
        {
            ITaskQueue queueObject;
            if (this.taskQueues.TryGetValue(typeof(T), out queueObject))
            {
                var queue = (TaskQueue<T>)queueObject;
                queue.Unsubscribe(handler);
                if (queue.Count == 0)
                {
                    this.RemoveTaskQueue(typeof(T));
                }
            }
        }

        private void RemoveTaskQueue(Type type)
        {
            this.taskQueueUnsubscribers[type].Invoke();
            this.taskQueueUnsubscribers.Remove(type);
            this.taskQueues[type].Dispose();
            this.taskQueues.Remove(type);
        }

        private void RaiseBaseClassesEvents<T>(T args) where T : IEvent
        {
            var baseType = typeof(T).GetTypeInfo().BaseType;
            while (baseType != null && typeof(IEvent).GetTypeInfo().IsAssignableFrom(baseType))
            {
                this.RaiseEvent(args, baseType);
                baseType = baseType.GetTypeInfo().BaseType;
            }
        }

        private void RaiseInterfacesEvents<T>(T args) where T : IEvent
        {
            foreach (var iface in typeof(T).GetTypeInfo().GetInterfaces().Where(i => typeof(IEvent).GetTypeInfo().IsAssignableFrom(i)))
            {
                this.RaiseEvent(args, iface);
            }
        }

        private void RaiseEvent<T>(T args) where T : IEvent
        {
            this.RaiseEvent(args, typeof(T));
        }

        private void RaiseEvent<T>(T args, Type eventType) where T : IEvent
        {
            ITaskQueue queueObject;
            if (this.taskQueues.TryGetValue(eventType, out queueObject))
            {
                queueObject.Enqueue(args);
            }
        }
        #endregion

        #region Private fields and constants
        private readonly IDictionary<Type, ITaskQueue> taskQueues = new Dictionary<Type, ITaskQueue>();
        private readonly IDictionary<Type, Action> taskQueueUnsubscribers = new Dictionary<Type, Action>();
        private IApp app;
        #endregion
    }
}