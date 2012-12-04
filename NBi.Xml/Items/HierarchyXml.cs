﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class HierarchyXml : DimensionXml
    {
        [XmlAttribute("dimension")]
        public string Dimension { get; set; }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }

        [XmlIgnore]
        protected virtual string ParentPath { get { return string.Format("[{0}]", Dimension); } }
        [XmlIgnore]
        protected  override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

        [XmlIgnore]
        public override string TypeName
        {
            get { return "hierarchy"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:dimension", Dimension);
            return dico;
        }
    }
}
