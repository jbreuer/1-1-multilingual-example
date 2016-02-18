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
	/// <summary>File</summary>
	[PublishedContentModel("File")]
	public partial class File : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "File";
		public new const PublishedItemType ModelItemType = PublishedItemType.Media;
#pragma warning restore 0109

		public File(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
            return UmbracoContext.Current.Facade.ContentCache.GetContentType(ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<File, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		///Size
		///</summary>
		[ImplementPropertyType("umbracoBytes")]
		public object UmbracoBytes
		{
			get { return this.GetPropertyValue("umbracoBytes"); }
		}

		///<summary>
		///Type
		///</summary>
		[ImplementPropertyType("umbracoExtension")]
		public object UmbracoExtension
		{
			get { return this.GetPropertyValue("umbracoExtension"); }
		}

		///<summary>
		///Upload file
		///</summary>
		[ImplementPropertyType("umbracoFile")]
		public object UmbracoFile
		{
			get { return this.GetPropertyValue("umbracoFile"); }
		}
	}
}
