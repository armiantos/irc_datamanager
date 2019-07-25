﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public interface IDatabase
    {
        void Connect(string dbHost, string dbUser, string dbPass);

        void Disconnect();

        List<string> ListDatabases();

        List<string> ListCollections();
    }
}
