﻿using System;
using System.Data;
using System.Collections.Generic;
using NBi.Core.ResultSet;
using NBi.NUnit;
using NBi.NUnit.Member;
using NBi.NUnit.Structure;
using NUnit.Framework.Constraints;
using NF = NUnit.Framework;

namespace NBi.NUnit.FluentInterface
{
    public class Is : NF.Is
    {

        public Is()
        {
        }

        public static SyntacticallyCorrectConstraint SyntacticallyCorrect()
        {
            return new SyntacticallyCorrectConstraint();
        }

        public static FasterThanConstraint FasterThan(int maxTimeMilliSeconds)
        {
            var ctr = new FasterThanConstraint();
            ctr.MaxTimeMilliSeconds(maxTimeMilliSeconds);
            return ctr;
        }

        public static EqualToConstraint EqualTo(ResultSet resultSet)
        {
            var ctr = new EqualToConstraint(resultSet);
            return ctr;
        }

        public static EqualToConstraint EqualTo(IDbCommand command)
        {
            var ctr = new EqualToConstraint(command);
            return ctr;
        }

        public static EqualToConstraint EqualTo(IEnumerable<IRow> rows)
        {
            var ctr = new EqualToConstraint(rows);
            return ctr;
        }

        public static OrderedConstraint Ordered()
        {
            var ctr = new OrderedConstraint();
            return ctr;
        }

        
    }
}