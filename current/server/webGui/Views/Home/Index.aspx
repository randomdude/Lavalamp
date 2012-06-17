<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<lavalamp.lavalampRuleInfo>>" %>
<%@ Import Namespace="lavalamp" %>
<%@ Import Namespace="ruleEngine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm("Index", "RuleEditor", FormMethod.Post)) { %>
    <table>
        <thead>
            <tr>
                <th>Rule State</th>
                <th>Name</th>
                <th>Details</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
       
    <% foreach (lavalampRuleInfo rule in Model)
       {%>
         <tr>
             <td><%= rule.state.HasValue ? rule.state.ToString() : "Unknown" %></td>
             <td><a href="noJS.html" class="post-link" ><%= rule.name %></a></td>
             <td><%= rule.details %></td>
             <td><% if (rule.state != ruleState.errored)
                    {%>
             <%=
                            Html.ActionLink(rule.state == ruleState.stopped ? "Run" : "Stop", "ChangeRuleState", rule) %>
            <% } %>
               <%= Html.ActionLink("Delete", "deleteRule", rule) %>
            </td>
            
         </tr>   
       <% } %>
       </tbody>
    </table>
    <% } %>
</asp:Content>
