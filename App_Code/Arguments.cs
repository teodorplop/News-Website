using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Arguments
/// </summary>
public class Arguments {
	public string domain;
	public string orderBy;
	public string sortOrder;
	public string search;
	public Arguments() {
		domain = orderBy = sortOrder = search = null;
	}
	public Arguments(HttpRequest request) {
		domain = request.QueryString["Domain"];
		orderBy = request.QueryString["OrderBy"];
		sortOrder = request.QueryString["SortOrder"];
		search = request.QueryString["Search"];
	}
}