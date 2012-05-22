﻿using System.Collections;
using System.Collections.Generic;
using NBi.Core.Analysis.Member;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ContainsConstraint : NUnitCtr.Constraint 
    {
        protected List<string> captions;
        protected IComparer comparer;
        
        /// <summary>
        /// .ctor, this class doesn't make usage of an engine
        /// </summary>
        public ContainsConstraint()
        {
            captions = new List<string>();
            comparer = new Member.ComparerByCaption(true);
        }
        
        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint IgnoreCase
        {
            get
            {
                comparer = new Member.ComparerByCaption(false);
                return this;
            }
        }

        public ContainsConstraint Caption(string value)
        {
            this.captions.Add(value);
            return this;
        }

        public ContainsConstraint Captions(ICollection<string> values)
        {
            this.captions.AddRange(values);
            return this;
        }

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is ICollection)
                return Matches((ICollection)actual);

            return false;
        }
        
        /// <summary>
        /// Handle a ICollection
        /// </summary>
        /// <param name="actual">an ICollection</param>
        /// <returns></returns>
        public bool Matches(ICollection actual)
        {
            bool res = (captions.Count>0);
            
            foreach (var member in captions)
	        {
                var ccc = new CollectionContainsConstraint(new MemberCaption(member));
                res &= ccc.Using(comparer).Matches(actual);
	        }

            return res;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("contains");
            
            if (captions.Count==1)
                writer.WriteExpectedValue(captions[0]);
            else
                writer.WriteExpectedValue(captions);
        }

       
    }
}
