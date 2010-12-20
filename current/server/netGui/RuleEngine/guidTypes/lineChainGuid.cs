﻿using System;

namespace netGui.RuleEngine
{
    public class lineChainGuid
    {
        public Guid id = Guid.NewGuid();

        public lineChainGuid() { }

        public lineChainGuid(string newGuid)
        {
            id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return id.ToString();
        }
    }
}