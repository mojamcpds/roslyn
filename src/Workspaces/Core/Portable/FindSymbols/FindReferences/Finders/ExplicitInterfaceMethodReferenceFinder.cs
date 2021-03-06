﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.FindSymbols.Finders
{
    internal class ExplicitInterfaceMethodReferenceFinder : AbstractReferenceFinder<IMethodSymbol>
    {
        protected override bool CanFind(IMethodSymbol symbol)
        {
            return symbol.MethodKind == MethodKind.ExplicitInterfaceImplementation;
        }

        protected override Task<IEnumerable<SymbolAndProjectId>> DetermineCascadedSymbolsAsync(
            SymbolAndProjectId<IMethodSymbol> symbolAndProjectId,
            Solution solution,
            IImmutableSet<Project> projects,
            CancellationToken cancellationToken)
        {
            // An explicit interface method will cascade to all the methods that it implements.
            return Task.FromResult(
                symbolAndProjectId.Symbol.ExplicitInterfaceImplementations.Select(
                    ei => symbolAndProjectId.WithSymbol((ISymbol)ei)));
        }

        protected override Task<IEnumerable<Document>> DetermineDocumentsToSearchAsync(
            IMethodSymbol symbol,
            Project project,
            IImmutableSet<Document> documents,
            CancellationToken cancellationToken)
        {
            // An explicit method can't be referenced anywhere.
            return SpecializedTasks.Default<IEnumerable<Document>>();
        }

        protected override Task<IEnumerable<ReferenceLocation>> FindReferencesInDocumentAsync(
            IMethodSymbol symbol,
            Document document,
            CancellationToken cancellationToken)
        {
            // An explicit method can't be referenced anywhere.
            return SpecializedTasks.Default<IEnumerable<ReferenceLocation>>();
        }
    }
}
