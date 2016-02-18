//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Zbu.ModelsBuilder v2.1.5.54
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Zbu.ModelsBuilder;
using Zbu.ModelsBuilder.Umbraco;

namespace Umbraco.Extensions.Models
{
	/// <summary>News Overview</summary>
	[PublishedContentModel("umbNewsOverview")]
	public partial class UmbNewsOverview : UmbMaster
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "umbNewsOverview";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public UmbNewsOverview(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
            return UmbracoContext.Current.Facade.ContentCache.GetContentType(ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<UmbNewsOverview, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}
	}
}
