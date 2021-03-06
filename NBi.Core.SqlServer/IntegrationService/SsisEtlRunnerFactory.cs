﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Etl;

namespace NBi.Core.SqlServer.IntegrationService
{
    public class SsisEtlRunnerFactory : IEtlRunnerFactory
    {
        public IEtlRunner Get(IEtl etl)
        {
            if (string.IsNullOrEmpty(etl.Server))
                return new EtlFileRunner(etl);
            #if ! SqlServer2008R2
            else if (!string.IsNullOrEmpty(etl.Catalog))
                return new EtlCatalogRunner(etl);
            #endif
            else if (string.IsNullOrEmpty(etl.UserName))
                return new EtlDtsWindowsRunner(etl);
            else
                return new EtlDtsSqlServerRunner(etl);
        }
    }
}
