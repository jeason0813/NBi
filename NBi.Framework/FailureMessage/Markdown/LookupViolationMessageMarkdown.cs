﻿using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Common.Helper;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown
{
    abstract class LookupViolationMessageMarkdown : LookupViolationMessage<MarkdownContainer>
    {

        public LookupViolationMessageMarkdown(IDictionary<string, ISampler<DataRow>> samplers)
            : base(samplers)
        { }

        protected override void RenderStandardTable(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, string title, MarkdownContainer container)
        {
            sampler.Build(rows);
            var tableHelper = new StandardTableHelperMarkdown(rows, metadata, sampler);
            tableHelper.Render(container);
        }

        protected virtual IEnumerable<IColumnDefinition> BuildMetadata(ColumnMappingCollection mappings, ColumnRole role, Func<ColumnMapping, IColumnIdentifier> identify)
        {
            foreach (var mapping in mappings ?? new ColumnMappingCollection())
                yield return new Column()
                {
                    Identifier = identify.Invoke(mapping),
                    Role = role,
                    Type = mapping.Type,
                };
        }

        public override string RenderReference() => reference.ToMarkdown();
        public override string RenderCandidate() => candidate.ToMarkdown();
        public override string RenderAnalysis() => analysis.ToMarkdown();

        private string Textify(RowViolationState s)
        {
            switch (s)
            {
                case RowViolationState.Missing: return "Missing";
                case RowViolationState.Unexpected: return "Unexpected";
                case RowViolationState.Mismatch: return "Non-matching";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected string GetExplanationText(LookupViolationCollection violations, RowViolationState state)
        {
            string Pluralize(int x) => x > 1 ? "s" : string.Empty;
            string Verbalize(int x) => x > 1 ? "are" : "is";
            string PluralizeVerb(int x) => x > 1 ? string.Empty : "s";
            string This(int x) => x > 1 ? $"These {x} distinct" : $"This";
            string Textify(RowViolationState s, int x)
            {
                switch (s)
                {
                    case RowViolationState.Missing: return $"missing. It means {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} not available in the system-under-test but {Verbalize(x)} found in the result-set defined in the assertion";
                    case RowViolationState.Unexpected: return $"unexpected. It means {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} available in the system-under-test but {Verbalize(x)} not found in the result-set defined in the assertion";
                    case RowViolationState.Mismatch: return $"non-matching. It means the values associated to {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} not equal in the candidate and reference tables";
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            string GetText(int x, int y) => $"{x} distinct key{Pluralize(x)} found in the candidate table {Verbalize(x)} {Textify(state, x)}. {This(x)} key{Pluralize(x)} appear{PluralizeVerb(x)} in {y} row{Pluralize(y)} of the candidate table.";

            var count = violations.Where(x => x.Value.State == state).Count();
            var countRow = violations.Where(x => x.Value.State == state).Sum(x => x.Value.Rows.Count());
            return GetText(count, countRow);
        }

        public override string RenderMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine(RenderPredicate());
            sb.AppendLine();
            sb.AppendLine(RenderReference());
            sb.AppendLine();
            sb.AppendLine(RenderCandidate());
            sb.AppendLine();
            sb.AppendLine(RenderAnalysis());
            return sb.ToString();
        }
    }
}
