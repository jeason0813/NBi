﻿using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Common
{
    abstract class LookupViolationMessage<T> : ILookupViolationMessageFormatter
    {
        public IDictionary<string, ISampler<DataRow>> Samplers { get; }

        protected T reference;
        protected T candidate;
        protected T analysis;

        public LookupViolationMessage(IDictionary<string, ISampler<DataRow>> samplers) => Samplers = samplers;

        public void Generate(IEnumerable<DataRow> referenceRows, IEnumerable<DataRow> candidateRows, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            var metadata = BuildMetadata(keyMappings, ColumnRole.Key, x => x.ReferenceColumn)
                .Union(BuildMetadata(valueMappings, ColumnRole.Value, x => x.ReferenceColumn));
            RenderStandardTable(referenceRows, metadata, Samplers["reference"], "Reference", reference);

            metadata = BuildMetadata(keyMappings, ColumnRole.Key, x => x.CandidateColumn)
                .Union(BuildMetadata(valueMappings, ColumnRole.Value, x => x.CandidateColumn));
            RenderStandardTable(candidateRows, metadata, Samplers["candidate"], "Candidate", candidate);
            RenderAnalysis(violations, metadata, Samplers["analysis"], keyMappings, valueMappings, analysis);
        }

        private IEnumerable<ColumnMetadata> BuildMetadata(ColumnMappingCollection mappings, ColumnRole role, Func<ColumnMapping, IColumnIdentifier> identify)
        {
            foreach (var mapping in mappings ?? new ColumnMappingCollection())
                yield return new ColumnMetadata()
                {
                    Identifier = identify.Invoke(mapping),
                    Role = role,
                    Type = mapping.Type,
                };
        }

        protected abstract void RenderStandardTable(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, string title, T writer);

        protected abstract void RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, T writer);

        public abstract string RenderReference();
        public abstract string RenderCandidate();
        public abstract string RenderAnalysis();
        public virtual string RenderPredicate() => "Some references are missing and violate referential integrity";

        public abstract string RenderMessage();
    }
}
