﻿using System;
using System.ComponentModel.Composition;
using DaxStudio.Interfaces;
using Caliburn.Micro;
using Serilog;
using System.Windows;
using DaxStudio.Common;

namespace DaxStudio.Standalone
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IDaxStudioHost))]
    public class DaxStudioHost 
        : IDaxStudioHost
    {
        private int _port;
        private IDaxStudioProxy _proxy;
        private IEventAggregator _eventAggregator;
        private string _commandLineFileName = string.Empty;
        private Application _app;

        //private UI.ViewModels.DocumentViewModel _activeDocument;
        [ImportingConstructor]
        public DaxStudioHost(IEventAggregator eventAggregator, Application app)
        {
            _eventAggregator = eventAggregator;
            _app = app;
            _port = _app.Args().Port;

            if (Port > 0)
            {
                _proxy = new DaxStudio.UI.Model.ProxyPowerPivot(_eventAggregator, _port);
            } else
            {
                _proxy = new DaxStudio.UI.Model.ProxyStandalone();
            }


            //string[] args = Environment.GetCommandLineArgs();
            //if (args.Length > 1)
            //{
            //    int.TryParse(args[1], out _port);
            //}
            //if (_port > 0)
            //{
            //    Log.Debug("{class} {method} {message} {port}", "DaxStudioHost", "ctor", "Constructing ProxyPowerPivot", _port);
            //    _proxy = new DaxStudio.UI.Model.ProxyPowerPivot(_eventAggregator, _port);
            //}
            //else
            //{
            //    // pass along commandline to UI
            //    Log.Debug("{class} {method} {message}", "DaxStudioHost", "ctor", "constructing ProxyStandalone");
            //    if (args.Length > 1) {
            //        if (args[1].ToLower() != "-log" ) _commandLineFileName = args[1];
            //    }
            //    _proxy = new DaxStudio.UI.Model.ProxyStandalone();
            //}
        }

        public bool IsExcel {
            get { return _proxy.IsExcel; }
        }

        public IDaxStudioProxy Proxy
        {
            get { return _proxy; }
        }

        public string CommandLineFileName
        {
            get { return  _app.Args().FileName; }
        }

        public void Dispose()
        {
            
        }


        public ADOTabular.AdomdClientWrappers.AdomdType ConnectionType
        {
            get { return ADOTabular.AdomdClientWrappers.AdomdType.AnalysisServices; }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public bool DebugLogging
        {
            get
            {
                return (bool)_app.Args().LoggingEnabled;
            }
        }
    }
}
