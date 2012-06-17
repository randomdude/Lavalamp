<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul>
    <li>
       <%= Html.ActionLink("All Rules","Index","Home") %>
    </li>
    <li>
         <%= Html.ActionLink("New Rule", "Index", "RuleEditor")%>
    </li>
    <li>
        <%= Html.ActionLink("V", "Index", "RuleEditor")%>
    </li>
</ul>