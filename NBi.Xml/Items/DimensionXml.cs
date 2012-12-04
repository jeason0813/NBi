﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class DimensionXml : AbstractMembersItem
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        public override object Instantiate()
        {
            //TODO Here?
            return null;
        }

        [XmlIgnore]
        protected virtual string Path { get { return string.Format("[{0}]", Caption); } }

        public override string TypeName
        {
            get { return "dimension"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:perspective", Perspective);
            return dico;
        }
    }
}
