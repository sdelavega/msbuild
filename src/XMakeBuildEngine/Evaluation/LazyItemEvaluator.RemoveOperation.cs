﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Build.Evaluation
{
    internal partial class LazyItemEvaluator<P, I, M, D>
    {
        class RemoveOperation : LazyItemOperation
        {
            public RemoveOperation(OperationBuilder builder, LazyItemEvaluator<P, I, M, D> lazyEvaluator)
                : base(builder, lazyEvaluator)
            {
            }

            protected override ICollection<I> SelectItems(ImmutableList<ItemData>.Builder listBuilder, ImmutableHashSet<string> globsToIgnore)
            {
                return SelectItemsMatchingItemSpec(listBuilder, _itemElement.RemoveLocation);
            }

            protected override void SaveItems(ICollection<I> items, ImmutableList<ItemData>.Builder listBuilder)
            {
                listBuilder.RemoveAll(itemData => items.Contains(itemData.Item));
            }

            public ImmutableHashSet<string>.Builder GetRemovedGlobs()
            {
                var globs = _itemSpec.Fragments.OfType<GlobFragment>().Select(g => g.ItemSpecFragment);
                var builder = ImmutableHashSet.CreateBuilder<string>();

                builder.UnionWith(globs);

                return builder;
            }
        }
    }
}
