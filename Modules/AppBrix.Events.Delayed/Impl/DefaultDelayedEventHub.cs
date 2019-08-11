// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Events.Delayed.Configuration;
using AppBrix.Lifecycle;
using System;
using System.Linq;
using System.Threading.Channels;

namespace AppBrix.Events.Delayed.Impl
{
    internal sealed class DefaultDelayedEventHub : IEventHub, IDelayedEventHub, IApplicationLifecycle
    {
        #region IApplicationLifecycle implementation
        public void Initialize(IInitializeContext context)
        {
            this.app = context.App;
            this.channel = Channel.CreateUnbounded<IEvent>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = true,
                SingleReader = true,
                SingleWriter = false
            });
            this.config = this.app.GetConfig<DelayedEventsConfig>();
            this.eventHub = this.app.GetEventHub();
        }

        public void Uninitialize()
        {
            if (this.channel != null)
            {
                this.channel.Writer.Complete();
                this.Flush();
            }

            this.eventHub = null;
            this.config = null;
            this.channel = null;
            this.app = null;
        }
        #endregion

        #region IEventHub implementation
        public void Subscribe<T>(Action<T> handler) where T : IEvent => this.eventHub.Subscribe(handler);

        public void Unsubscribe<T>(Action<T> handler) where T : IEvent => this.eventHub.Unsubscribe(handler);

        public void Raise(IEvent args)
        {
            switch (this.config.DefaultBehavior)
            {
                case EventBehavior.Immediate:
                    this.RaiseImmediate(args);
                    break;
                case EventBehavior.Delayed:
                    this.RaiseDelayed(args);
                    break;
                default:
                        throw new InvalidOperationException($@"{nameof(this.config.DefaultBehavior)}: {this.config.DefaultBehavior}");
            }
        }
        #endregion

        #region IDelayedEventHub implementation
        public void Flush()
        {
            lock (this.channel)
            {
                var reader = this.channel.Reader;
                while (reader.TryRead(out var args))
                {
                    this.RaiseImmediate(args);
                }
            }
        }

        public void RaiseDelayed(IEvent args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            this.channel.Writer.TryWrite(args);
        }

        public void RaiseImmediate(IEvent args) => this.eventHub.Raise(args);
        #endregion

        #region Private methods
        #endregion

        #region Private fields and constants
        private IApp app;
        private Channel<IEvent> channel;
        private DelayedEventsConfig config;
        private IEventHub eventHub;
        #endregion
    }
}
