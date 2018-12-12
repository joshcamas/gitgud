using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

namespace GitGud
{
    public class Commit
    {
        public string hash;
        public string tree;
        public string parent;
        public string author_name;
        public string author_email;
        public string date;
        public string subject;

    }
}