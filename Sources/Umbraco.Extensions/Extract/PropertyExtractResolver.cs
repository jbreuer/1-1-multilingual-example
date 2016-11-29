// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtractResolver.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The property extract resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Umbraco.Core.ObjectResolution;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;

    /// <summary>
    /// The property extract resolver.
    /// </summary>
    public class PropertyExtractResolver : ManyObjectsResolverBase<PropertyExtractResolver, PropertyExtractBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyExtractResolver"/> class.
        /// </summary>
        /// <param name="extractItems">
        /// The extract items.
        /// </param>
        public PropertyExtractResolver(IEnumerable<Type> extractItems)
            : base(extractItems, ObjectLifetimeScope.Application)
        {
        }

        /// <summary>
        /// Gets the property extract items.
        /// </summary>
        public List<PropertyExtractBase> PropertyExtractItems
        {
            get
            {
                return this.Values.ToList();
            }
        }
    }
}