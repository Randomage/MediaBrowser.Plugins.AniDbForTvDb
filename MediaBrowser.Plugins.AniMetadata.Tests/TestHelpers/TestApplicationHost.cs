﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Events;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Updates;
using NSubstitute;
using SimpleInjector;

namespace MediaBrowser.Plugins.AniMetadata.Tests.TestHelpers
{
    public class TestApplicationHost : IApplicationHost
    {
        public TestApplicationHost()
        {
            Container = new Container();

            Container.Register(() => Substitute.For<IApplicationPaths>());
            Container.Register(() => Substitute.For<IHttpClient>());
            Container.Register(() => Substitute.For<ILogManager>());
            Container.Register<IApplicationHost>(() => this);

            Container.GetInstance(typeof(ILogManager));
        }

        protected readonly Container Container;

        public void NotifyPendingRestart()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetExports<T>(bool manageLiftime = true)
        {
            throw new NotImplementedException();
        }

        public Task<CheckForUpdateResult> CheckForApplicationUpdate(CancellationToken cancellationToken, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(PackageVersionInfo package, CancellationToken cancellationToken, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            return (T)Container.GetInstance(typeof(T));
        }

        public T TryResolve<T>()
        {
            throw new NotImplementedException();
        }

        public Task Shutdown()
        {
            throw new NotImplementedException();
        }

        public void RemovePlugin(IPlugin plugin)
        {
            throw new NotImplementedException();
        }

        public Task Init(IProgress<double> progress)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public string OperatingSystemDisplayName { get; }
        public string Name { get; }
        public string SystemId { get; }
        public bool HasPendingRestart { get; }
        public bool IsShuttingDown { get; }
        public bool CanSelfRestart { get; }
        public Version ApplicationVersion { get; }
        public bool CanSelfUpdate { get; }
        public bool IsFirstRun { get; }
        public List<string> FailedAssemblies { get; }
        public Type[] AllConcreteTypes { get; }
        public IPlugin[] Plugins { get; }
        public PackageVersionClass SystemUpdateLevel { get; }
        public event EventHandler<GenericEventArgs<PackageVersionInfo>> ApplicationUpdated;
        public event EventHandler HasPendingRestartChanged;
    }
}